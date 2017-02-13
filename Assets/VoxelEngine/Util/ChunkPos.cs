using System;
using VoxelEngine.Level;

namespace VoxelEngine.Util {

    public struct ChunkPos {
        public int x;
        public int y;
        public int z;

        public ChunkPos(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public BlockPos toBlockPos() {
            return new BlockPos(this.x * Chunk.SIZE, this.y * Chunk.SIZE, this.z * Chunk.SIZE);
        }

        public override string ToString() {
            return "(" + this.x + ", " + this.y + ", " + this.z + ")";
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 47;
                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();
                return hash;
            }
        }
    }
}
