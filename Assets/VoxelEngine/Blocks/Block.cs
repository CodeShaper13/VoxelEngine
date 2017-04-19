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

        [Obsolete("Remember to update the placeholder with the correct block")]
        public static Block placeholder = new Block(255).setName("PLACEHOLDER").setTexture(0, 0);

        public static Block air = new BlockAir(0).setName("Air").setTransparent().setReplaceable().setRenderer(null);
        public static Block stone = new BlockStone(1).setMineTime(1f).setTexture(0, 0).setType(Type.STONE).setStatesUsed(4);
        public static Block dirt = new Block(2).setName("Dirt").setMineTime(0.15f).setTexture(1, 0).setType(Type.DIRT);
        public static Block grass = new BlockGrass(3).setName("grass").setMineTime(0.15f);
        public static Block gravel = new Block(4).setName("Gravel").setMineTime(0).setTexture(0, 11).setType(Type.DIRT);
        public static Block coalOre = new BlockOre(5, Item.coal, 5).setName("Coal Ore").setMineTime(0).setType(Type.STONE);
        public static Block bronzeOre = new BlockOre(6, Item.bronzeBar, 6).setName("Bronze Ore").setMineTime(0).setType(Type.STONE);
        public static Block ironOre = new BlockOre(7, Item.ironBar, 7).setName("Iron Ore").setMineTime(0).setType(Type.STONE);
        public static Block goldOre = new BlockOre(8, Item.goldBar, 8).setName("Gold Ore").setMineTime(0).setType(Type.STONE);
        public static Block rubyOre = new BlockOre(9, Item.ruby, 9).setName("Ruby Ore").setMineTime(0).setType(Type.STONE);
        public static Block uraniumOre = new BlockOre(10, Item.uranium, 10).setName("Uranium Ore").setMineTime(0).setType(Type.STONE);
        public static Block mushroom = new BlockMushroom(12, 4).setName("Mushroom").setEmittedLight(3); // TODO Debuging light
        public static Block mushroom2 = new BlockMushroom(13, 5).setName("Mushroom");
        public static Block healingMushroom = new BlockMushroom(14, 6).setName("Healshroom");
        public static Block poisonMushroom = new BlockMushroom(15, 7).setName("Deathshroom");
        public static Block mossyBrick = new Block(16).setName("Brick").setMineTime(0.5f).setTexture(2, 11).setType(Type.STONE);
        public static Block cable = new Block(17).setName("Cable");
        public static Block ironGrate = new Block(18).setName("Iron Grate").setType(Type.STONE);
        public static Block chest = new BlockChest(19).setName("Chest").setTransparent();
        public static Block lantern = new BlockLantern(20).setName("Lanturn").setTransparent().setRenderAsItem(1, 1);
        public static Block torch = new BlockTorch(21).setName("Torch").setTransparent();
        public static Block ladder = new BlockLadder(22).setName("Ladder");
        public static Block rail = new BlockRail(32).setName("Rail").setTransparent().setRenderer(RenderManager.RAIL);//.setRenderFlat();
        public static Block fence = new Block(33).setName("Fence").setTransparent().setRenderer(RenderManager.FENCE);
        public static Block plank = new Block(34).setName("Plank");
        public static Block wood = new BlockWood(36).setName("Log").setStatesUsed(3);
        public static Block stoneSlab = new BlockSlab(35, Block.wood);
        public static Block leaves = new Block(37).setName("Leaves").setTexture(0, 1).setTransparent();
        public static Block water = new BlockFluid(38).setName("Water").setTexture(0, 12).setRenderer(RenderManager.FLUID);
        public static Block lava = new BlockFluid(39).setName("Lava").setTexture(1, 12).setRenderer(RenderManager.FLUID).setEmittedLight(5);

        public static Block moss;
        public static Block root;
        public static Block flower;
        public static Block door;

        public static Block glorb = new BlockGlorb(11).setName("Glorb").setTexture(1, 11).setType(Type.STONE).setMineTime(1);

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
        public Type blockType;
        public BlockRenderer renderer;
        /// <summary> The number of meta data numbers used.  Used in prerendering held item meshes. </summary>
        public int statesUsed;
        /// <summary> The amount of light given off by the block, 0-15.  0 is none. </summary>
        public int emittedLight = 0;

        // Unused???
        public bool renderAsItem;
        public TexturePos itemAtlasPos;

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

        public Block setType(Type blockType) {
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

        public Block setRenderAsItem(int x, int y) {
            this.renderAsItem = true;
            this.itemAtlasPos = new TexturePos(x, y);
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
        /// Returns the block with the corresponding id.
        /// </summary>
        public static Block getBlock(int id) {
            return (Block.BLOCK_LIST[id] != null || id > Block.BLOCK_LIST.Length) ? Block.BLOCK_LIST[id] : Block.air;
        }

        public enum Type {
            NORMAL,
            STONE,
            DIRT
        }
    }
}