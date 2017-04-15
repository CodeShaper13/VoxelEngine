using VoxelEngine.Blocks;

namespace VoxelEngine.Util {

    public class BlockState {

        public Block block;
        public int meta;
        public BlockPos pos;

        public BlockState(Block block, int meta, BlockPos pos) {
            this.block = block;
            this.meta = meta;
            this.pos = pos;
        }
    }
}