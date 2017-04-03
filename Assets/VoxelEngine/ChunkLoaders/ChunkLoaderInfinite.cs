using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.ChunkLoaders {

    public class ChunkLoaderInfinite : ChunkLoaderBase {

        public ChunkLoaderInfinite(World world, EntityPlayer player) : base(world, player, 1) {}

        protected override bool isOutOfBounds(ChunkPos occupiedChunkPos, Chunk chunk) {
            if (this.toFar(occupiedChunkPos.x, chunk.chunkPos.x) ||
                this.toFar(occupiedChunkPos.y, chunk.chunkPos.y) ||
                this.toFar(occupiedChunkPos.z, chunk.chunkPos.z)) {
                return true;
            }
            return false;
        }

        protected override void loadYAxis(ChunkPos occupiedChunkPos, int x, int z) {
            for (int y = -this.loadRadius; y < this.loadRadius + 1; y++) {
                ChunkPos pos = new ChunkPos(x + occupiedChunkPos.x, y + occupiedChunkPos.y, z + occupiedChunkPos.z);
                Chunk chunk = world.getChunk(pos);
                if (chunk == null && !this.buildQueue.Contains(pos)) {
                    this.buildQueue.Enqueue(pos);
                }
            }
        }
    }
}
