using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockLantern : BlockTileEntity {

        public BlockLantern(int id) : base(id) {
            this.setRenderer(RenderManager.LANTERN);
            this.setEmittedLight(12);
            this.setTransparent();
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, int meta) {
            return new TileEntityLantern(world, x, y, z);
        }
    }
}
