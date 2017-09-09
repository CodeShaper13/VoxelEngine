using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockTest : Block {

        public BlockTest(int id) : base(id) {
            this.setRenderer(RenderManager.TEST);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            if(direction == Direction.NORTH) {
                return new TexturePos(10, 1);
            } else {
                return new TexturePos(10, 0);
            }
        }
    }
}
