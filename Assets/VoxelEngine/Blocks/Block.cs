using System;
using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {
    public class Block {

        public static Block[] BLOCK_LIST = new Block[256];

        public static Block air = new BlockAir(0).setName("Air").setTransparent().setReplaceable().setRenderer(null);
        public static Block stone = new BlockStone(1).setMineTime(1f).setTexture(0, 0).setType(Type.STONE).setStatesUsed(5);
        public static Block dirt = new Block(2).setName("Dirt").setMineTime(0.15f).setTexture(1, 0).setType(Type.DIRT);
        public static Block gravel = new Block(3).setName("Gravel").setMineTime(0).setTexture(0, 11).setType(Type.DIRT);
        public static Block lava = new BlockLava(4).setName("Lava").setTexture(0, 12).setReplaceable();//.setSolid(false);
        public static Block coalOre = new BlockOre(5, Item.coal, 5).setName("Coal Ore").setMineTime(0).setType(Type.STONE);
        public static Block bronzeOre = new BlockOre(6, Item.bronzeBar, 6).setName("Bronze Ore").setMineTime(0).setType(Type.STONE);
        public static Block ironOre = new BlockOre(7, Item.ironBar, 7).setName("Iron Ore").setMineTime(0).setType(Type.STONE);
        public static Block goldOre = new BlockOre(8, Item.goldBar, 8).setName("Gold Ore").setMineTime(0).setType(Type.STONE);
        public static Block rubyOre = new BlockOre(9, Item.ruby, 9).setName("Ruby Ore").setMineTime(0).setType(Type.STONE);
        public static Block uraniumOre = new BlockOre(10, Item.uranium, 10).setName("Uranium Ore").setMineTime(0).setType(Type.STONE);
        public static Block glorb = new BlockGlorb(11).setName("Glorb").setTexture(1, 11).setType(Type.STONE).setMineTime(1);
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
        public static Block rail = new BlockRail(32).setName("Rail").setTransparent().setRenderer(BlockRenderer.RAIL);//.setRenderFlat();
        public static Block fence = new Block(33).setName("Fence").setTransparent().setRenderer(BlockRenderer.FENCE);
        public static Block plank = new Block(34).setName("Plank");
        public static Block moss;
        public static Block root;
        public static Block flower;
        public static Block door;

        public static Block grass = new BlockGrass(100).setName("grass").setMineTime(0.15f);
        public static Block wood = new BlockWood(101).setName("log").setStatesUsed(3);

        [Obsolete("Remember to update the placeholder with the correct block")]
        public static Block placeholder = new Block(255).setName("PLACEHOLDER").setTexture(15, 15);

        public byte id = 0;
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
        /// <summary> The highest used meta data, used in prerendering meshes for the hud. </summary>
        public byte statesUsed;
        /// <summary> The amount of light given off by the block, 0-15.  0 is none. </summary>
        public int emittedLight = 0;

        // Unused???
        public bool renderAsItem;
        public TexturePos itemAtlasPos;

        public Block(byte id) {
            this.id = id;
            if (Block.BLOCK_LIST[this.id] != null) {
                Debug.Log("ERROR!  Two blocks may not have the same id " + this.id);
            } else {
                Block.BLOCK_LIST[this.id] = this;
            }
            this.renderer = BlockRenderer.CUBE;

            this.setTexture(0, 0);

            this.statesUsed = 1;
        }

        //neighborDir points to the block that made this update happen
        public virtual void onNeighborChange(World world, BlockPos pos, byte meta, Direction neighborDir) {
            //TODO do we need to return a bool if the block changed, to make more chunks dirty?
        }

        // Unused?
        public virtual void onTick(World world, BlockPos pos) {}

        public virtual void onRandomTick(World world, int x, int y, int z, byte meta, int tickSeed) { }

        // Returns true if something happened
        public virtual bool onRightClick(World world, EntityPlayer player, BlockPos pos, byte meta) {
            return false;
        }

        public virtual void onPlace(World world, BlockPos pos, byte meta) { }

        //Called when the block is removed from World.setBlock()
        public virtual void onDestroy(World world, BlockPos pos, byte meta) {}

        public virtual string getName(byte meta) {
            return this.name;
        }

        public virtual ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            return new ItemStack[] { new ItemStack(this.asItem(), meta) };
        }

        public virtual string getMagnifyingText(byte meta) {
            return this.getName(meta);
        }

        public virtual TexturePos getTexturePos(Direction direction, byte meta) {
            return this.texturePos;
        }

        public virtual Vector2[] getUVs(byte meta, Direction direction, Vector2[] uvArray) {
            TexturePos tilePos = this.getTexturePos(direction, meta);
            float x = TexturePos.BLOCK_SIZE * tilePos.x;
            float y = TexturePos.BLOCK_SIZE * tilePos.y;
            uvArray[0] = new Vector2(x, y);
            uvArray[1] = new Vector2(x, y + TexturePos.BLOCK_SIZE);
            uvArray[2] = new Vector2(x + TexturePos.BLOCK_SIZE, y + TexturePos.BLOCK_SIZE);
            uvArray[3] = new Vector2(x + TexturePos.BLOCK_SIZE, y);
            return uvArray;
        }

        public virtual byte adjustMetaOnPlace(World world, BlockPos pos, byte meta, Direction clickedDirNormal, Vector3 angle) {
            return meta;
        }

        public virtual bool isValidPlaceLocation(World world, BlockPos pos, byte meta, Direction clickedDirNormal) {
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

        public Block setStatesUsed(byte statesUsed) {
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


        public Item asItem() {
            return Item.ITEM_LIST[this.id];
        }

        public static Block getBlock(byte id) {
            return Block.BLOCK_LIST[id] != null ? Block.BLOCK_LIST[id] : Block.air;
        }

        public enum Type {
            NORMAL,
            STONE,
            DIRT
        }
    }
}