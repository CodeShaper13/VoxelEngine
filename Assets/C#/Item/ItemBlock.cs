public class ItemBlock : Item {
    private static IRenderItem RENDER_BLOCK = new RenderItemBlock();

    public ItemBlock(Block block) {
        this.id = block.id;
        this.setName(block.name);
        this.setRenderData(ItemBlock.RENDER_BLOCK);
    }
}
