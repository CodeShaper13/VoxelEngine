using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation {

    public class PopulationArea {

        private Chunk[] chunks;

        public PopulationArea(World world, ChunkPos center) {
            this.chunks = new Chunk[27];

            for(int x = center.x - 1; x <= center.x + 1; x++) {
                for (int y = center.y - 1; y <= center.y + 1; y++) {
                    for (int z = center.z - 1; z <= center.z + 1; z++) {
                        this.chunks[(y * 3 * 3) + (z * 3) + x] = world.getChunk(new ChunkPos(x, y, z));
                    }
                }
            }
        }

        public Block getBlock(int x, int y, int z) {
            return null;
        }

        public void setState(int x, int y, int z, Block block, int meta) {

        }
    }
}
