using UnityEngine;

public class Item {
    private static int NEXT_ID = 256;
    public static Item[] ITEM_LIST = new Item[512];
    private static IRenderItem RENDER_BILLBOARD = new RenderItemBillboard();

    public static Item basicItem = new Item();

    public int id = 256;
    public string name = "null";
    public TexturePos texturePos;

    public IRenderItem itemRenderer;

    public Item() {
        this.id = Item.NEXT_ID++;
        Item.ITEM_LIST[this.id] = this;

        this.itemRenderer = Item.RENDER_BILLBOARD;
    }

    //Hit.transform will be null if air was clicked
    public virtual ItemStack onRightClick(World world, ItemStack stack, RaycastHit hit) {
        return stack;
    }

    public Item setName(string name) {
        this.name = name;
        return this;
    }

    public Item setRenderData(IRenderItem data) {
        this.itemRenderer = data;
        return this;
    }

    public Item setTexture(int x, int y) {
        this.texturePos = new TexturePos(x, y);
        return this;
    }

    //Static helper methods
    public static void initBlockItems() {
        foreach(Block b in Block.BLOCK_LIST) {
            if(b != null) {
                Item.ITEM_LIST[b.id] = new ItemBlock(b);
            }
        }
    }
}
