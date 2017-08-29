using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Entities.Registry;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Render.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class Item {

        public static Item[] ITEM_LIST = new Item[512];

        public static Item pebble = new ItemThrowable(256, EntityRegistry.throwable).setName("Rock").setTexture(11, 2);
        public static Item coal = new Item(257).setName("Coal Lump").setTexture(12, 0);
        public static Item bronzeBar; // = new Item(258).setName("Bronze Bar").setTexture(0, 0);
        public static Item ironBar = new Item(259).setName("Iron Bar").setTexture(0, 0);
        public static Item goldBar; // = new Item(260).setName("Gold Bar").setTexture(0, 0);
        public static Item ruby = new Item(261).setName("Ruby").setTexture(11, 1);
        public static Item pickaxe = new ItemTool(262, 4f, EnumToolType.PICKAXE, EnumBlockType.STONE).setName("Pickaxe").setTexture(13, 0);
        public static Item shovel = new ItemTool(263, 4f, EnumToolType.SHOVEL, EnumBlockType.DIRT).setName("Shovel").setTexture(14, 0);
        public static Item axe = new ItemTool(264, 4f, EnumToolType.AXE, EnumBlockType.WOOD).setName("Axe").setTexture(15, 0);
        public static Item knife = new ItemSword(265, 2).setName("Knife").setTexture(0, 0);
        public static Item pistol; // 266
        public static Item shotgun; // 267
        public static Item rifle; // 268
        public static Item glassShard = new Item(269).setName("Glass Shard").setTexture(12, 1);
        public static Item flowerItem = new Item(270).setName("Flower").setTexture(12, 3);
        public static Item bone = new Item(271).setName("Bone").setTexture(11, 6);
        public static Item skull = new Item(272).setName("Skull").setTexture(12, 6);
        public static Item rawFish = new Item(273).setName("Raw Fish").setTexture(13, 6);
        public static Item cookedFish = new ItemFood(274, 10, 1).setName("Cooked Fish").setTexture(0, 0);
        public static Item corn = new ItemFood(275, 25, 5).setName("Corn").setTexture(13, 3);
        public static Item flesh = new ItemFood(276, 25, 5).setName("Flesh").setTexture(0, 0);
        public static Item mushroom = new ItemFood(277, 25, 5).setName("Mushroom").setTexture(0, 0);
        public static Item stew = new ItemFood(278, 25, 5).setName("Stew").setTexture(0, 0);
        public static Item carrot = new ItemFood(279, 25, 5).setName("Carrot").setTexture(13, 1);
        public static Item bucket = new Item(280).setName("Bucket").setTexture(12, 2);
        public static Item fishingRod = new Item(281).setName("Fishing Pole").setTexture(12, 5);
        public static Item magnifyingGlass = new ItemMagnifyingGlass(282).setName("Magnifying Glass").setTexture(12, 4);
        public static Item arrowhead = new Item(283).setName("Arrowhead").setTexture(15, 2);
        public static Item dynamite = new ItemDynamite(284).setName("Dynamite").setTexture(15, 4);
        public static Item minecart;
        public static Item topHat;
        public static Item goggles;
        public static Item pilotHat;
        public static Item trenchCoat;
        public static Item vest;
        public static Item combatBoots;
        public static Item smellyBoots;

        public int id = 256;
        private string name = "null";
        public TexturePos texturePos;
        public IRenderItem itemRenderer;
        public int maxStackSize = ItemStack.MAX_SIZE;

        public Item(int id) {
            if (Item.ITEM_LIST[id] != null) {
                throw new Exception("Two items may not have the same id of " + id);
            }

            this.id = id;
            Item.ITEM_LIST[this.id] = this;
            this.setRenderer(RenderManager.ITEM_RENDERER_BILLBOARD);
        }

        /// <summary>
        /// Callback for when the player clicks while holding this item.  Return the passed in item stack.
        /// </summary>
        public virtual ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            return stack;
        }

        public virtual int getStatesUsed() {
            return 1;
        }

        /// <summary>
        /// Returns the items name based on it's meta.
        /// </summary>
        public virtual string getName(int meta) {
            return this.name;
        }

        /// <summary>
        /// Returns the correct TexturePos for an item.
        /// </summary>
        public virtual TexturePos getItemTexturePos(int meta) {
            return this.texturePos;
        }

        /// <summary>
        /// Renders the item in the players hand.  Override for special held renderings.
        /// </summary>
        public virtual void renderAsHeldItem(int meta, int count, Transform handTransfrom) {
            MutableTransform mt = this.getHandTransform();
            Matrix4x4 matrix = Matrix4x4.TRS(
                handTransfrom.position + mt.position + new Vector3(),
                handTransfrom.rotation * mt.rotation,
                mt.scale);
            Graphics.DrawMesh(RenderManager.getItemMesh(this, meta, true), matrix, RenderManager.getMaterial(this.id), 0, null, 0, null, false, false);
        }

        /// <summary>
        /// Returns the MutableTransfrom to use when rendering an item in a container.
        /// </summary>
        public virtual MutableTransform getContainerTransfrom() {
            return new MutableTransform(Vector3.zero, Quaternion.identity, new Vector3(0.2f, 0.2f, 0.2f));
        }

        /// <summary>
        /// Returns the mutable transfrom to use when the item is held in the players hand.
        /// </summary>
        public virtual MutableTransform getHandTransform() {
            return new MutableTransform(new Vector3(0, 0, 0), Quaternion.Euler(0, -106, -2), new Vector3(0.2f, 0.2f, 0.2f));
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

        public Item setMaxStackSize(int size) {
            if(size > ItemStack.MAX_SIZE) {
                throw new Exception("An item's maxStackSize can not be greater than the global limit of " + ItemStack.MAX_SIZE + "!");
            }
            this.maxStackSize = size;
            return this;
        }
        
        /// <summary>
        /// Creates item versions of all the blocks.
        /// </summary>
        public static void initBlockItems() {
            foreach (Block b in Block.BLOCK_LIST) {
                if (b != null) {
                    Item item = new ItemBlock(b);
                    Item.ITEM_LIST[b.id] = item;
                }
            }
        }
    }
}
