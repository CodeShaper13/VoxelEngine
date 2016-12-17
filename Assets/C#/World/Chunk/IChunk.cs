public interface IChunk {

    Block getBlock(int x, int y, int z);

    Block setBlock(int x, int y, int z);

    byte getMeta(int x, int y, int z);

    byte setMeta(int x, int y, int z);
}
