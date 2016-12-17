using UnityEngine;
using System;

public class Block {
    private static byte NEXT_ID = 0;
    public static Block[] BLOCK_LIST = new Block[256];
    public static BlockModel MODEL_DEFAULT = new BlockModelCube();
    public static BlockModel MODEL_CROSS = new BlockModelCross();

    //All blocks:
    public static Block air = new BlockAir().setName("air").setSolid(false).setReplaceable(true);
    public static Block stone = new Block().setName("stone").setMineTime(0.25f).setTexture(0, 0);
    public static Block dirt = new Block().setName("dirt").setMineTime(0.15f).setTexture(1, 0);
    public static Block grass = new BlockGrass().setName("grass").setMineTime(0.15f);
    public static Block wood = new BlockWood().setName("log");
    public static Block leaves = new Block().setName("leaves").setTexture(0, 1).setSolid(false);

    public static Block coal = new BlockOre().setName("coal").setMineTime(0.1f).setTexture(0, 3);
    public static Block bronze = new BlockOre().setName("bronze").setMineTime(0.1f).setTexture(2, 2);
    public static Block iron = new BlockOre().setName("iron").setMineTime(0.1f).setTexture(1, 2);
    public static Block gold = new BlockOre().setName("gold").setMineTime(0.1f).setTexture(3, 3);
    public static Block emerald = new BlockOre().setName("emerald").setMineTime(0.1f).setTexture(2, 3);
    public static Block ruby = new BlockOre().setName("ruby").setMineTime(0.1f).setTexture(0, 2);
    public static Block diamond = new BlockOre().setName("diamond").setMineTime(0.1f).setTexture(1, 3);

    //Fields:
    public byte id = 0;
    public string name = "null";
    public float mineTime = 0.0f;
    public TexturePos texturePos = new TexturePos(0, 0);
    public bool isSolid = true;
    public bool replaceable = false;

    public Block() {
        this.id = Block.NEXT_ID++;
        Block.BLOCK_LIST[this.id] = this;
    }

    //Checks if the passed direction is solid (solid faces are not rendered)
    public virtual bool isSideSolid(Direction direction) {
        return this.isSolid;
    }

    //Used to check if the chunk needs to be redrawn after meta changes.  True if the chunk should be redrawn
    public virtual bool dirtyAfterMetaChange(BlockPos pos, byte newMeta) {
        return false;
    }

    public virtual void onNeighborChange(BlockPos pos, Direction neighborDir) {
        //TODO do we need to return a bool if the block changed, to make more chunks dirty?
    }

    public virtual ItemStack[] getDrops(byte meta) {
        return new ItemStack[] {new ItemStack(this.asItem())};
    }

    public virtual MeshData renderBlock(Chunk chunk, int x, int y, int z, byte meta, MeshData meshData) {
        meshData.useRenderDataForCol = true; //TODO make this better, maybe allow blocks to override a methods that provides this value?
        bool[] renderFace = new bool[6];
        for(int i = 0; i < 6; i++) {
            Direction d = Direction.all[i];
            renderFace[i] = !chunk.getBlock(x + d.direction.x, y + d.direction.y, z + d.direction.z).isSideSolid(d);
        }
        BlockModel model = this.getModel(meta);
        model.preRender(this, meta, meshData);
        model.renderBlock(x, y, z, renderFace);
        return model.meshData;
    }

    public virtual BlockModel getModel(byte meta) {
        return Block.MODEL_DEFAULT;
    }

    //Returns the coords for the textures, depending on the pos
    public virtual TexturePos getTexturePos(Direction direction, byte meta) {
        return this.texturePos;
    }

    public virtual void onRandomTick(World world, BlockPos pos, byte meta) {
        throw new NotImplementedException();
    }

    public virtual void onRightClick(World world, BlockPos pos, byte meta) {

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


    public Item asItem() {
        return Item.ITEM_LIST[this.id];
    }

    public static Block getBlock(byte id) {
        return Block.BLOCK_LIST[id] != null ? Block.BLOCK_LIST[id] : Block.air;
    }
}