using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.ChunkLoaders {

    public class ChunkLoaderLockedY : ChunkLoaderBase {

        private int worldHeight = 8;

        public ChunkLoaderLockedY(World world, EntityPlayer player) : base(world, player, 3) {}

        protected override bool isOutOfBounds(ChunkPos occupiedChunkPos, Chunk chunk) {
            if (this.toFar(occupiedChunkPos.x, chunk.chunkPos.x) || this.toFar(occupiedChunkPos.z, chunk.chunkPos.z)) {
                return true;
            }
            return false;
        }

        protected override void loadYAxis(ChunkPos occupiedChunkPos, int x, int z) {
            for (int y = 0; y < this.worldHeight; y++) {
                ChunkPos pos = new ChunkPos(x + occupiedChunkPos.x, y, z + occupiedChunkPos.z);
                Chunk chunk = world.getChunk(pos);
                if (chunk == null && !this.buildQueue.Contains(pos)) {
                    this.buildQueue.Enqueue(pos);
                }
            }
        }
    }
}
