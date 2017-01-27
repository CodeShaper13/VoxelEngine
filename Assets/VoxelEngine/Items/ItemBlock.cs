using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Render.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class ItemBlock : Item {
        private static IRenderItem RENDER_BLOCK = new RenderItemBlock();

        public Block block;

        public ItemBlock(Block block) : base(block.id) {
            this.block = block;
            this.id = block.id;
            this.setName(block.name);
            this.setRenderData(ItemBlock.RENDER_BLOCK);
        }

        public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            if (hit.state != null) {
                BlockPos pos = BlockPos.fromRaycast(hit.unityRaycastHit, true);
                if (world.getBlock(pos).replaceable) {
                    world.setBlock(pos, this.block);
                    world.setMeta(pos, stack.meta);
                    stack = stack.safeDeduction();
                }
            }
            return stack;
        }
    }
}
