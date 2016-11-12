using UnityEngine;

public class Block {
    private static byte NEXT_ID = 0;
    private static Block[] BLOCK_LIST = new Block[256];

    //All blocks:
    public static Block air = new BlockAir().setName("air").setSolid(false);
    public static Block stone = new Block().setName("stone").setMineTime(1).setTexture(0, 0);
    public static Block dirt = new Block().setName("dirt").setTexture(0, 1);
    public static Block grass = new BlockGrass().setName("grass");
    public static Block wood = new BlockWood().setName("wood");
    public static Block leaves = new Block().setName("leaves").setTexture(0, 1).setSolid(false);

    //Fields:
    public byte id = 0;
    public string name = "null";
    public float mineTime = 0.0f;
    public TexturePos texture = new TexturePos(0, 0);
    public bool isSolid = true;

    public Block() {
        this.id = Block.NEXT_ID++;
    }

    //Checks if the passed direction is solid (solid faces are not rendered)
    public virtual bool isSideSolid(Direction direction) {
        return this.isSolid;
    }

    //Renders the block's mesh
    public virtual MeshData renderBlockMesh (Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.useRenderDataForCol = true;
        if (!chunk.getBlock(x, y + 1, z).isSideSolid(Direction.down)) {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }
        if (!chunk.getBlock(x, y - 1, z).isSideSolid(Direction.up)) {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }
        if (!chunk.getBlock(x, y, z + 1).isSideSolid(Direction.south)) {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }
        if (!chunk.getBlock(x, y, z - 1).isSideSolid(Direction.north)) {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }
        if (!chunk.getBlock(x + 1, y, z).isSideSolid(Direction.west)) {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }
        if (!chunk.getBlock(x - 1, y, z).isSideSolid(Direction.east)) {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }
        return meshData;
    }

    //Adds all the faces to the mesh on the up side
    protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    //Adds all the faces to the mesh on the down side
    protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    //Adds all the faces to the mesh on the north side
    protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    //Adds all the faces to the mesh on the east side
    protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    //Adds all the faces to the mesh on the south side
    protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    //Adds all the faces to the mesh on the west side
    protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.west));
        return meshData;
    }

    //Returns the coords for the textures, depending on the pos
    public virtual TexturePos getTexturePos(Direction direction) {
        return this.texture;
    }

    public virtual Vector2[] FaceUVs(Direction direction) {
        Vector2[] UVs = new Vector2[4];
        TexturePos tilePos = getTexturePos(direction);

        UVs[0] = new Vector2(TexturePos.TILE_SIZE * tilePos.x + TexturePos.TILE_SIZE, TexturePos.TILE_SIZE * tilePos.y);
        UVs[1] = new Vector2(TexturePos.TILE_SIZE * tilePos.x + TexturePos.TILE_SIZE, TexturePos.TILE_SIZE * tilePos.y + TexturePos.TILE_SIZE);
        UVs[2] = new Vector2(TexturePos.TILE_SIZE * tilePos.x, TexturePos.TILE_SIZE * tilePos.y + TexturePos.TILE_SIZE);
        UVs[3] = new Vector2(TexturePos.TILE_SIZE * tilePos.x, TexturePos.TILE_SIZE * tilePos.y);
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
        this.texture = new TexturePos(x, y);
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