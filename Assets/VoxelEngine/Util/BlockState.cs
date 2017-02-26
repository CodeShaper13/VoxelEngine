using VoxelEngine.Blocks;

namespace VoxelEngine.Util {

    public class BlockState {

        public Block block;
        public byte meta;
        public BlockPos pos;

        public BlockState(Block block, byte meta, BlockPos pos) {
            this.block = block;
            this.meta = meta;
            this.pos = pos;
        }
    }
}