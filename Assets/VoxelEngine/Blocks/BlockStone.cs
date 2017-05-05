using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockStone : Block {

        public BlockStone(int id) : base(id) {
            this.setType(EnumBlockType.STONE);
            this.setStatesUsed(4);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return new TexturePos(meta, 4);
        }

        public override string getName(int meta) {
            switch (meta) {
                case 0:
                    return "Stone 0";
                case 1:
                    return "Stone 1";
                case 2:
                    return "Stone 2";
                case 3:
                    return "Stone 3";
                case 4:
                    return "Stone 4";
            }
            return base.getName(meta);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            ItemStack stack;
            if (brokenWith != null && brokenWith.toolType == EnumToolType.PICKAXE) {
                stack = new ItemStack(this.asItem(), meta);
            }
            else {
                stack = new ItemStack(Item.pebble);
            }
            return new ItemStack[] { stack };
        }
    }
}
