using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockOre : Block {
        private Item droppedItem;
        private int textureY;

        public BlockOre(int id, Item drop, int textureY) : base(id) {
            this.droppedItem = drop;
            this.textureY = textureY;
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            ItemStack stack;
            if (brokenWith != null && brokenWith.toolType == ItemTool.ToolType.PICKAXE) {
                stack = new ItemStack(this.droppedItem);
            }
            else {
                stack = new ItemStack(Item.pebble);
            }
            return new ItemStack[] { stack };
        }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            return new TexturePos(meta, this.textureY);
        }
    }
}
