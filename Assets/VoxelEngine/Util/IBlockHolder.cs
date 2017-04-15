using VoxelEngine.Blocks;

namespace VoxelEngine.Util {
    
    public interface IBlockHolder {

        Block getBlock(int x, int y, int z);

        void setBlock(int x, int y, int z, Block block);

        int getMeta(int x, int y, int z);

        void setMeta(int x, int y, int z, int meta);
    }
}
