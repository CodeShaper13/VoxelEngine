using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockStone : Block {

        public BlockStone(int id) : base(id) {
            this.setType(EnumBlockType.STONE);
            this.setStatesUsed(1);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return new TexturePos(meta, 4);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            if (brokenWith != null && brokenWith.toolType == EnumToolType.PICKAXE) {
                return new ItemStack[] { new ItemStack(this.asItem(), meta) };
            }
            return null;
        }
    }
}
