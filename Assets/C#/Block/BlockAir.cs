public class BlockAir : Block {

    public override MeshData renderBlock(Chunk chunk, int x, int y, int z, byte meta, MeshData meshData) {
        return meshData;
    }
}