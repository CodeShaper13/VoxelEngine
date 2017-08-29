//#define MAX_LIGHT

using Assets.VoxelEngine.Render;
using fNbt;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Entities;
using VoxelEngine.Entities.Registry;
using VoxelEngine.Render;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Level {

    public class Chunk : MonoBehaviour {

        public const int SIZE = 15;
        public const int BLOCK_COUNT = Chunk.SIZE * Chunk.SIZE * Chunk.SIZE;

        public MeshFilter filter;
        private MeshCollider blockCollider;
        private MeshCollider triggerCollider;
        public World world;

        public Block[] blocks;
        public byte[] metaData;
        /// <summary> First 4 bits are block light, last 4 are sky light. </summary>
        public byte[] lightLevel;
        /// <summary> Holds all the TileEntities in the chunk.  BlockPos is in world coordinates. </summary>
        public Dictionary<BlockPos, TileEntityBase> tileEntityDict; //TODO replace with a faster collection type
        public BlockPos worldPos;
        public ChunkPos chunkPos;
        public Bounds chunkBounds;
        /// <summary> If true, the chunk has been changed and needs it's mesh to be rebaked. </summary>
        private bool isDirty;
        /// <summary> If true, the population world gen phase and lighting has been done. </summary>
        public bool hasDoneGen2;
        /// <summary> If true, the chunk should not be rendered and is ment to read from only.  Gen Phase 2 will still edit theses? </summary>
        public bool isReadOnly;
        [HideInInspector]
        public List<ScheduledTick> scheduledTicks;

        private void Awake() {
            this.filter = this.GetComponent<MeshFilter>();
            MeshCollider[] colliders = this.GetComponents<MeshCollider>();
            this.blockCollider = colliders[0];
            this.triggerCollider = colliders[1];

            this.blocks = new Block[Chunk.BLOCK_COUNT];
            this.metaData = new byte[Chunk.BLOCK_COUNT];
            this.lightLevel = new byte[Chunk.BLOCK_COUNT];
            this.tileEntityDict = new Dictionary<BlockPos, TileEntityBase>();
            this.scheduledTicks = new List<ScheduledTick>();
        }

        /// <summary>
        /// Acts like a constructor of a chunk.
        /// </summary>
        public void initChunk(World w, NewChunkInstructions instructions) {
            this.world = w;
            this.worldPos = instructions.chunkPos.toBlockPos();
            this.chunkPos = instructions.chunkPos;
            this.isReadOnly = instructions.isReadOnly;
            float radius = 7f;
            this.chunkBounds = new Bounds(new Vector3(radius + this.worldPos.x, radius + this.worldPos.y, radius + this.worldPos.z), new Vector3(Chunk.SIZE, Chunk.SIZE, Chunk.SIZE));
            this.name = "Chunk" + this.chunkPos.ToString();
        }

        private void Update() {
            if (!this.isReadOnly && this.isDirty) {
                this.renderChunk();
            }

            if(Main.isDeveloperMode) {
                DebugDrawer.bounds(this.chunkBounds, this.isReadOnly ? new Color(1, 0, 0, 0.25f) : this.hasDoneGen2 ? Color.blue : new Color(0, 1, 0, 0.25f));
            }
        }

        private void LateUpdate() {
            ScheduledTick tick;
            for (int i = this.scheduledTicks.Count - 1; i >= 0; i--) {
                tick = this.scheduledTicks[i];
                tick.timeUntil -= Time.deltaTime;
                if(tick.timeUntil <= 0) {
                    BlockPos pos = tick.pos - this.worldPos;
                    this.getBlock(pos.x, pos.y, pos.z).applyScheduledTick(this.world, tick.pos);
                    this.scheduledTicks.RemoveAt(i);
                }
            }
        }

        private void FixedUpdate() {
            if(!this.isReadOnly) {
                return;

                // Randomly tick blocks.
                int x, y, z;
                for (int i = 0; i < 3; i++) {
                    x = UnityEngine.Random.Range(0, Chunk.SIZE - 1);
                    y = UnityEngine.Random.Range(0, Chunk.SIZE - 1);
                    z = UnityEngine.Random.Range(0, Chunk.SIZE - 1);
                    this.getBlock(x, y, z).onRandomTick(this.world, x + this.worldPos.x, y + this.worldPos.y, z + this.worldPos.z, this.getMeta(x, y, z), i);
                }
                /*
                int i = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                for (int j = 0; j < 3; j++) {
                    x = (i >> j * 12) & 0x0F;     // 0  12
                    y = (i >> j * 12 + 4) & 0x0F; // 4  16
                    z = (i >> j * 12 + 8) & 0x0F; // 8  20
                    this.getBlock(x, y, z).onRandomTick(this.world, x + this.pos.x, y + this.pos.y, z + this.pos.z, this.getMeta(x, y, z), i);
                }
                */

                /*
                for(int k = this.scheduledTicks.Count - 1; k >= 0; k--) {
                    ScheduledTick tick = this.scheduledTicks[k];
                    tick.remainingTicks -= 1;
                    if(tick.remainingTicks <= 0) {
                        this.getBlock()
                        this.scheduledTicks.RemoveAt(k);
                    }
                }
                */
            }
        }

        /// <summary>
        /// Resets the chunk, clearing out fields and preparing it to be used again.
        /// </summary>
        public void resetChunk() {
            this.tileEntityDict.Clear();
            this.scheduledTicks.Clear();
            this.isDirty = false;
            this.hasDoneGen2 = false;
            this.isReadOnly = false;
            Array.Clear(this.blocks, 0, this.blocks.Length);
            Array.Clear(this.metaData, 0, this.metaData.Length);
            Array.Clear(this.lightLevel, 0, this.lightLevel.Length);
        }

        public Block getBlock(int x, int y, int z) {
            return this.blocks[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x];
        }

        public void setBlock(int x, int y, int z, Block block) {
            this.isDirty = true;
            this.blocks[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x] = block;
        }

        public int getMeta(int x, int y, int z) {
            return this.metaData[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x];
        }

        /// <summary>
        /// Sets the meta at (x, y, z) and dirties the chunk.
        /// </summary>
        public void setMeta(int x, int y, int z, int meta) {
            this.isDirty = true;
            this.metaData[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x] = (byte)meta;
        }

        public int getLight(int x, int y, int z) {
            #if (MAX_LIGHT)
                return 12;
            #endif
            return this.lightLevel[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x];
        }

        /// <summary>
        /// Sets the light level at (x, y, z) and dirties the chunk.
        /// </summary>
        public void setLight(int x, int y, int z, int amount) {
            this.isDirty = true;
            this.lightLevel[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x] = (byte)amount;
        }

        /// <summary>
        /// Checks if the passes world coordinates are within the chunk.
        /// </summary>
        public bool isInChunk(int worldX, int worldY, int worldZ) {
            int inChunkX = worldX - this.worldPos.x;
            int inChunkY = worldY - this.worldPos.y;
            int inChunkZ = worldZ - this.worldPos.z;
            return (
                inChunkX >= 0 && inChunkX < Chunk.SIZE &&
                inChunkY >= 0 && inChunkY < Chunk.SIZE &&
                inChunkZ >= 0 && inChunkZ < Chunk.SIZE);
        }

        /// <summary>
        /// Bakes the block meshes and light levels into the chunk.
        /// </summary>
        public void renderChunk() {
            CachedChunk3x3 cachedRegion = CachedChunk3x3.getNewRegion(this.world, this);
            if(!cachedRegion.allChunksLoaded()) {
                // Waiting for the lazy chunk loading to finish...
                return;
            }

            this.isDirty = false;

            MeshBuilder meshBuilder = RenderManager.getMeshBuilder();
            meshBuilder.useRenderDataForCol = true;

            Block currentBlock, neighborBlock;
            bool cachedIsSolid;
            Block[] surroundingBlocks = new Block[6];
            BlockPos dirPos;
            int x, y, z, i, facesCulled, x1, y1, z1, renderFace, x2, y2, z2;
            Direction direction;

            // Bake blocks into mesh.
            for (x = 0; x < Chunk.SIZE; x++) {
                for (y = 0; y < Chunk.SIZE; y++) {
                    for (z = 0; z < Chunk.SIZE; z++) {
                        currentBlock = this.getBlock(x, y, z);
                        if(currentBlock.renderer != null && currentBlock.renderer.bakeIntoChunks) {

                            renderFace = 0;

                            // Find the surrounding blocks and faces to cull.
                            facesCulled = 0;

                            for (i = 0; i < 6; i++) {
                                direction = Direction.all[i];
                                dirPos = direction.blockPos;
                                x1 = x + dirPos.x;
                                y1 = y + dirPos.y;
                                z1 = z + dirPos.z;
                                
                                if (x1 < 0 || y1 < 0 || z1 < 0 || x1 >= Chunk.SIZE || y1 >= Chunk.SIZE || z1 >= Chunk.SIZE) {
                                    neighborBlock = cachedRegion.getBlock(x1, y1, z1);
                                } else {
                                    neighborBlock = this.getBlock(x1, y1, z1);
                                }

                                cachedIsSolid = neighborBlock.isSolid;
                                if(!cachedIsSolid) {
                                    renderFace |= direction.renderMask;
                                }

                                if (currentBlock.renderer.lookupAdjacentBlocks) {
                                    surroundingBlocks[i] = neighborBlock;
                                }

                                if (cachedIsSolid) {
                                    facesCulled++;
                                }                                
                            }

                            // If at least one face is visible, render the block.
                            if(facesCulled != 6) {
                                // Populate the meshData with light levels.
                                meshBuilder.setLightLevel(0, 0, 0, this.getLight(x, y, z));
                                
                                // If the renderer requests it, pass the light levels into the meshBuilder
                                if(currentBlock.renderer.lookupAdjacentLight == true) {
                                    for(x2 = -1; x2 <= 1; x2++) {
                                        for (y2 = -1; y2 <= 1; y2++) {
                                            for (z2 = -1; z2 <= 1; z2++) {
                                                x1 = x + x2;
                                                y1 = y + y2;
                                                z1 = z + z2;
                                                if (x1 < 0 || y1 < 0 || z1 < 0 || x1 >= Chunk.SIZE || y1 >= Chunk.SIZE || z1 >= Chunk.SIZE) {
                                                    i = cachedRegion.getLight(x1, y1, z1);
                                                    //this.world.getLight(this.worldPos.x + x1, this.worldPos.y + y1, this.worldPos.z + z1); // cachedRegion.getLight(x1, y1, z1);
                                                } else {
                                                    i = this.getLight(x1, y1, z1);
                                                }
                                                meshBuilder.setLightLevel(x2, y2, z2, i);
                                            }
                                        }
                                    }
                                }
                                // Render the block.
                                currentBlock.renderer.renderBlock(currentBlock, this.getMeta(x, y, z), meshBuilder, x, y, z, renderFace, surroundingBlocks);
                            }
                        }
                    }
                }
            }

            // Set light UVs for tile entities that don't bake into the chunk.
            Material[] materials;
            Color lightColor;
            foreach (TileEntityBase te in this.tileEntityDict.Values) {
                if(te is TileEntityGameObject) {
                    x = te.posX - this.worldPos.x;
                    y = te.posY - this.worldPos.y;
                    z = te.posZ - this.worldPos.z;
                    materials = ((TileEntityGameObject)te).modelMaterials;
                    lightColor = RenderManager.instance.lightHelper.getColorFromBrightness(this.getLight(x, y, z));
                    for (i = 0; i < materials.Length; i++) {
                        materials[i].SetColor(LightHelper.COLOR_ID, lightColor);
                    }
                }
            }

            this.filter.mesh = meshBuilder.getGraphicMesh();
            this.blockCollider.sharedMesh = meshBuilder.getColliderMesh();
        }

        public NbtCompound writeToNbt(NbtCompound tag, bool deleteEntities) {
            tag.Add(new NbtByte("hasDoneGen2", this.hasDoneGen2 ? (byte)1 : (byte)0));
            byte[] blockBytes = new byte[Chunk.BLOCK_COUNT];
            int i, x, y, z;
            for (i = 0; i < Chunk.BLOCK_COUNT; i++) {
                blockBytes[i] = (byte)this.blocks[i].id;
            }
            tag.Add(new NbtByteArray("blocks", blockBytes));
            tag.Add(new NbtByteArray("meta", this.metaData));
            tag.Add(new NbtByteArray("light", this.lightLevel));

            // Tile Entites.
            NbtList list = new NbtList("tileEntities", NbtTagType.Compound);
            foreach(TileEntityBase te in this.tileEntityDict.Values) {
                list.Add(te.writeToNbt(new NbtCompound()));
            }
            tag.Add(list);

            // Scheduled ticks.
            list = new NbtList("scheduledTicks", NbtTagType.Compound);
            ScheduledTick tick;
            for(i = 0; i < this.scheduledTicks.Count; i++) {
                tick = this.scheduledTicks[i];
                list.Add(this.scheduledTicks[i].writeToNbt());
            }
            tag.Add(list);

            // Entites.
            Entity entity;
            List<Entity> entitiesInChunk = new List<Entity>();
            for (i = this.world.entityList.Count - 1; i >= 0; i--) {
                entity = this.world.entityList[i];
                if(!(entity is EntityPlayer)) {
                    x = MathHelper.floor((int)entity.transform.position.x / (float)Chunk.SIZE);
                    y = MathHelper.floor((int)entity.transform.position.y / (float)Chunk.SIZE);
                    z = MathHelper.floor((int)entity.transform.position.z / (float)Chunk.SIZE);

                    if (x == this.chunkPos.x && y == this.chunkPos.y && z == this.chunkPos.z) {
                        world.entityList.Remove(entity);
                        entitiesInChunk.Add(entity);
                    }
                }
            }
            NbtList list1 = new NbtList("entities", NbtTagType.Compound);
            for (i = 0; i < entitiesInChunk.Count; i++) {
                entity = entitiesInChunk[i];
                list1.Add(entity.writeToNbt(new NbtCompound()));
                if (deleteEntities) {
                    GameObject.Destroy(entity.gameObject);
                }
            }
            tag.Add(list1);

            return tag;
        }

        public void readFromNbt(NbtCompound tag) {
            this.hasDoneGen2 = tag.Get<NbtByte>("hasDoneGen2").ByteValue == 1 ? true : false;
            byte[] blockBytes = tag.Get<NbtByteArray>("blocks").ByteArrayValue;
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                this.blocks[i] = Block.getBlockFromId(blockBytes[i]);
            }
            this.metaData = tag.Get<NbtByteArray>("meta").ByteArrayValue;
            this.lightLevel = tag.Get<NbtByteArray>("light").ByteArrayValue;

            // Populate the tile entity dictionary.
            foreach(NbtCompound compound in tag.Get<NbtList>("tileEntities")) {
                BlockPos pos = new BlockPos(compound.Get<NbtInt>("x").Value, compound.Get<NbtInt>("y").Value, compound.Get<NbtInt>("z").Value);
                TileEntityBase te = TileEntityBase.getTileEntityFromId(this.world, pos, compound);
                te.readFromNbt(compound);
                this.world.addTileEntity(pos, te);
            }

            // Populate the tile entity dictionary.
            foreach (NbtCompound compound in tag.Get<NbtList>("scheduledTicks")) {
                this.scheduledTicks.Add(new ScheduledTick(compound));
            }

            // Spawn the entities that were saved in the chunk back into the world.
            foreach (NbtCompound compound in tag.Get<NbtList>("entities")) {
                int id = compound.Get<NbtInt>("id").Value;
                RegisteredEntity re = EntityRegistry.getRegisteredEntityFromId(id);
                if (re != null) {
                    this.world.spawnEntity(re, compound);
                } else {
                    print("Error!  Entity with an unknown ID of " + id + " was found!  Ignoring!");
                }
            }
        }

        public void setDirty() {
            if(!this.isReadOnly) {
                this.isDirty = true;
            }
        }
    }
}