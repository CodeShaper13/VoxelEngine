using UnityEngine;

public class Block {
    private static byte NEXT_ID = 0;
    public static Block[] BLOCK_LIST = new Block[256];
    public static BlockModel DEFAULT_MODEL = new BlockModel();

    //All blocks:
    public static Block air = new BlockAir().setName("air").setSolid(false);
    public static Block stone = new Block().setName("stone").setMineTime(0.25f).setTexture(0, 0);
    public static Block dirt = new Block().setName("dirt").setTexture(1, 0);
    public static Block grass = new BlockGrass().setName("grass");
    public static Block wood = new BlockWood().setName("wood");
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

    public virtual MeshData renderBlock(Chunk chunk, int x, int y, int z, byte meta, MeshData meshData) {
        meshData.useRenderDataForCol = true; //TODO make this better, maybe allow blocks to override a methods that provides this value?
        bool[] renderFace = new bool[6];
        for(int i = 0; i < 6; i++) {
            Direction d = Direction.all[i];
            renderFace[i] = !chunk.getBlock(x + d.direction.x, y + d.direction.y, z + d.direction.z).isSideSolid(d);
        }
        return Block.DEFAULT_MODEL.renderBlock(x, y, z, this, meta, meshData, renderFace);
    }

    ////Renders the block's mesh
    //public virtual MeshData renderBlockMesh (Chunk chunk, int x, int y, int z, byte blockMeta, MeshData meshData) {
    //    meshData.useRenderDataForCol = true;
    //    if (!chunk.getBlock(x, y + 1, z).isSideSolid(Direction.DOWN)) {
    //        meshData = FaceDataUp(chunk, x, y, z, meshData);
    //    }
    //    if (!chunk.getBlock(x, y - 1, z).isSideSolid(Direction.UP)) {
    //        meshData = FaceDataDown(chunk, x, y, z, meshData);
    //    }
    //    if (!chunk.getBlock(x, y, z + 1).isSideSolid(Direction.SOUTH)) {
    //        meshData = FaceDataNorth(chunk, x, y, z, meshData);
    //    }
    //    if (!chunk.getBlock(x, y, z - 1).isSideSolid(Direction.NORTH)) {
    //        meshData = FaceDataSouth(chunk, x, y, z, meshData);
    //    }
    //    if (!chunk.getBlock(x + 1, y, z).isSideSolid(Direction.WEST)) {
    //        meshData = FaceDataEast(chunk, x, y, z, meshData);
    //    }
    //    if (!chunk.getBlock(x - 1, y, z).isSideSolid(Direction.EAST)) {
    //        meshData = FaceDataWest(chunk, x, y, z, meshData);
    //    }
    //    return meshData;
    //}

    //Returns the coords for the textures, depending on the pos
    public virtual TexturePos getTexturePos(Direction direction, byte meta) {
        return this.texturePos;
    }

    public virtual Vector2[] FaceUVs(Direction direction) {
        Vector2[] UVs = new Vector2[4];
        TexturePos tilePos = getTexturePos(direction, 0);

        UVs[0] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x + TexturePos.BLOCK_SIZE, TexturePos.BLOCK_SIZE * tilePos.y);
        UVs[1] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x + TexturePos.BLOCK_SIZE, TexturePos.BLOCK_SIZE * tilePos.y + TexturePos.BLOCK_SIZE);
        UVs[2] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x, TexturePos.BLOCK_SIZE * tilePos.y + TexturePos.BLOCK_SIZE);
        UVs[3] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x, TexturePos.BLOCK_SIZE * tilePos.y);
        return UVs;
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

    //Helper methods for translating block to id and back again.  If the id coresponds to a null block, returns air
    public static Block getBlock(byte id) {
        return Block.BLOCK_LIST[id] != null ? Block.BLOCK_LIST[id] : Block.air;
    }
}