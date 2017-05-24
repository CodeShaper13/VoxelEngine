using VoxelEngine.Blocks;

namespace VoxelEngine.Util {

    /// <summary>
    /// A wrapper for a block, its meta and position.
    /// </summary>
    public class BlockState {

        public Block block;
        public int meta;
        public BlockPos pos;

        public BlockState(Block block, int meta) {
            this.block = block;
            this.meta = meta;
            this.pos = BlockPos.zero;
        }

        public BlockState(Block block, int meta, BlockPos pos) {
            this.block = block;
            this.meta = meta;
            this.pos = pos;
        }
    }
}