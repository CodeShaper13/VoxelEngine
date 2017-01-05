
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockGrass : Block {

        public override void onRandomTick(World world, BlockPos pos, byte meta, int tickSeed) {
            //int i = (tickSeed) & 0x03;
            //BlockPos pos1 = pos + Direction.xzPlane[i].direction;
            //if (world.getBlock(pos1) == Block.dirt) {
            //    world.setBlock(pos1, Block.grass);
            //}
        }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            TexturePos tile = new TexturePos(0, 0);
            if (direction == Direction.UP) {
                tile.x = 2;
            }
            else if (direction == Direction.DOWN) {
                tile.x = 1;
            }
            else {
                tile.x = 3;
            }
            return tile;
        }
    }
}