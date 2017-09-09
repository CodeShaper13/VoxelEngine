using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockAir : Block {

        public BlockAir(byte id) : base(id) {
            this.setName("Air");
            this.setStatesUsed(0);
            this.setTransparent();
            this.setReplaceable();
            this.setRenderer(null);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return null;
        }
    }
}
