using VoxelEngine.Blocks;

namespace VoxelEngine.Level {

    public interface IChunk {

        Block getBlock(int x, int y, int z);

        int getLight(int x, int y, int z);
    }
}
