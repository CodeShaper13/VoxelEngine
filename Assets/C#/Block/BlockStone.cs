public class BlockStone : Block {

    public BlockStone() : base() {

    }

    public override TexturePos getTexturePos(Direction direction, byte meta) {
        return new TexturePos(meta, 4);
    }

    public override string getName(byte meta) {
        switch(meta) {
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

    public override ItemStack[] getDrops(byte meta, ItemTool brokenWith) {
        ItemStack stack;
        if(brokenWith != null && brokenWith.toolType == ItemTool.ToolType.PICKAXE) {
            stack = new ItemStack(this.asItem(), meta);
        } else {
            stack = new ItemStack(Item.pebble);
        }
        return new ItemStack[] { stack };
    }
}
