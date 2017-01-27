using fNbt;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Entities;
using VoxelEngine.Render;
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

        public bool isModified;
        public bool isDirty;
        public bool isPopulated;

        private MeshCollider blockCollider;
        private MeshCollider triggerCollider;

        private MeshFilter filter;
        public World world;

        public BlockPos pos;
        public ChunkPos chunkPos;

        public void Awake() {
            this.filter = this.GetComponent<MeshFilter>();
            MeshCollider[] colliders = this.GetComponents<MeshCollider>();
            this.blockCollider = colliders[0];
            this.triggerCollider = colliders[1];
            this.tileEntityDict = new Dictionary<BlockPos, TileEntityBase>();
        }

        //Like a constructor, but since this is a GameObject it can't have one.
        public void initChunk(World w, ChunkPos pos) {
            this.world = w;
            this.pos = pos.toBlockPos();
            this.chunkPos = pos;
            this.gameObject.name = "Chunk" + this.chunkPos;
        }

        public void Update() {
            if (isDirty) {
                isDirty = false;
                Stopwatch s = new Stopwatch();
                s.Start();
                this.renderChunk();
                s.Stop();

                Chunk.TOTAL_BAKED += 1;
                Chunk.MIL += s.ElapsedMilliseconds;
            }
        }

        public void updateChunk() {
            this.tickBlocks();
        }

        private void tickBlocks() {
            int i = Random.Range(int.MinValue, int.MaxValue);
            for (int j = 0; j < 3; j++) {
                int x = (i >> j * 12) & 0x0F;     // 0  12
                int y = (i >> j * 12 + 4) & 0x0F; // 4  16
                int z = (i >> j * 12 + 8) & 0x0F; // 8  20
                this.getBlock(x, y, z).onRandomTick(this.world, x + this.pos.x, y + this.pos.y, z + this.pos.z, this.getMeta(x, y, z), i);
            }
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
            Profiler.BeginSample("renderChunk");
            MeshData meshData = new MeshData();

            Block b;
            byte meta;
            bool[] renderFace = new bool[6];
            Direction d;

            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int y = 0; y < Chunk.SIZE; y++) {
                    for (int z = 0; z < Chunk.SIZE; z++) {
                        Profiler.BeginSample("renderBlock");

                        b = this.getBlock(x, y, z);
                        if(b.renderer != null && b.renderer.renderInWorld) {
                            meta = this.getMeta(x, y, z);
                            meshData.useRenderDataForCol = (b != Block.lava);
                            for (int i = 0; i < 6; i++) {
                                d = Direction.all[i];
                                renderFace[i] = !this.getBlock(x + d.direction.x, y + d.direction.y, z + d.direction.z).isSolid;
                            }
                            b.renderer.renderBlock(b, meta, meshData, x, y, z, renderFace);
                        }

                        Profiler.EndSample();
                    }
                }
            }
            this.filter.mesh = meshData.toMesh();
            Mesh colMesh = new Mesh();
            colMesh.vertices = meshData.colVertices.ToArray();
            colMesh.triangles = meshData.colTriangles.ToArray();
            colMesh.RecalculateNormals();

            this.blockCollider.sharedMesh = colMesh;
            Profiler.EndSample();
        }

        public NbtCompound writeToNbt(NbtCompound tag) {
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