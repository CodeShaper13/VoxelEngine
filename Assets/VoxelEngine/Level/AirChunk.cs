using VoxelEngine.Blocks;

namespace VoxelEngine.Level {

    public class AirChunk : IChunk {

        public Block getBlock(int x, int y, int z) {
            return Block.air;
        }

        public int getLight(int x, int y, int z) {
            return 0;
        }
    }
}
