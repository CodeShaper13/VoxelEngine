using VoxelEngine.Level;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockLantern : BlockTileEntity {

        public BlockLantern(byte id) : base(id) {
            this.setRenderer(new BlockRendererMesh(References.list.lanturnMesh).setRenderInWorld(false));
        }

        public override void onNeighborChange(World world, BlockPos pos, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta) {
            return new TileEntityLantern(world, x, y, z);
        }
    }
}
