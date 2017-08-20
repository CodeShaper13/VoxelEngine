using System;
using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.Render.NewSys;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    /// <summary>
    /// Base class for any block in the game.  You should never directly create any new block instances, use the static references below.
    /// </summary>
    public class Block {

        public static Block[] BLOCK_LIST = new Block[256];

        public static Block air = new BlockAir(0);
        public static Block stone = new BlockStone(1).setMineTime(3f);
        public static Block dirt = new Block(2).setName("Dirt").setMineTime(1.5f).setTexture(1, 0).setType(EnumBlockType.DIRT);
        public static Block grass = new BlockGrass(3).setName("grass");
        public static Block gravel = new BlockFalling(4).setName("Gravel").setMineTime(1.5f).setTexture(0, 11).setType(EnumBlockType.DIRT);
        public static Block coalOre = new BlockOre(5, Item.coal, 5).setName("Coal Ore").setMineTime(3f);
        public static Block bronzeOre = new BlockOre(6, null, 6).setName("Bronze Ore").setMineTime(3f);
        public static Block ironOre = new BlockOre(7, null, 6).setName("Iron Ore").setMineTime(3f);
        public static Block goldOre = new BlockOre(8, null, 9).setName("Gold Ore").setMineTime(3f);
        public static Block rubyOre = new BlockOre(9, Item.ruby, 8).setName("Ruby Ore").setMineTime(3f);
        public static Block water = new BlockFluid(10).setName("Water").setTexture(0, 12);
        public static Block lava = new BlockFluid(11).setName("Lava").setTexture(1, 12).setEmittedLight(5);
        public static Block cobblestone = new Block(12).setName("Cobblestone").setTexture(5, 2).setType(EnumBlockType.STONE).setMineTime(4);
        public static Block cobblestoneSlab = new BlockSlab(13, Block.cobblestone);
        public static Block cobblestoneStairs = new BlockStairs(14, Block.cobblestone);
        public static Block mushroom = new BlockMushroom(15, 4).setName("Mushroom");
        public static Block chest = new BlockChest(16).setName("Chest").setMineTime(2f);
        // 17
        public static Block torch = new BlockTorch(18).setName("Torch").setMineTime(0.5f);
        public static Block ladder = new BlockLadder(19).setName("Ladder").setMineTime(0.5f).setType(EnumBlockType.WOOD);
        public static Block fence = new BlockFence(20).setName("Fence").setMineTime(1.25f).setType(EnumBlockType.WOOD);
        public static Block ironFence = new Block(21).setName("Iron Fence").setType(EnumBlockType.STONE);
        public static Block plank = new Block(22).setName("Wood Plank").setTexture(5, 0).setType(EnumBlockType.WOOD).setMineTime(0.75f);
        public static Block plankSlab = new BlockSlab(23, Block.plank);
        public static Block plankStairs = new BlockStairs(24, Block.plank);
        public static Block brick = new Block(25).setName("Brick").setTexture(5, 1).setType(EnumBlockType.STONE).setMineTime(3f);
        public static Block brickSlab = new BlockSlab(26, Block.brick);
        public static Block brickStair = new BlockStairs(27, Block.brick);
        public static Block wood = new BlockWood(28).setName("Log").setMineTime(2);
        public static Block leaves = new Block(29).setName("Leaves").setTexture(0, 1).setTransparent();
        // 30
        public static Block roofWooden = new BlockRoof(31, Block.plank);
        //public static Block roofSlab = new BlockSlab(32, Block.roof);
        public static Block bookcase = new BlockBookcase(33).setName("Bookcase");
        public static Block glass = new Block(34).setName("Glass").setTransparent().setTexture(3, 1);
        public static Block rail = new BlockRail(35).setName("Rail").setMineTime(0.15f);
        public static Block door; // 36
        public static Block farmland = new BlockFarmland(37).setName("Farmland");
        public static Block bed = new BlockBed(38).setName("Bed");
        public static Block cornCrop = new BlockCrop(39, Item.corn, 3, 5, 7).setName("Corn");
        public static Block carrotCrop = new BlockCrop(40, Item.carrot, 2, 4, 3).setName("Carrots");
        public static Block wire = new BlockWire(43).setName("Wire");
        public static Block piston;
        public static Block pistonHead;
        public static Block lever;
        public static Block button;
        public static Block dial;
        public static Block gauge;
        public static Block lanternOn = new BlockLantern(50).setName("Lanturn").setTexture(5, 4).setEmittedLight(15);
        public static Block lanternOff = new BlockLantern(51).setName("Lanturn").setTexture(5, 5);
        public static Block logicAnd;
        public static Block logicOr;
        public static Block logicNot = new BlockLogicNot(54).setName("Logic NOT");
        public static Block diode;
        public static Block looker;
        public static Block updateChecker;
        public static Block blockPlacer;
        public static Block blockBreaker;
        public static Block memorySwitch;


        [Obsolete("Remember to update the placeholder with the correct block!")]
        public static Block placeholder = new Block(255).setName("PLACEHOLDER").setTexture(31, 31);

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
            if (Block.BLOCK_LIST[id] != null) {
                throw new Exception("Two blocks may not have the same id of " + this.id);
            }

            this.id = id;
            Block.BLOCK_LIST[this.id] = this;
            this.renderer = RenderManager.CUBE;
            this.setTexture(0, 0);
            this.statesUsed = 1;
            this.containerTransfrom = new MutableTransform(Vector3.zero, Quaternion.Euler(-9.2246f, 45.7556f, -9.346399f), new Vector3(0.125f, 0.125f, 0.125f));
        }

        /// <summary>
        /// Called on a block when one of it's neighbors changes.  neighborDir points to the block that made this happen.
        /// </summary>
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

        /// <summary>
        /// Returns the correct TexturePos for a block.
        /// </summary>
        public virtual TexturePos getTexturePos(Direction direction, int meta) {
            return this.texturePos;
        }

        [Obsolete("Use getUvPlane() instead!")]
        public virtual Vector2[] applyUvAlterations(Vector2[] uvs, int meta, Direction direction, Vector2 faceRadius, Vector2 faceOffset) {
            return uvs;
        }

        public virtual UvPlane getUvPlane(int meta, Direction direction) {
            TexturePos pos = this.getTexturePos(direction, meta);
            return new UvPlane(pos, 0, 0, 32, 32);
        }

        public virtual string getAsDebugText(int meta) {
            return this.getName(meta) + ":" + meta;
        }

        public virtual bool acceptsWire(Direction directionOfWire, int meta) {
            return false;
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