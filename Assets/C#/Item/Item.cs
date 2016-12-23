using UnityEngine;

public class Item {
    private static int NEXT_ID = 256;
    public static Item[] ITEM_LIST = new Item[512];
    private static IRenderItem RENDER_BILLBOARD = new RenderItemBillboard();

    public static Item pebble = new ItemThrowable().setName("Pebble").setTexture(1, 0);
    public static Item coal = new Item().setName("Coal").setTexture(0, 0);
    public static Item bronzeBar = new Item().setName("Bronze Bar").setTexture(0, 1);
    public static Item ironBar = new Item().setName("Iron Bar").setTexture(0, 2);
    public static Item goldBar = new Item().setName("Gold Bar").setTexture(0, 3);
    public static Item ruby = new Item().setName("Ruby").setTexture(0, 4);
    public static Item uranium = new Item().setName("Uranium").setTexture(0, 5);
    public static Item bronzeHelmet;
    public static Item bronzeChestplate;
    public static Item ironHelmet;
    public static Item ironChestplate;
    public static Item goldHelmet;
    public static Item goldChestplate;
    public static Item bronzePickaxe = new ItemTool(1.25f, ItemTool.ToolType.PICKAXE, Block.Type.STONE);
    public static Item bronzeShovel = new ItemTool(1.25f, ItemTool.ToolType.SHOVEL, Block.Type.DIRT);
    public static Item bronzeSword;
    public static Item ironPickaxe = new ItemTool(1.5f, ItemTool.ToolType.PICKAXE, Block.Type.STONE);
    public static Item ironShovel = new ItemTool(1.5f, ItemTool.ToolType.SHOVEL, Block.Type.DIRT);
    public static Item ironSword;
    public static Item goldPickaxe = new ItemTool(2f, ItemTool.ToolType.PICKAXE, Block.Type.STONE);
    public static Item goldShovel = new ItemTool(2f, ItemTool.ToolType.SHOVEL, Block.Type.DIRT);
    public static Item goldSword;
    public static Item glassShard = new Item().setName("Glass Shard").setTexture(1, 1);
    public static Item glorbDust = new Item().setName("Glorb Dust").setTexture(1, 2);
    public static Item flowerItem = new Item().setName("Flower").setTexture(1, 3);
    public static Item magnifyingGlass;
    public static Item minecart;

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
    public virtual ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, RaycastHit hit) {
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
