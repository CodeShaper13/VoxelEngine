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
            this.setType(Type.STONE);
            this.setStatesUsed(5);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            ItemStack stack;
            if (brokenWith != null && brokenWith.toolType == EnumToolType.PICKAXE) {
                stack = new ItemStack(this.droppedItem);
            }
            else {
                stack = new ItemStack(Item.pebble);
            }
            return new ItemStack[] { stack };
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return new TexturePos(meta, this.textureY);
        }
    }
}
