using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockTest : Block {

        public BlockTest(int id) : base(id) {
            this.setRenderer(RenderManager.TEST);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return new TexturePos(10, direction.index - 1);
        }
    }
}
