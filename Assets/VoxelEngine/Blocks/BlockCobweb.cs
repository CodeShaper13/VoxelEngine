using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockCobweb : Block {

        // Bits 0 and 1 are direction, 2 is up/down.
        public BlockCobweb(int id) : base(id) {
            this.setTexture(8, 3);
            this.setRenderer(RenderManager.COBWEB);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return null;
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            //TODO
            base.onNeighborChange(world, pos, meta, neighborDir);
        }

        public static int getMetaForState(Direction dir, bool top) {
            return (dir.index - 1) | (top ? 1 : 0) << 2;
        }
    }
}
