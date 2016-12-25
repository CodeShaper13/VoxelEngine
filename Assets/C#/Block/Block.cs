using UnityEngine;
using System;

public class Block {
    private static byte NEXT_ID = 0;
    public static Block[] BLOCK_LIST = new Block[256];
    public static BlockModel MODEL_DEFAULT = new BlockModelCube();
    public static BlockModel MODEL_CROSS = new BlockModelCross();

    public static Block air = new BlockAir().setName("Air").setSolid(false).setReplaceable(true);
    public static Block stone = new BlockStone().setMineTime(1f).setTexture(0, 0).setType(Type.STONE);
    public static Block dirt = new Block().setName("Dirt").setMineTime(0.15f).setTexture(1, 0).setType(Type.DIRT);
    public static Block gravel = new Block().setName("Gravel").setMineTime(0).setTexture(0, 11).setType(Type.DIRT);
    public static Block moss;
    public static Block root;
    public static Block flower;
    public static Block lava;
    public static Block coalOre = new BlockOre(Item.coal, 5).setName("Coal Ore").setMineTime(0).setType(Type.STONE);
    public static Block bronzeOre = new BlockOre(Item.bronzeBar, 6).setName("Bronze Ore").setMineTime(0).setType(Type.STONE);
    public static Block ironOre = new BlockOre(Item.ironBar, 7).setName("Iron Ore").setMineTime(0).setType(Type.STONE);
    public static Block goldOre = new BlockOre(Item.goldBar, 8).setName("Gold Ore").setMineTime(0).setType(Type.STONE);
    public static Block rubyOre = new BlockOre(Item.ruby, 9).setName("Ruby Ore").setMineTime(0).setType(Type.STONE);
    public static Block uraniumOre = new BlockOre(Item.uranium, 10).setName("Uranium Ore").setMineTime(0).setType(Type.STONE);
    public static Block glorb = new BlockGlorb().setName("Glorb").setTexture(1, 11).setType(Type.STONE).setMineTime(1);
    public static Block lanturn;
    public static Block mushroom = new BlockMushroom(4).setName("Mushroom").setSolid(false).setMineTime(0.1f);
    public static Block mushroom2 = new BlockMushroom(5).setName("Mushroom").setSolid(false).setMineTime(0.1f);
    public static Block healingMushroom = new BlockMushroom(6).setName("Healshroom").setSolid(false).setMineTime(0.1f);
    public static Block poisonMushroom = new BlockMushroom(7).setName("Deathshroom").setSolid(false).setMineTime(0.1f);
    public static Block torch; //Burned out varient too with meta difference
    public static Block rail;
    public static Block door;
    public static Block mossyBrick = new Block().setName("Brick").setMineTime(0.5f).setTexture(2, 11).setType(Type.STONE);
    public static Block cable = new Block().setName("Cable");
    public static Block ironGrate = new Block().setName("Iron Grate").setType(Type.STONE);

    public static Block grass = new BlockGrass().setName("grass").setMineTime(0.15f);
    public static Block wood = new BlockWood().setName("log");

    //Fields:
    public byte id = 0;
    public string name = "null";
    public float mineTime;
    public TexturePos texturePos = new TexturePos(0, 0);
    public bool isSolid = true;
    public bool replaceable;
    public Type blockType;

    public Block() {
        this.id = Block.NEXT_ID++;
        Block.BLOCK_LIST[this.id] = this;
    }

    //neighborDir points to the block that made this update happen
    public virtual void onNeighborChange(World world, BlockPos pos, Direction neighborDir) {
        //TODO do we need to return a bool if the block changed, to make more chunks dirty?
    }
    
    public virtual void onRandomTick(World world, BlockPos pos, byte meta, int tickSeed) {
    }

    public virtual void onRightClick(World world, BlockPos pos, byte meta) {

    }

    public virtual void onPlace(World world, BlockPos pos, byte meta) {

    }

    //Called when the block is removed from World.setBlock()
    public virtual void onDestroy(World world, BlockPos pos, byte meta) {

    }

    //Checks if the passed direction is solid (solid faces are not rendered)
    public virtual bool isSideSolid(Direction direction) {
        return this.isSolid;
    }

    public virtual string getName(byte meta) {
        return this.name;
    }

    public virtual ItemStack[] getDrops(byte meta, ItemTool brokenWith) {
        return new ItemStack[] { new ItemStack(this.asItem(), meta) };
    }

    public virtual string getMagnifyingText(byte meta) {
        return this.getName(meta);
    }

    public virtual TexturePos getTexturePos(Direction direction, byte meta) {
        return this.texturePos;
    }

    public virtual BlockModel getModel(byte meta) {
        return Block.MODEL_DEFAULT;
    }

    //Used to check if the chunk needs to be redrawn after meta changes.  True if the chunk should be redrawn
    public virtual bool dirtyAfterMetaChange(BlockPos pos, byte newMeta) {
        return true;
    }

    public virtual MeshData renderBlock(Chunk chunk, int x, int y, int z, byte meta, MeshData meshData) {
        meshData.useRenderDataForCol = true; //TODO make this better, maybe allow blocks to override a methods that provides this value?
        bool[] renderFace = new bool[6];
        for (int i = 0; i < 6; i++) {
            Direction d = Direction.all[i];
            renderFace[i] = !chunk.getBlock(x + d.direction.x, y + d.direction.y, z + d.direction.z).isSideSolid(d);
        }
        BlockModel model = this.getModel(meta);
        model.preRender(this, meta, meshData);
        model.renderBlock(x, y, z, renderFace);
        return model.meshData;
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

    public Block setSolid(bool flag) {
        this.isSolid = flag;
        return this;
    }

    public Block setReplaceable(bool flag) {
        this.replaceable = flag;
        return this;
    }

    public Block setType(Type blockType) {
        this.blockType = blockType;
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