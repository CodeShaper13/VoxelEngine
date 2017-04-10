using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace Assets.VoxelEngine.Render {

    public class CachedRegion {

        public IChunk[] cachedChunks;

        public CachedRegion(World world, Chunk chunk) {
            int x = chunk.chunkPos.x;
            int y = chunk.chunkPos.y;
            int z = chunk.chunkPos.z;

            this.cachedChunks = new IChunk[7] {
                chunk,
                world.getChunk(new ChunkPos(x + 1, y, z)),
                world.getChunk(new ChunkPos(x - 1, y, z)),
                world.getChunk(new ChunkPos(x, y + 1, z)),
                world.getChunk(new ChunkPos(x, y - 1, z)),
                world.getChunk(new ChunkPos(x, y, z + 1)),
                world.getChunk(new ChunkPos(x, y, z - 1)),
            };

            // Replace null instances with an air chunk, to save expensive is null equality checks.
            for(int i = 0; i < 7; i++) {
                if(this.cachedChunks[i] == null) {
                    this.cachedChunks[i] = RenderManager.instance.airChunk;
                }
            }
        }

        public Block getBlock(int x, int y, int z) {
            return this.getChunk(ref x, ref y, ref z).getBlock(x, y, z);
        }

        public int getLight(int x, int y, int z) {
            return this.getChunk(ref x, ref y, ref z).getLight(x, y, z);
        }

        private IChunk getChunk(ref int x, ref int y, ref int z) {
            if (x < 0) {
                x += Chunk.SIZE;
                return this.cachedChunks[1];
            }
            else if (x >= Chunk.SIZE) {
                x -= Chunk.SIZE;
                return this.cachedChunks[2];
            }
            else if (y < 0) {
                y += Chunk.SIZE;
                return this.cachedChunks[3];
            }
            else if (y >= Chunk.SIZE) {
                y -= Chunk.SIZE;
                return this.cachedChunks[4];
            }
            else if (z < 0) {
                z += Chunk.SIZE;
                return this.cachedChunks[5];
            }
            else if (z >= Chunk.SIZE) {
                z -= Chunk.SIZE;
                return this.cachedChunks[6];
            }
            else {
                return this.cachedChunks[0];
            }
        }
    }
}