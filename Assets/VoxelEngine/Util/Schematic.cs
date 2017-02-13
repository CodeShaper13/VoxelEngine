using fNbt;
using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;

namespace VoxelEngine.Util {

    public class Schematic : IBlockHolder {

        public string schematicName;
        public Block[] blocks;
        public byte[] metaData;
        public readonly int sizeX;
        public readonly int sizeY;
        public readonly int sizeZ;
        private readonly int totalSize;

        private Schematic(int sizeX, int sizeY, int sizeZ) {
            this.schematicName = string.Empty;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
            this.totalSize = this.sizeX * this.sizeY * this.sizeZ;
            this.blocks = new Block[this.totalSize];
            this.metaData = new byte[this.totalSize];
        }

        public static Schematic newSchematic(Chunk chunk) {
            Schematic s = new Schematic(Chunk.SIZE, Chunk.SIZE, Chunk.SIZE);

            for (int i = 0; i < s.totalSize; i++) {
                Array.Copy(s.blocks, chunk.blocks, s.totalSize);
                Array.Copy(s.metaData, chunk.metaData, s.totalSize);
            }

            return s;
        }

        public static Schematic newSchematic(int sizeX, int sizeY, int sizeZ) {
            return new Schematic(sizeX, sizeY, sizeZ);
        }

        public static Schematic newSchematic(World world, BlockPos pos1, BlockPos pos2) {
            int startX = Mathf.Min(pos1.x, pos2.x);
            int startY = Mathf.Min(pos1.y, pos2.y);
            int startZ = Mathf.Min(pos1.z, pos2.z);
            int endX = Mathf.Max(pos1.x, pos2.x);
            int endY = Mathf.Max(pos1.y, pos2.y);
            int endZ = Mathf.Max(pos1.z, pos2.z);
            int x, y, z;

            Schematic s = new Schematic(endX - startX, endY - startY, endZ - startZ);

            for (int i = startX; i <= endX; i++) {
                for (int j = startY; j <= endY; j++) {
                    for (int k = startZ; k <= endZ; k++) {
                        s.setBlock(i - startX, j - startY, k - startZ, world.getBlock(i, j, k));
                        s.setMeta(i - startX, j - startY, k - startZ, world.getMeta(i, j, k));
                    }
                }
            }
            return s;
        }

        public static Schematic newSchematic(NbtCompound tag) {
            Schematic s = new Schematic(
                tag.Get<NbtInt>("sizeX").IntValue,
                tag.Get<NbtInt>("sizeY").IntValue,
                tag.Get<NbtInt>("sizeZ").IntValue);
            s.readFromNbt(tag);
            return s;
        }

        public Block getBlock(int x, int y, int z) {
            return this.blocks[(y * this.sizeX * this.sizeZ) + (z * this.sizeX) + x];
        }

        public void setBlock(int x, int y, int z, Block block) {
            this.blocks[(y * this.sizeX * this.sizeZ) + (z * this.sizeX) + x] = block;
        }

        public byte getMeta(int x, int y, int z) {
            return this.metaData[(y * this.sizeX * this.sizeZ) + (z * this.sizeX) + x];
        }

        public void setMeta(int x, int y, int z, byte meta) {
            this.metaData[(y * this.sizeX * this.sizeZ) + (z * this.sizeX) + x] = meta;
        }

        public NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtString("name", this.schematicName));
            tag.Add(new NbtInt("sizeX", this.sizeX));
            tag.Add(new NbtInt("sizeY", this.sizeY));
            tag.Add(new NbtInt("sizeZ", this.sizeZ));
            byte[] blockBytes = new byte[Chunk.BLOCK_COUNT];
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                blockBytes[i] = this.blocks[i].id;
            }
            tag.Add(new NbtByteArray("blocks", blockBytes));
            tag.Add(new NbtByteArray("meta", this.metaData));
            return tag;
        }

        public void readFromNbt(NbtCompound tag) {
            // sizeX, sizeY and sizeZ are set in ctor
            this.schematicName = tag.Get<NbtString>("name").StringValue;
            byte[] blockBytes = tag.Get<NbtByteArray>("blocks").ByteArrayValue;
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                this.blocks[i] = Block.getBlock(blockBytes[i]);
            }
            this.metaData = tag.Get<NbtByteArray>("meta").ByteArrayValue;
        }
    }
}
