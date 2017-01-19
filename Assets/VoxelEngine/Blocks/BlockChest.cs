using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockChest : BlockTileEntity {

        public override void onRightClick(World world, EntityPlayer player, BlockPos pos, byte meta) {
            player.openContainer(References.list.containerChest, ((TileEntityChest)world.getTileEntity(pos)).chestData);
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta) {
            return new TileEntityChest(world, x, y, z);
        }
    }
}
