using UnityEngine;

public class ItemBlock : Item {
    private static IRenderItem RENDER_BLOCK = new RenderItemBlock();

    public Block block;

    public ItemBlock(Block block) {
        this.block = block;
        this.id = block.id;
        this.setName(block.name);
        this.setRenderData(ItemBlock.RENDER_BLOCK);
    }

    public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, RaycastHit hit) {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk != null) {
            BlockPos pos = BlockPos.fromRaycast(hit, true);
            if (world.getBlock(pos).replaceable) {
                world.setBlock(pos, this.block);
                world.setMeta(pos, stack.meta);
                stack.count -= 1;
                if(stack.count <= 0) {
                    stack = null;
                }
            }
        }

        return stack;
    }
}
