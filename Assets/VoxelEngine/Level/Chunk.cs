#define MAX_LIGHT

using Assets.VoxelEngine.Render;
using fNbt;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Entities;
using VoxelEngine.Render;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Level {

    public class Chunk : MonoBehaviour, IChunk {

        public const int SIZE = 16;
        public const int BLOCK_COUNT = Chunk.SIZE * Chunk.SIZE * Chunk.SIZE;

        private MeshFilter filter;
        private MeshCollider blockCollider;
        private MeshCollider triggerCollider;
        public World world;

        public Block[] blocks;
        public byte[] metaData;
        /// <summary> First 4 bits are block light, last 4 are sky light </summary>
        public byte[] lightLevel;
        public Dictionary<BlockPos, TileEntityBase> tileEntityDict; //TODO replace with a faster collection type
        public List<ScheduledTick> scheduledTicks;

        public BlockPos pos;
        public ChunkPos chunkPos;
        public Bounds chunkBounds;
        public bool isModified;
        public bool isDirty;
        public bool isPopulated;

        public void Awake() {
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

        // Sets all the fields, making sure to clear old oens if this chunk is reused
        public void initChunk(World w, ChunkPos pos) {
            this.world = w;
            this.pos = pos.toBlockPos();
            this.chunkPos = pos;
            this.gameObject.name = "Chunk" + this.chunkPos;
            this.chunkBounds = new Bounds(new Vector3(this.pos.x + 8, this.pos.y + 8, this.pos.z + 8), new Vector3(16, 16, 16));
        }

        public void Update() {
            if (isDirty) {
                isDirty = false;
                this.renderChunk();
            }
        }

        public void FixedUpdate() {
            int x, y, z;
            int i = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            for (int j = 0; j < 3; j++) {
                x = (i >> j * 12) & 0x0F;     // 0  12
                y = (i >> j * 12 + 4) & 0x0F; // 4  16
                z = (i >> j * 12 + 8) & 0x0F; // 8  20
                this.getBlock(x, y, z).onRandomTick(this.world, x + this.pos.x, y + this.pos.y, z + this.pos.z, this.getMeta(x, y, z), i);
            }

            //for(int k = this.scheduledTicks.Count - 1; k >= 0; k--) {
            //    ScheduledTick tick = this.scheduledTicks[k];
            //    tick.remainingTicks -= 1;
            //    if(tick.remainingTicks <= 0) {
            //        this.getBlock()
            //        this.scheduledTicks.RemoveAt(k);
            //    }
            //}
        }

        /// <summary>
        /// Resets the chunk, clearing out fields and preparing it to be used again.
        /// </summary>
        public void resetChunk() {
            this.tileEntityDict.Clear();
            this.scheduledTicks.Clear();
            this.isModified = false;
            this.isDirty = false;
            this.isPopulated = false;
            Array.Clear(this.blocks, 0, this.blocks.Length);
            Array.Clear(this.metaData, 0, this.metaData.Length);
            Array.Clear(this.lightLevel, 0, this.lightLevel.Length);
        }

        public Block getBlock(int x, int y, int z) {
            return this.blocks[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x];
        }

        public void setBlock(int x, int y, int z, Block block) {
            this.isModified = true;
            this.blocks[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x] = block;
        }

        public int getMeta(int x, int y, int z) {
            return this.metaData[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x];
        }

        public void setMeta(int x, int y, int z, int meta) {
            this.isModified = true;
            this.metaData[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x] = (byte)meta;
        }

        /// <summary>
        /// Returns the light level at (x, y, z)
        /// </summary>
        public int getLight(int x, int y, int z) {
#if (MAX_LIGHT)
            return 15;
#endif
            return this.lightLevel[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x];
        }

        /// <summary>
        /// Sets the light level at (x, y, z)
        /// </summary>
        public void setLight(int x, int y, int z, int amount) {
            this.isModified = true;
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
            MeshBuilder meshData = RenderManager.instance.getMeshBuilder();

            Block b, neightborBlock;
            byte meta;
            bool flag;
            bool[] renderFace = new bool[6];
            Block[] surroundingBlocks = new Block[6];
            Direction dir;
            int x, y, z, i, facesCulled, x1, y1, z1;
            int blocksSkipped = 0;
            IChunk c;

            CachedRegion cachedRegion = new CachedRegion(this.world, this);

            // Bake blocks into mesh.
            for (x = 0; x < Chunk.SIZE; x++) {
                for (y = 0; y < Chunk.SIZE; y++) {
                    for (z = 0; z < Chunk.SIZE; z++) {
                        b = this.blocks[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
                        if(b.renderer != null && b.renderer.bakeIntoChunks) {
                            // Find the surrounding blocks and faces to cull.
                            facesCulled = 0;
                            for (i = 0; i < 6; i++) {
                                dir = Direction.all[i];

                                x1 = x + dir.direction.x;
                                y1 = y + dir.direction.y;
                                z1 = z + dir.direction.z;

                                c = cachedRegion.getChunk(x1, y1, z1);
                                x1 += (x1 < 0 ? Chunk.SIZE : x1 >= Chunk.SIZE ? -Chunk.SIZE : 0);
                                y1 += (y1 < 0 ? Chunk.SIZE : y1 >= Chunk.SIZE ? -Chunk.SIZE : 0);
                                z1 += (z1 < 0 ? Chunk.SIZE : z1 >= Chunk.SIZE ? -Chunk.SIZE : 0);
                                neightborBlock = c.getBlock(x1, y1, z1);
                                //neightborBlock = cachedRegion.getBlock(x + dir.direction.x, y + dir.direction.y, z + dir.direction.z);


                                flag = neightborBlock.isSolid;
                                renderFace[i] = !flag;
                                surroundingBlocks[i] = neightborBlock;
                                if (flag) {
                                    facesCulled++;
                                }
                            }

                            if(facesCulled != 6) {
                                meta = this.metaData[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
                                meshData.useRenderDataForCol = (b != Block.lava);

                                // Populate the meshData with light levels.
                                if (b.renderer.lookupAdjacentLight == EnumLightLookup.CURRENT) {
                                    meshData.lightLevels[0] = this.getLight(x, y, z); // No need for cachedRegion overhead, this is always in the chunk
                                } else {
                                    for (i = 0; i < 6; i++) {
                                        dir = Direction.all[i];

                                        meshData.lightLevels[i + 1] = cachedRegion.getLight(x + dir.direction.x, y + dir.direction.y, z + dir.direction.z);
                                    }
                                    meshData.lightLevels[0] = this.getLight(x, y, z);
                                }

                                // Render the block.
                                b.renderer.renderBlock(b, meta, meshData, x, y, z, renderFace, surroundingBlocks);
                            } else {
                                blocksSkipped++;
                            }
                        }
                    }
                }
            }

            //print("Blocks skipped: " + j);

            // Set light uvs for tile entities.
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
                this.blocks[i] = Block.getBlock(blockBytes[i]);
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
                byte id = compound.Get<NbtByte>("id").ByteValue;
                GameObject prefab = EntityList.getPrefabFromId(id);
                if(prefab != null) {
                    this.world.spawnEntity(prefab, compound);
                } else {
                    print("Error!  Entity with an unknown ID of " + id + " was found!  Ignoring!");
                }
            }
        }
    }
}