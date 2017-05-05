using System;
using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    /// <summary>
    /// Base class for any block in the game.  You should never directly create any new block instances, use the static references below.
    /// </summary>
    public class Block {

        public static Block[] BLOCK_LIST = new Block[256];

        public static Block air = new BlockAir(0).setName("Air");
        public static Block stone = new BlockStone(1).setMineTime(1f);
        public static Block dirt = new Block(2).setName("Dirt").setMineTime(0.15f).setTexture(1, 0).setType(EnumBlockType.DIRT);
        public static Block grass = new BlockGrass(3).setName("grass");
        public static Block gravel = new Block(4).setName("Gravel").setMineTime(0).setTexture(0, 11).setType(EnumBlockType.DIRT);
        public static Block coalOre = new BlockOre(5, Item.coal, 5).setName("Coal Ore").setMineTime(0);
        public static Block bronzeOre = new BlockOre(6, Item.bronzeBar, 6).setName("Bronze Ore").setMineTime(0);
        public static Block ironOre = new BlockOre(7, Item.ironBar, 7).setName("Iron Ore").setMineTime(0);
        public static Block goldOre = new BlockOre(8, Item.goldBar, 8).setName("Gold Ore").setMineTime(0);
        public static Block rubyOre = new BlockOre(9, Item.ruby, 9).setName("Ruby Ore").setMineTime(0);
        public static Block water = new BlockFluid(10).setName("Water").setTexture(0, 12);
        public static Block lava = new BlockFluid(11).setName("Lava").setTexture(1, 12).setEmittedLight(5);
        public static Block cornCrop = new BlockCrop(12, Item.corn, 3, 5, 7).setName("Corn");
        public static Block carrotCrop = new BlockCrop(13, Item.carrot, 2, 4, 3).setName("Carrots");
        public static Block mushroom = new BlockMushroom(14, 4).setName("Mushroom");
        public static Block chest = new BlockChest(16).setName("Chest");
        public static Block lantern = new BlockLantern(17).setName("Lanturn");
        public static Block torch = new BlockTorch(18).setName("Torch");
        public static Block ladder = new BlockLadder(19).setName("Ladder");
        public static Block fence = new Block(20).setName("Fence").setTransparent().setRenderer(RenderManager.FENCE).setType(EnumBlockType.WOOD);
        public static Block ironFence = new Block(21).setName("Iron Fence").setType(EnumBlockType.STONE);
        public static Block plank = new Block(22).setName("Wood Plank").setTexture(5, 0).setType(EnumBlockType.WOOD);
        public static Block plankSlab = new BlockSlab(23, Block.plank);
        public static Block plankStair; // 24
        public static Block brick = new Block(25).setName("Brick").setTexture(5, 1).setType(EnumBlockType.STONE);
        public static Block brickSlab = new BlockSlab(26, Block.brick);
        public static Block brickStair; // 27
        public static Block wood = new BlockWood(28).setName("Log");
        public static Block leaves = new Block(29).setName("Leaves").setTexture(0, 1).setTransparent();
        public static Block cobblestone = new Block(30).setName("Cobblestone").setTexture(5, 2).setType(EnumBlockType.STONE);
        public static Block roof = new Block(31).setName("Roof").setType(EnumBlockType.WOOD).setTexture(5, 3);
        public static Block roofSlab = new BlockSlab(32, Block.roof);
        public static Block roodStair; // 33
        public static Block glass = new Block(34).setName("Glass").setTransparent().setTexture(3, 1);
        public static Block rail = new BlockRail(35).setName("Rail");
        public static Block door; // 36
        public static Block farmland = new BlockFarmland(37).setName("Farmland");

        [Obsolete("Remember to update the placeholder with the correct block")]
        public static Block placeholder = new Block(255).setName("PLACEHOLDER").setTexture(0, 0);

        /// <summary> The blocks id, there will never be duplicate. </summary>
        public int id = 0;
        public string name = "null";
        /// <summary> In seconds how long it takes to mine the block. </summary>
        public float mineTime;
        public TexturePos texturePos;
        /// <summary> Is the block a full 1x1x1 cube?  Commonly used to see if it can support other blocks, like ladders.  Also used to cull faces that are against solid blocks </summary>
        public bool isSolid = true;
        /// <summary> Can other blocks replace this one.  Air is replaceable. </summary>
        public bool replaceable;
        /// <summary> The blocks type.  Used by tools to increase efficiency. </summary>
        public EnumBlockType blockType;
        public BlockRenderer renderer;
        /// <summary> The number of meta data numbers used.  Used in prerendering held item meshes. </summary>
        public int statesUsed;
        /// <summary> The amount of light given off by the block, 0-15.  0 is none. </summary>
        public int emittedLight = 0;
        /// <summary>  The MutableTransfrom to use when rendered in a container. </summary>
        public MutableTransform containerTransfrom;

        public Block(int id) {
            this.id = id;
            if (Block.BLOCK_LIST[this.id] != null) {
                Debug.Log("ERROR!  Two blocks may not have the same id " + this.id);
            } else {
                Block.BLOCK_LIST[this.id] = this;
            }
            this.renderer = RenderManager.CUBE;
            this.setTexture(0, 0);
            this.statesUsed = 1;
            this.containerTransfrom = new MutableTransform(Vector3.zero, Quaternion.Euler(-9.2246f, 45.7556f, -9.346399f), new Vector3(0.125f, 0.125f, 0.125f));
        }

        //neighborDir points to the block that made this update happen
        public virtual void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            //TODO do we need to return a bool if the block changed, to make more chunks dirty?
        }

        /// <summary>
        /// Called randomly on blocks.
        /// </summary>
        public virtual void onRandomTick(World world, int x, int y, int z, int meta, int tickSeed) { }

        /// <summary>
        /// Called when the player clicks a block.  Return true to stop further prossessing,
        /// eg. the held ItemBlock being placed.
        /// </summary>
        public virtual bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace) {
            return false;
        }

        /// <summary>
        /// Called when a block is placed in World.setBlock().
        /// </summary>
        public virtual void onPlace(World world, BlockPos pos, int meta) { }

        /// <summary>
        /// Called right before a block is removed from World.setBlock(). Used by TileEntities to remove itself from the chunk and provide cleanup.
        /// </summary>
        public virtual void onDestroy(World world, BlockPos pos, int meta) { }

        /// <summary>
        /// Returns the name of a block.  Override if a block changes name based on meta.
        /// </summary>
        public virtual string getName(int meta) {
            return this.name;
        }

        /// <summary>
        /// Returns an array of ItemStacks that this block should drop.
        /// </summary>
        public virtual ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return new ItemStack[] { new ItemStack(this.asItem(), (byte)meta) };
        }

        public virtual string getMagnifyingText(int meta) {
            return this.getName(meta);
        }

        /// <summary>
        /// Returns the correct TexturePos for a block.
        /// </summary>
        public virtual TexturePos getTexturePos(Direction direction, int meta) {
            return this.texturePos;
        }

        /// <summary>
        /// Returns an array of uvs to use for the corresponding face of a block.
        /// </summary>
        public virtual Vector2[] getUVs(int meta, Direction direction, Vector2[] uvArray) {
            TexturePos tilePos = this.getTexturePos(direction, meta);
            float x = TexturePos.BLOCK_SIZE * tilePos.x;
            float y = TexturePos.BLOCK_SIZE * tilePos.y;
            uvArray[0] = new Vector2(x, y);
            uvArray[1] = new Vector2(x, y + TexturePos.BLOCK_SIZE);
            uvArray[2] = new Vector2(x + TexturePos.BLOCK_SIZE, y + TexturePos.BLOCK_SIZE);
            uvArray[3] = new Vector2(x + TexturePos.BLOCK_SIZE, y);
            return uvArray;
        }

        /// <summary>
        /// Called by ItemBlock when a block is placed.  By default the held meta is used, but this method can change it to a different meta to be used.
        /// </summary>
        public virtual int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return meta;
        }

        /// <summary>
        /// Called by item block to determin if the block trying to be placed is in a vlid spot,
        /// stops torches from going on non solid blocks, like fences, etc...
        /// </summary>
        public virtual bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal) {
            return true;
        }

        ////////////////////////////////
        // Constructor helper methods //
        ////////////////////////////////
        public Block setName(string name) {
            this.name = name;
            return this;
        }

        public Block setMineTime(float time) {
            this.mineTime = time;
            return this;
        }

        public Block setTexture(int x, int y) {
            this.texturePos = new TexturePos(x, y);
            return this;
        }

        public Block setTransparent() {
            this.isSolid = false;
            return this;
        }

        public Block setReplaceable() {
            this.replaceable = true;
            return this;
        }

        public Block setType(EnumBlockType blockType) {
            this.blockType = blockType;
            return this;
        }

        public Block setRenderer(BlockRenderer renderer) {
            this.renderer = renderer;
            return this;
        }

        /// <summary>
        /// The number of meta values used, NOT the maximum used.  If a
        /// block had an (on) and (off) state, this value would be 2.
        /// </summary>
        public Block setStatesUsed(int statesUsed) {
            this.statesUsed = statesUsed;
            return this;
        }

        public Block setContainerTransfrom(MutableTransform transfrom) {
            this.containerTransfrom = transfrom;
            return this;
        }

        public Block setEmittedLight(int level) {
            if(level > 15) {
                Debug.Log("A Block can't emitt light greater than 15!");
                this.emittedLight = 15;
            } else {
                this.emittedLight = level;
            }
            return this;
        }

        /// <summary>
        /// Returns the Block as an item, it will be an instance of ItemBlock.
        /// </summary>
        /// <returns></returns>
        public Item asItem() {
            return Item.ITEM_LIST[this.id];
        }

        /// <summary>
        /// Returns the block with the corresponding id, or air if the block could not be found.
        /// </summary>
        public static Block getBlockFromId(int id) {
            return (id > 0 && id < Block.BLOCK_LIST.Length && Block.BLOCK_LIST[id] != null) ? Block.BLOCK_LIST[id] : Block.air;
        }
    }
}