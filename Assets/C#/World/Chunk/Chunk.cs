using UnityEngine;

public class Chunk : MonoBehaviour {
    public const int SIZE = 16;
    public const int BLOCK_COUNT = Chunk.SIZE * Chunk.SIZE * Chunk.SIZE;

    public Block[] blocks = new Block[Chunk.BLOCK_COUNT];
    public byte[] metaData = new byte[Chunk.BLOCK_COUNT];

    public bool isNeedingSave = false;
    public bool isDirty = false;
    public bool isPopulated = false;

    private MeshFilter filter;
    private MeshCollider coll;
    public World world;

    public BlockPos pos;
    public ChunkPos chunkPos;

    public void Awake() {
        this.filter = this.GetComponent<MeshFilter>();
        this.coll = this.GetComponent<MeshCollider>();
    }

    //Like a constructor, but since this is a GameObject it can't have one.
    public void initChunk(World w, ChunkPos pos) {
        this.world = w;
        this.pos = pos.toBlockPos();
        this.chunkPos = pos;
        this.gameObject.name = "Chunk" + this.chunkPos;
    }

    public void updateChunk() {
        this.tickBlocks();
        if (isDirty) {
            isDirty = false;
            this.renderChunk();
        }
    }

    private void tickBlocks() {
        int i = Random.Range(int.MinValue, int.MaxValue);
        for (int j = 0; j < 3; j++) {
            int x = (i >> j * 12) & 0x0F;     // 0  12
            int y = (i >> j * 12 + 4) & 0x0F; // 4  16
            int z = (i >> j * 12 + 8) & 0x0F; // 8  20
            this.getBlock(x, y, z).onRandomTick(this.world, new BlockPos(x + this.pos.x, y + this.pos.y, z + this.pos.z), this.getMeta(x, y, z), i);
        }
    }

    public Block getBlock(int x, int y, int z) {
        if (Util.inChunkBounds(x) && Util.inChunkBounds(y) && Util.inChunkBounds(z)) {
            return this.blocks[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
        }
        return world.getBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    //This should only be used in world generation, or a case where the neighbor blocks should not be updated
    public void setBlock(int x, int y, int z, Block block) {
        this.isNeedingSave = true;
        this.blocks[x + Chunk.SIZE * (z + Chunk.SIZE * y)] = block;
    }

    public byte getMeta(int x, int y, int z) {
        return this.metaData[x + Chunk.SIZE * (z + Chunk.SIZE * y)];
    }

    //This should only be used in world generation, or a case where the neighbor blocks should not be updated
    public void setMeta(int x, int y, int z, byte meta) {
        this.isNeedingSave = true;
        this.metaData[x + Chunk.SIZE * (z + Chunk.SIZE * y)] = meta;
    } 

    //Renders all the blocks within the chunk
    private void renderChunk() {
        MeshData meshData = new MeshData();

        for (int x = 0; x < Chunk.SIZE; x++) {
            for (int y = 0; y < Chunk.SIZE; y++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    meshData = this.getBlock(x, y, z).renderBlock(this, x, y, z, this.getMeta(x, y, z), meshData);
                }
            }
        }
        Mesh mesh = meshData.toMesh();
        Mesh colMesh = new Mesh();
        colMesh.vertices = meshData.colVertices.ToArray();
        colMesh.triangles = meshData.colTriangles.ToArray();
        colMesh.RecalculateNormals();

        this.filter.mesh = mesh;
        this.coll.sharedMesh = colMesh;
    }
}