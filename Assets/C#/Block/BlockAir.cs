public class BlockAir : Block {
    public override MeshData renderBlockMesh(Chunk chunk, int x, int y, int z, MeshData meshData) {
        return meshData;
    }
}