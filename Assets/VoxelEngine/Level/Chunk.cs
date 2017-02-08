using fNbt;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Entities;
using VoxelEngine.Render;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.Src.Util;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Level {

    public class Chunk : MonoBehaviour {
        public static float TOTAL_BAKED = 0;
        public static long MIL = 0;

        public const int SIZE = 16;
        public const int BLOCK_COUNT = Chunk.SIZE * Chunk.SIZE * Chunk.SIZE;

        public Block[] blocks = new Block[Chunk.BLOCK_COUNT];
        public byte[] metaData = new byte[Chunk.BLOCK_COUNT];
        public Dictionary<BlockPos, TileEntityBase> tileEntityDict; //TODO replace with a faster collection type
        public List<ScheduledTick> scheduledTicks;

        public bool isModified;
        public bool isDirty;
        public bool isPopulated;

        private MeshCollider blockCollider;
        private MeshCollider triggerCollider;

        private MeshFilter filter;
        public World world;

        public BlockPos pos;
        public ChunkPos chunkPos;
        public Bounds chunkBounds;

        public void Awake() {
            this.filter = this.GetComponent<MeshFilter>();
            MeshCollider[] colliders = this.GetComponents<MeshCollider>();
            this.blockCollider = colliders[0];
            this.triggerCollider = colliders[1];
            this.tileEntityDict = new Dictionary<BlockPos, TileEntityBase>();
            this.scheduledTicks = new List<ScheduledTick>();
        }

        //Like a constructor, but since this is a GameObject it can't have one.
        public void initChunk(World w, ChunkPos pos) {
            this.world = w;
            this.pos = pos.toBlockPos();
            this.chunkPos = pos;
            this.gameObject.name = "Chunk" + this.chunkPos;
            this.chunkBounds = new Bounds(new Vector3(this.pos.x + 8, this.pos.y + 8, this.pos.z + 8), new Vector3(16, 16, 16));
        }

        public void Update() {
            //DebugDrawer.bounds(this.chunkBounds, Color.white);

            if (isDirty) {
                isDirty = false;
                //Stopwatch s = new Stopwatch();
                //s.Start();
                this.renderChunk();
                //s.Stop();

                //Chunk.TOTAL_BAKED += 1;
                //Chunk.MIL += s.ElapsedMilliseconds;
            }
        }

        public void FixedUpdate() {
            int x, y, z;
            int i = Random.Range(int.MinValue, int.MaxValue);
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

        public Block getBlock(int x, int y, int z) {
            //TODO refactor code to remove this if block
            if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                return this.blocks[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
            }
            return world.getBlock(pos.x + x, pos.y + y, pos.z + z);
        }

        //This should only be used in world generation, or a case where the neighbor blocks should not be updated
        public void setBlock(int x, int y, int z, Block block) {
            this.isModified = true;
            this.blocks[x + Chunk.SIZE * (z + Chunk.SIZE * y)] = block;
        }

        public byte getMeta(int x, int y, int z) {
            return this.metaData[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
        }

        //This should only be used in world generation, or a case where the neighbor blocks should not be updated
        public void setMeta(int x, int y, int z, byte meta) {
            this.isModified = true;
            this.metaData[x + Chunk.SIZE * (z + Chunk.SIZE * y)] = meta;
        }

        //Renders all the blocks within the chunk
        private void renderChunk() {
            //Profiler.BeginSample("renderChunk");
            MeshData meshData = new MeshData();

            Block b;
            byte meta;
            bool[] renderFace = new bool[6];
            Direction d;
            int x, y, z, i;

            for (x = 0; x < Chunk.SIZE; x++) {
                for (y = 0; y < Chunk.SIZE; y++) {
                    for (z = 0; z < Chunk.SIZE; z++) {
                        //Profiler.BeginSample("renderBlock");

                        b = this.getBlock(x, y, z);
                        if(b.renderer != null && b.renderer.renderInWorld) {
                            meta = this.getMeta(x, y, z);
                            meshData.useRenderDataForCol = (b != Block.lava);
                            for (i = 0; i < 6; i++) {
                                d = Direction.all[i];
                                renderFace[i] = !this.getBlock(x + d.direction.x, y + d.direction.y, z + d.direction.z).isSolid;
                            }
                            //Profiler.BeginSample("calculatingMesh");
                            b.renderer.renderBlock(b, meta, meshData, x, y, z, renderFace);
                            //Profiler.EndSample();
                        }

                        //Profiler.EndSample();
                    }
                }
            }
            this.filter.mesh = meshData.toMesh();
            Mesh colMesh = new Mesh();
            colMesh.vertices = meshData.colVertices.ToArray();
            colMesh.triangles = meshData.colTriangles.ToArray();
            colMesh.RecalculateNormals();

            this.blockCollider.sharedMesh = colMesh;
            //Profiler.EndSample();
        }

        public NbtCompound writeToNbt(NbtCompound tag, bool deleteEntities) {
            tag.Add(new NbtByte("isPopulated", this.isPopulated ? (byte)1 : (byte)0));
            byte[] blockBytes = new byte[Chunk.BLOCK_COUNT];
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                blockBytes[i] = this.blocks[i].id;
            }
            tag.Add(new NbtByteArray("blocks", blockBytes));
            tag.Add(new NbtByteArray("meta", this.metaData));

            NbtList list = new NbtList("tileEntities", NbtTagType.Compound);
            foreach(TileEntityBase te in this.tileEntityDict.Values) {
                list.Add(te.writeToNbt(new NbtCompound()));
            }
            tag.Add(list);

            Entity entity;
            List<Entity> entitiesInChunk = new List<Entity>();
            for (int i = this.world.entityList.Count - 1; i >= 0; i--) {
                entity = this.world.entityList[i];
                int x = Mathf.FloorToInt((int)entity.transform.position.x / (float)Chunk.SIZE);
                int y = Mathf.FloorToInt((int)entity.transform.position.y / (float)Chunk.SIZE);
                int z = Mathf.FloorToInt((int)entity.transform.position.z / (float)Chunk.SIZE);

                if (x == this.chunkPos.x && y == this.chunkPos.y && z == this.chunkPos.z) {
                    world.entityList.Remove(entity);
                    entitiesInChunk.Add(entity);
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