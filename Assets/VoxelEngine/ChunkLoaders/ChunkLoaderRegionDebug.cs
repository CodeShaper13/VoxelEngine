using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.ChunkLoaders {

    public class ChunkLoaderRegionDebug : ChunkLoaderBase {

        public ChunkLoaderRegionDebug(World world, EntityPlayer player) : base(world, player, 0) {
            this.world = world;
            this.player = player;

            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 8; j++) {
                    //this.world.loadChunk(new ChunkPos(i, 0, j));
                }
            }
        }

        public override void updateChunkLoader() {
        }
    }
}
