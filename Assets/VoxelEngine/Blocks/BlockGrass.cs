using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockGrass : Block {

        public BlockGrass(byte id) : base(id) {
            this.setType(EnumBlockType.DIRT);
            this.setRenderer(RenderManager.GRASS);
        }

        public override void onRandomTick(World world, int x, int y, int z, int meta, int tickSeed) {
            //int i = (tickSeed) & 0x03;
            //BlockPos pos1 = pos + Direction.xzPlane[i].direction;
            //if (world.getBlock(pos1) == Block.dirt) {
            //    world.setBlock(pos1, Block.grass);
            //}
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            if (direction == Direction.UP) {
                return new TexturePos(2, 0);
            } else if (direction == Direction.DOWN) {
                return new TexturePos(1, 0);
            } else {
                return new TexturePos(3, 0);
            }
        }
    }
}