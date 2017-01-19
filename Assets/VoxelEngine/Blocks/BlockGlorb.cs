using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockGlorb : Block {

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            if (brokenWith != null && brokenWith.toolType == ItemTool.ToolType.PICKAXE) {
                return new ItemStack[] { new ItemStack(Item.glorbDust) };
            }
            return new ItemStack[0];
        }
    }
}
