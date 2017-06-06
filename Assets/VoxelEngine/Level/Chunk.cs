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

        private MeshFilter filter;
        private MeshCollider blockCollider;
        private MeshCollider triggerCollider;
        public World world;

        public Block[] blocks;
        public byte[] metaData;
        /// <summary> First 4 bits are block light, last 4 are sky light </summary>
        public byte[] lightLevel;
        /// <summary> Holds all the TileEntities in the chunk.  BlockPos is in world coordinates. </summary>
        public Dictionary<BlockPos, TileEntityBase> tileEntityDict; //TODO replace with a faster collection type
        public BlockPos pos;
        public ChunkPos chunkPos;
        public Bounds chunkBounds;
        /// <summary> If true, the chunk has been changed and needs it's mesh to be rebaked. </summary>
        public bool isDirty;
        /// <summary> If true, the population world gen phase has been done. </summary>
        public bool isPopulated;
        /// <summary> If true, the chunk should not be rendered and is ment to read from only </summary>
        public bool isReadOnly;

        public void Awake() {
            this.filter = this.GetComponent<MeshFilter>();
            MeshCollider[] colliders = this.GetComponents<MeshCollider>();
            this.blockCollider = colliders[0];
            this.triggerCollider = colliders[1];

            this.blocks = new Block[Chunk.BLOCK_COUNT];
            this.metaData = new byte[Chunk.BLOCK_COUNT];
            this.lightLevel = new byte[Chunk.BLOCK_COUNT];
            this.tileEntityDict = new Dictionary<BlockPos, TileEntityBase>();
        }

        /// <summary>
        /// Acts like a constructor of a chunk.
        /// </summary>
        public void initChunk(World w, NewChunkInstructions instructions) {
            this.world = w;
            this.pos = instructions.chunkPos.toBlockPos();
            this.chunkPos = instructions.chunkPos;
            this.setReadOnly(instructions.isReadOnly);
            float radius = (float)Chunk.SIZE / 2;
            this.chunkBounds = new Bounds(new Vector3(this.pos.x + radius, this.pos.y + radius, this.pos.z + radius), new Vector3(Chunk.SIZE, Chunk.SIZE, Chunk.SIZE));
        }

        private void Update() {
            if (!this.isReadOnly && this.isDirty) {
                this.renderChunk();
            }

            //DebugDrawer.bounds(this.chunkBounds, this.isReadOnly ? Color.red : Color.green);
        }

        public void FixedUpdate() {
            if(!this.isReadOnly) {
                // Randomly tick blocks.
                int x, y, z;
                for (int i = 0; i < 3; i++) {
                    x = UnityEngine.Random.Range(0, Chunk.SIZE);
                    y = UnityEngine.Random.Range(0, Chunk.SIZE);
                    z = UnityEngine.Random.Range(0, Chunk.SIZE);
                    this.getBlock(x, y, z).onRandomTick(this.world, x + this.pos.x, y + this.pos.y, z + this.pos.z, this.getMeta(x, y, z), i);
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
            this.isDirty = false;
            this.isPopulated = false;
            this.isReadOnly = false;
            Array.Clear(this.blocks, 0, this.blocks.Length);
            Array.Clear(this.metaData, 0, this.metaData.Length);
            Array.Clear(this.lightLevel, 0, this.lightLevel.Length);
        }

        public void setReadOnly(bool flag) {
            this.isReadOnly = flag;
            this.gameObject.name = "Chunk" + this.chunkPos + (flag ? "(READ ONLY)" : string.Empty);
        }

        public Block getBlock(int x, int y, int z) {
            if((x < 0 || y < 0 || z < 0 || x >= Chunk.SIZE || y >= Chunk.SIZE || z >= Chunk.SIZE)) {
                Debug.Log(x + ", " + y + ", " + z);
            }
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

        /// <summary>
        /// Returns the light level at (x, y, z).
        /// </summary>
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
            int inChunkX = worldX - this.pos.x;
            int inChunkY = worldY - this.pos.y;
            int inChunkZ = worldZ - this.pos.z;
            return (
                inChunkX >= 0 && inChunkX < Chunk.SIZE &&
                inChunkY >= 0 && inChunkY < Chunk.SIZE &&
                inChunkZ >= 0 && inChunkZ < Chunk.SIZE);
        }

        /// <summary>
        /// Checks if the passed world x and z are in the chunk.
        /// </summary>
        public bool isInChunkIgnoreY(int worldX, int worldZ) {
            int inChunkX = worldX - this.pos.x;
            int inChunkZ = worldZ - this.pos.z;
            return (
                inChunkX >= 0 && inChunkX < Chunk.SIZE &&
                inChunkZ >= 0 && inChunkZ < Chunk.SIZE);
        }

        /// <summary>
        /// Bakes the block meshes and light levels into the chunk.
        /// </summary>
        public void renderChunk() {
            CachedRegion cachedRegion = new CachedRegion(this.world, this);
            if(cachedRegion.allChunksLoaded()) {
                // Waiting for the lazy chunk loading to finish...
                return;
            }

            this.isDirty = false;

            MeshBuilder meshData = RenderManager.instance.getMeshBuilder();
            meshData.useRenderDataForCol = true;

            Block currentBlock, neighborBlock;
            bool cachedIsSolid;
            bool[] renderFace = new bool[6];
            Block[] surroundingBlocks = new Block[6];
            BlockPos dirPos;
            int x, y, z, i, facesCulled, meta, x1, y1, z1;

            // Bake blocks into mesh.
            for (x = 0; x < Chunk.SIZE; x++) {
                for (y = 0; y < Chunk.SIZE; y++) {
                    for (z = 0; z < Chunk.SIZE; z++) {
                        currentBlock = this.getBlock(x, y, z);
                        if(currentBlock.renderer != null && currentBlock.renderer.bakeIntoChunks) {

                            //Profiler.BeginSample("Looking up data");
                            // Find the surrounding blocks and faces to cull.
                            facesCulled = 0;

                            for (i = 0; i < 6; i++) {
                                //Profiler.BeginSample("Direction Stuff");
                                dirPos = Direction.all[i].direction;
                                x1 = x + dirPos.x;
                                y1 = y + dirPos.y;
                                z1 = z + dirPos.z;
                                //Profiler.EndSample();
                                
                                //Profiler.BeginSample("Lookup Neighbor");
                                if (x1 < 0 || y1 < 0 || z1 < 0 || x1 >= Chunk.SIZE || y1 >= Chunk.SIZE || z1 >= Chunk.SIZE) {
                                    neighborBlock = cachedRegion.getBlock(x1, y1, z1);
                                } else {
                                    neighborBlock = this.getBlock(x1, y1, z1);
                                }
                                //Profiler.EndSample();

                                //Profiler.BeginSample("Lookup other data");
                                cachedIsSolid = neighborBlock.isSolid;
                                renderFace[i] = !cachedIsSolid;
                                if(currentBlock.renderer.lookupAdjacentBlocks) {
                                    surroundingBlocks[i] = neighborBlock;
                                }
                                if (cachedIsSolid) {
                                    facesCulled++;
                                }                                
                                //Profiler.EndSample();
                            }
                            //Profiler.EndSample();

                            if(facesCulled != 6) {
                                meta = this.getMeta(x, y, z);

                                // Populate the meshData with light levels.
                                meshData.lightLevels[0] = this.getLight(x, y, z);

                                
                                if(currentBlock.renderer.lookupAdjacentLight == true) {
                                    for (i = 0; i < 6; i++) {
                                        dirPos = Direction.all[i].direction;
                                        x1 = x + dirPos.x;
                                        y1 = y + dirPos.y;
                                        z1 = z + dirPos.z;

                                        if (x1 < 0 || y1 < 0 || z1 < 0 || x1 >= Chunk.SIZE || y1 >= Chunk.SIZE || z1 >= Chunk.SIZE) {
                                            meshData.lightLevels[i + 1] = cachedRegion.getLight(x1, y1, z1);
                                        } else {
                                            meshData.lightLevels[i + 1] = this.getLight(x1, y1, z1);
                                        }
                                    }
                                }

                                currentBlock.renderer.renderBlock(currentBlock, meta, meshData, x, y, z, renderFace, surroundingBlocks);
                            }
                        }
                    }
                }
            }

            // Set light uvs for tile entities that don't bake into the chunk.
            Material[] materials;
            Color lightColor;
            foreach (TileEntityBase te in this.tileEntityDict.Values) {
                if(te is TileEntityGameObject) {
                    x = te.posX - this.pos.x;
                    y = te.posY - this.pos.y;
                    z = te.posZ - this.pos.z;
                    materials = ((TileEntityGameObject)te).modelMaterials;
                    lightColor = RenderManager.instance.lightHelper.getColorFromBrightness(this.getLight(x, y, z));
                    for (i = 0; i < materials.Length; i++) {
                        materials[i].SetColor(LightHelper.COLOR_ID, lightColor);
                    }
                }
            }

            this.filter.mesh = meshData.toMesh();
            this.blockCollider.sharedMesh = meshData.getColliderMesh();

            meshData.cleanup();
        }

        public NbtCompound writeToNbt(NbtCompound tag, bool deleteEntities) {
            tag.Add(new NbtByte("isPopulated", this.isPopulated ? (byte)1 : (byte)0));
            byte[] blockBytes = new byte[Chunk.BLOCK_COUNT];
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                blockBytes[i] = (byte)this.blocks[i].id;
            }
            tag.Add(new NbtByteArray("blocks", blockBytes));
            tag.Add(new NbtByteArray("meta", this.metaData));
            tag.Add(new NbtByteArray("light", this.lightLevel));

            NbtList list = new NbtList("tileEntities", NbtTagType.Compound);
            foreach(TileEntityBase te in this.tileEntityDict.Values) {
                list.Add(te.writeToNbt(new NbtCompound()));
            }
            tag.Add(list);

            Entity entity;
            List<Entity> entitiesInChunk = new List<Entity>();
            for (int i = this.world.entityList.Count - 1; i >= 0; i--) {
                entity = this.world.entityList[i];
                if(!(entity is EntityPlayer)) {
                    int x = Mathf.FloorToInt((int)entity.transform.position.x / (float)Chunk.SIZE);
                    int y = Mathf.FloorToInt((int)entity.transform.position.y / (float)Chunk.SIZE);
                    int z = Mathf.FloorToInt((int)entity.transform.position.z / (float)Chunk.SIZE);

                    if (x == this.chunkPos.x && y == this.chunkPos.y && z == this.chunkPos.z) {
                        world.entityList.Remove(entity);
                        entitiesInChunk.Add(entity);
                    }
                }
            }
            NbtList list1 = new NbtList("entities", NbtTagType.Compound);
            for (int i = 0; i < entitiesInChunk.Count; i++) {
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
            this.isPopulated = tag.Get<NbtByte>("isPopulated").ByteValue == 1 ? true : false;
            byte[] blockBytes = tag.Get<NbtByteArray>("blocks").ByteArrayValue;
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                this.blocks[i] = Block.getBlockFromId(blockBytes[i]);
            }
            this.metaData = tag.Get<NbtByteArray>("meta").ByteArrayValue;
            this.lightLevel = tag.Get<NbtByteArray>("light").ByteArrayValue;

            //Populate the tile entity dictionary
            foreach(NbtCompound compound in tag.Get<NbtList>("tileEntities")) {
                BlockPos pos = new BlockPos(compound.Get<NbtInt>("x").IntValue, compound.Get<NbtInt>("y").IntValue, compound.Get<NbtInt>("z").IntValue);
                TileEntityBase te = TileEntityBase.getTileEntityFromId(this.world, pos, compound);
                te.readFromNbt(compound);
                this.world.addTileEntity(pos, te);
            }
            
            //spawn the entities that were saved in the chunk back into the world
            foreach(NbtCompound compound in tag.Get<NbtList>("entities")) {
                int id = compound.Get<NbtInt>("id").IntValue;
                GameObject prefab = EntityRegistry.getEntityPrefabFromId(id);
                if(prefab != null) {
                    this.world.spawnEntity(prefab, compound);
                } else {
                    print("Error!  Entity with an unknown ID of " + id + " was found!  Ignoring!");
                }
            }
        }
    }
}