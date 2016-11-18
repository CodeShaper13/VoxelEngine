using UnityEngine;

//A chunk in the game
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour {
    public const int SIZE = 16;
    public const int BLOCK_COUNT = Chunk.SIZE * Chunk.SIZE * Chunk.SIZE;

    public Block[] blocks = new Block[Chunk.BLOCK_COUNT];
    public byte[] metaData = new byte[Chunk.BLOCK_COUNT];

    //When true, the chunk will be redrawn when updated
    public bool dirty = false;
    //Has the chunk been rendered after generation?
    public bool rendered;

    //Has the chunk been populated?
    public bool populated = false;

    private MeshFilter filter;
    private MeshCollider coll;
    public World world;

    //In world coordinates, not chunk
    public BlockPos pos;
    public int chunkX, chunkY, chunkZ;

    void Awake() {
        this.filter = this.GetComponent<MeshFilter>();
        this.coll = this.GetComponent<MeshCollider>();
    }

    void Update() {
        if (dirty) {
            dirty = false;
            this.renderChunk();
        }
    }

    //Like a constructor, but since this is a GameObject it can't have one.
    public void initChunk(World w, BlockPos pos) {
        this.world = w;
        this.pos = pos;
        BlockPos p = pos.toChunkPos();
        this.chunkX = p.x;
        this.chunkY = p.y;
        this.chunkZ = p.z;
        this.gameObject.name = "Chunk" + p;
        this.transform.parent = this.world.transform;
    }

    //For the following for methods, make sure the pos is between 0 and 15

    public Block getBlock(int x, int y, int z) {
        if (Util.inChunkBounds(x) && Util.inChunkBounds(y) && Util.inChunkBounds(z)) {
            return this.blocks[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
        }
        return world.getBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    //This should only be used in world generation, or a case where the neighbor blocks should not be updated
    public void setBlock(int x, int y, int z, Block block) {
        this.blocks[x + Chunk.SIZE * (z + Chunk.SIZE * y)] = block;
    }

    public byte getMeta(int x, int y, int z) {
        return this.metaData[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
    }

    //This should only be used in world generation, or a case where the neighbor blocks should not be updated
    public void setMeta(int x, int y, int z, byte meta) {
        this.metaData[x + Chunk.SIZE * (z + Chunk.SIZE * y)] = meta;
    } 

    //Renders all the blocks within the chunk
    void renderChunk() {
        this.rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < Chunk.SIZE; x++) {
            for (int y = 0; y < Chunk.SIZE; y++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    meshData = this.getBlock(x, y, z).renderBlock(this, x, y, z, this.getMeta(x, y, z), meshData);
                    //meshData = this.getBlock(x, y, z).renderBlockMesh(this, x, y, z, this.getMeta(x, y, z), meshData);
                }
            }
        }
        this.applyChunkMesh(meshData);
    }

    //Applies the new render and collision mesh to the chunks components
    void applyChunkMesh(MeshData meshData) {
        this.filter.mesh.Clear();
        this.filter.mesh.vertices = meshData.vertices.ToArray();
        this.filter.mesh.triangles = meshData.triangles.ToArray();

        this.filter.mesh.uv = meshData.uv.ToArray();
        this.filter.mesh.RecalculateNormals();

        this.coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        this.coll.sharedMesh = mesh;
    }
}