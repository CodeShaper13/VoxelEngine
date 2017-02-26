using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Render.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class Item {

        public static Item[] ITEM_LIST = new Item[512];

        public static IRenderItem RENDER_BILLBOARD = new RenderItemBillboard();

        public static Item pebble = new ItemThrowable(256).setName("Pebble").setTexture(1, 0);
        public static Item coal = new Item(257).setName("Coal").setTexture(0, 0);
        public static Item bronzeBar = new Item(258).setName("Bronze Bar").setTexture(0, 1);
        public static Item ironBar = new Item(259).setName("Iron Bar").setTexture(0, 2);
        public static Item goldBar = new Item(260).setName("Gold Bar").setTexture(0, 3);
        public static Item ruby = new Item(261).setName("Ruby").setTexture(0, 4);
        public static Item uranium = new Item(262).setName("Uranium").setTexture(0, 5);
        public static Item bronzePickaxe = new ItemTool(263, 1.25f, ItemTool.ToolType.PICKAXE, Block.Type.STONE).setName("Bronze Pickaxe").setTexture(2, 0);
        public static Item bronzeShovel = new ItemTool(264, 1.25f, ItemTool.ToolType.SHOVEL, Block.Type.DIRT).setName("Bronze Shovel").setTexture(3, 0);
        public static Item bronzeSword = new ItemSword(265, 2).setName("Bronze Sword").setTexture(4, 0);
        public static Item ironPickaxe = new ItemTool(266, 1.5f, ItemTool.ToolType.PICKAXE, Block.Type.STONE).setName("Iron Pickaxe").setTexture(2, 1);
        public static Item ironShovel = new ItemTool(267, 1.5f, ItemTool.ToolType.SHOVEL, Block.Type.DIRT).setName("Iron Shovel").setTexture(3, 1);
        public static Item ironSword = new ItemSword(268, 4).setName("Iron Sword").setTexture(4, 1);
        public static Item goldPickaxe = new ItemTool(269, 2f, ItemTool.ToolType.PICKAXE, Block.Type.STONE).setName("Gold Pickaxe").setTexture(2, 2);
        public static Item goldShovel = new ItemTool(270, 2f, ItemTool.ToolType.SHOVEL, Block.Type.DIRT).setName("Gold Shovel").setTexture(3, 2);
        public static Item goldSword = new ItemSword(271, 5).setName("Gold Sword").setTexture(4, 2);
        public static Item glassShard = new Item(272).setName("Glass Shard").setTexture(1, 1);
        public static Item glorbDust = new Item(273).setName("Glorb Dust").setTexture(1, 2);
        public static Item flowerItem = new Item(274).setName("Flower").setTexture(1, 3);
        public static Item magnifyingGlass = new ItemMagnifyingGlass(275).setName("Magnifying Glass").setTexture(1, 4);
        public static Item food = new ItemFood(276, 25, 5).setName("Food").setTexture(0, 0);
        public static Item bronzeHelmet;
        public static Item bronzeChestplate;
        public static Item ironHelmet;
        public static Item ironChestplate;
        public static Item goldHelmet;
        public static Item goldChestplate;
        public static Item minecart;

        public int id = 256;
        private string name = "null";
        public TexturePos texturePos;
        public IRenderItem itemRenderer;
        public Mesh[] preRenderedMeshes;

        public Item(int id) {
            this.id = id;

            if (Item.ITEM_LIST[this.id] != null) {
                Debug.Log("ERROR!  Two items may not have the same id " + this.id);
            } else {
                Item.ITEM_LIST[this.id] = this;
            }

            this.setRenderer(Item.RENDER_BILLBOARD);
        }

        public virtual ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            return stack;
        }

        public virtual byte getStatesUsed() {
            return 1;
        }

        public virtual string getName(byte meta) {
            return this.name;
        }

        public Mesh getPreRenderedMesh(byte meta) {
            if(meta >= this.preRenderedMeshes.Length) {
                Debug.Log("Could not find prerendered mesh for " + this.getName(meta) + ":" + meta);
                return Block.placeholder.asItem().getPreRenderedMesh(0);
            } else {
                return this.preRenderedMeshes[meta];
            }
        }
        

        public Item setName(string name) {
            this.name = name;
            return this;
        }

        public Item setRenderer(IRenderItem data) {
            this.itemRenderer = data;
            return this;
        }

        public Item setTexture(int x, int y) {
            this.texturePos = new TexturePos(x, y);
            return this;
        }

        // Creates item versions of all the blocks
        public static void initBlockItems() {
            foreach (Block b in Block.BLOCK_LIST) {
                if (b != null) {
                    Item item = new ItemBlock(b);
                    Item.ITEM_LIST[b.id] = item;
                }
            }
        }

        // Prerenders all the items, saving the meshes in Item.preRenderedMeshes
        public static void preRenderItems() {
            for (int i = 0; i < Item.ITEM_LIST.Length; i++) {
                Item item = Item.ITEM_LIST[i];
                if (item != null) {
                    item.preRenderedMeshes = new Mesh[item.getStatesUsed()];
                    for (byte j = 0; j < item.preRenderedMeshes.Length; j++) {
                        item.preRenderedMeshes[j] = item.itemRenderer.renderItem(item, j);
                    }
                }
            }
        }
    }
}
