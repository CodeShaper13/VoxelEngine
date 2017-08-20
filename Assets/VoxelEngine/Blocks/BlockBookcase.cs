using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockBookcase : Block {

        public BlockBookcase(int id) : base(id) {
            this.setType(EnumBlockType.WOOD);
            this.setMineTime(1);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            if(direction.axis == EnumAxis.Y) {
                return Block.plank.texturePos;
            } else {
                return new TexturePos(5, 3);
            }
        }
    }
}
