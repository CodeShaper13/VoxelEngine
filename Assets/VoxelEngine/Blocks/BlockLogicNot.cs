using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockLogicNot : BlockLogicBase {

        public BlockLogicNot(int id) : base(id) {
            
        }

        public override bool acceptsWire(Direction directionOfWire, int meta) {
            return directionOfWire.axis == Direction.horizontal[meta].axis;
        }

        public override TexturePos getTopTexture(int rotation) {
            return new TexturePos(9, 4, rotation);
        }
    }
}
