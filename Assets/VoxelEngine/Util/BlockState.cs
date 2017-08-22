using VoxelEngine.Blocks;

namespace VoxelEngine.Util {

    /// <summary>
    /// A wrapper for a block, its meta and position.
    /// </summary>
    public struct BlockState {

        public static BlockState NULL_STATE = new BlockState(null);

        public Block block;
        public int meta;

        public BlockState(Block block) {
            this.block = block;
            this.meta = 0;
        }

        public BlockState(Block block, int meta) {
            this.block = block;
            this.meta = meta;
        }
    }
}