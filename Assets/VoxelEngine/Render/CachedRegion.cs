using System;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace Assets.VoxelEngine.Render {

    /// <summary>
    /// Used in Chunk mesh baking for faster block, meta and light lookup.
    /// </summary>
    [Obsolete("Use CachedChunk3x3 instead!")]
    public class CachedRegion {

        private Chunk up;
        private Chunk down;
        private Chunk north;
        private Chunk east;
        private Chunk west;
        private Chunk south;

        public CachedRegion(World world, Chunk chunk) {
            int x = chunk.chunkPos.x;
            int y = chunk.chunkPos.y;
            int z = chunk.chunkPos.z;

            this.east = world.getChunk(new ChunkPos(x + 1, y, z));
            this.west = world.getChunk(new ChunkPos(x - 1, y, z));
            this.up = world.getChunk(new ChunkPos(x, y + 1, z));
            this.down = world.getChunk(new ChunkPos(x, y - 1, z));
            this.north = world.getChunk(new ChunkPos(x, y, z + 1));
            this.south = world.getChunk(new ChunkPos(x, y, z - 1));
        }

        /// <summary>
        /// Returns true is all the adjacent chunks are loaded.
        /// </summary>
        public bool allChunksLoaded() {
            return this.east != null && this.west != null && this.up != null && this.down != null && this.north != null && this.south != null;
        }

        public Block getBlock(int x, int y, int z) {
            int x1 = x + (x < 0 ? Chunk.SIZE : x >= Chunk.SIZE ? -Chunk.SIZE : 0);
            int y1 = y + (y < 0 ? Chunk.SIZE : y >= Chunk.SIZE ? -Chunk.SIZE : 0);
            int z1 = z + (z < 0 ? Chunk.SIZE : z >= Chunk.SIZE ? -Chunk.SIZE : 0);
            return this.getChunk(x, y, z).getBlock(x1, y1, z1);
        }

        public int getLight(int x, int y, int z) {
            int x1 = x + (x < 0 ? Chunk.SIZE : x >= Chunk.SIZE ? -Chunk.SIZE : 0);
            int y1 = y + (y < 0 ? Chunk.SIZE : y >= Chunk.SIZE ? -Chunk.SIZE : 0);
            int z1 = z + (z < 0 ? Chunk.SIZE : z >= Chunk.SIZE ? -Chunk.SIZE : 0);
            return this.getChunk(x, y, z).getLight(x1, y1, z1);
        }

        private Chunk getChunk(int x, int y, int z) {
            if (x < 0) {
                return this.west;
            } else if (x >= Chunk.SIZE) {
                return this.east;
            } else if (y < 0) {
                return this.down;
            } else if (y >= Chunk.SIZE) {
                return this.up;
            } else if (z < 0) {
                return this.south;
            } else { // (z >= Chunk.SIZE)
                return this.north;
            }
        }
    }
}