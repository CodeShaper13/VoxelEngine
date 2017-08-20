using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockLogicNot : BlockLogicBase {

        public BlockLogicNot(int id) : base(id) {
            
        }

        public override TexturePos getTopTexture() {
            return new TexturePos(9, 4);
        }
    }
}
