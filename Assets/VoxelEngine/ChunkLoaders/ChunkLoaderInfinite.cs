using System;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.ChunkLoaders {

    public class ChunkLoaderInfinite : ChunkLoaderBase {

        public ChunkLoaderInfinite(World world, EntityPlayer player) : base(world, player, 1) { }

        protected override bool isOutOfBounds(ChunkPos occupiedChunkPos, Chunk chunk) {
            if (this.toFar(occupiedChunkPos.x, chunk.chunkPos.x) ||
                this.toFar(occupiedChunkPos.y, chunk.chunkPos.y) ||
                this.toFar(occupiedChunkPos.z, chunk.chunkPos.z)) {
                return true;
            }
            return false;
        }

        protected override void loadChunks(ChunkPos occupiedChunkPos) {
            int x, y, z;
            bool flagX, flagY, flagZ, isReadOnly;
            for (x = -this.loadRadius; x <= this.loadRadius; x++) {
                for(y = -this.loadRadius; y <= this.loadRadius; y++) {
                    for (z = -this.loadRadius; z <= this.loadRadius; z++) {
                        flagX = Math.Abs(x) == loadRadius;
                        flagY = Math.Abs(y) == loadRadius;
                        flagZ = Math.Abs(z) == loadRadius;

                        isReadOnly = flagX || flagY || flagZ;
                        NewChunkInstructions instructions = new NewChunkInstructions(x + occupiedChunkPos.x, y + occupiedChunkPos.y, z + occupiedChunkPos.z, isReadOnly);
                        Chunk chunk = world.getChunk(instructions.chunkPos);

                        if (chunk == null) {
                            if (!this.buildQueue.Contains(instructions)) {
                                this.buildQueue.Enqueue(instructions);
                            }
                        } else {
                            chunk.isReadOnly = isReadOnly;
                        }
                    }
                }
            }
        }
    }
}
