using UnityEngine;
using UnityEngine.Profiling;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace Assets.VoxelEngine.Render {

    public class CachedRegion {

        private Chunk up;
        private Chunk down;
        private Chunk north;
        private Chunk east;
        private Chunk west;
        private Chunk south;
        private Chunk cached;

        public CachedRegion(World world, Chunk chunk) {
            int x = chunk.chunkPos.x;
            int y = chunk.chunkPos.y;
            int z = chunk.chunkPos.z;

            this.cached = chunk;

            this.east = world.getChunk(new ChunkPos(x + 1, y, z));
            this.west = world.getChunk(new ChunkPos(x - 1, y, z));
            this.up = world.getChunk(new ChunkPos(x, y + 1, z));
            this.down = world.getChunk(new ChunkPos(x, y - 1, z));
            this.north = world.getChunk(new ChunkPos(x, y, z + 1));
            this.south = world.getChunk(new ChunkPos(x, y, z - 1));
        }

        public bool check() {
            return this.east == null || this.west == null || this.up == null || this.down == null || this.north == null || this.south == null;
        }

        public Block getBlock(int x, int y, int z) {
            this.getChunk(x, y, z);
            Profiler.BeginSample("Ternary");
            x += x < 0 ? Chunk.SIZE : x >= Chunk.SIZE ? -Chunk.SIZE : 0;
            y += y < 0 ? Chunk.SIZE : y >= Chunk.SIZE ? -Chunk.SIZE : 0;
            z += z < 0 ? Chunk.SIZE : z >= Chunk.SIZE ? -Chunk.SIZE : 0;
            Profiler.EndSample();
            return this.cached.getBlock(x, y, z);
            /*
            if (x < 0) {
                x += Chunk.SIZE;
                return this.cachedChunks[2].getBlock(x, y, z);
            }
            else if (x >= Chunk.SIZE) {
                x -= Chunk.SIZE;
                return this.cachedChunks[1].getBlock(x, y, z);
            }
            else if (y < 0) {
                y += Chunk.SIZE;
                return this.cachedChunks[4].getBlock(x, y, z);
            }
            else if (y >= Chunk.SIZE) {
                y -= Chunk.SIZE;
                return this.cachedChunks[3].getBlock(x, y, z);
            }
            else if (z < 0) {
                z += Chunk.SIZE;
                return this.cachedChunks[6].getBlock(x, y, z);
            }
            else if (z >= Chunk.SIZE) {
                z -= Chunk.SIZE;
                return this.cachedChunks[5].getBlock(x, y, z);
            }
            else {
                return this.cachedChunks[0].getBlock(x, y, z);
            }
            */
        }

        public int getLight(int x, int y, int z) {
            this.getChunk(x, y, z);
            x += x < 0 ? Chunk.SIZE : x >= Chunk.SIZE ? -Chunk.SIZE : 0;
            y += y < 0 ? Chunk.SIZE : y >= Chunk.SIZE ? -Chunk.SIZE : 0;
            z += z < 0 ? Chunk.SIZE : z >= Chunk.SIZE ? -Chunk.SIZE : 0;
            return this.cached.getLight(x, y, z);
        }

        private void getChunk(int x, int y, int z) {
            if (x < 0) {
                //x += Chunk.SIZE;
                this.cached = this.west;
            }
            else if (x >= Chunk.SIZE) {
                //x -= Chunk.SIZE;
                this.cached = this.east;
            }
            else if (y < 0) {
                //y += Chunk.SIZE;
                this.cached = this.down;
            }
            else if (y >= Chunk.SIZE) {
                //y -= Chunk.SIZE;
                this.cached = this.up;
            }
            else if (z < 0) {
                //z += Chunk.SIZE;
                this.cached = this.south;
            }
            else { // (z >= Chunk.SIZE)
                //z -= Chunk.SIZE;
                this.cached = this.north;
            }
        }
    }
}