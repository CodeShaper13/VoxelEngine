using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockWood : Block {

        public BlockWood(byte id) : base(id) { }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            TexturePos pos = new TexturePos(1, 1);
            if(meta == 0) {
                if (direction == Direction.UP || direction == Direction.DOWN) {
                    pos.x = 2;
                }
            } else if(meta == 1) {
                if (direction == Direction.EAST || direction == Direction.WEST) {
                    pos.x = 2;
                }
            } else if(meta == 2) {
                if (direction == Direction.NORTH || direction == Direction.SOUTH) {
                    pos.x = 2;
                }
            }
            return pos;
        }
    }
}
