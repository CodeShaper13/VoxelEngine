using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockMushroom : Block {
        private int textureY;

        public BlockMushroom(byte id, int textureY) : base(id) {
            this.textureY = textureY;
            this.setTransparent();
            this.setMineTime(0.1f);
            this.setRenderer(RenderManager.MUSHROOM);
            this.setStatesUsed(4);
        }

        public override void onNeighborChange(World world, BlockPos pos, byte meta, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override void onRandomTick(World world, int x, int y, int z, byte meta, int tickSeed) {
            base.onRandomTick(world, x, y, z, meta, tickSeed);
            //TODO
        }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            return new TexturePos(5 + meta, textureY);
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, byte meta, Direction intendedDir) {
            return world.getBlock(pos.move(Direction.DOWN)).isSolid;
        }
    }
}
