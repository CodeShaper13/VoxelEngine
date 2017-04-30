using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockGlorb : BlockTileEntity {

        public BlockGlorb(byte id) : base(id) { }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, int meta) {
            return new TileEntityGlorb(world, x, y, z);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            /*
            if (brokenWith != null && brokenWith.toolType == ItemTool.ToolType.PICKAXE) {
                return new ItemStack[] { new ItemStack(Item.glorbDust) };
            }
            */
            return new ItemStack[0];
        }
    }
}
