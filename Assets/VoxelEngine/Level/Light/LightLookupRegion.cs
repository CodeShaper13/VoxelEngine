using VoxelEngine.Blocks;

namespace VoxelEngine.Level.Light {

    public class LightLookupRegion {

        private int[] lookup;

        public LightLookupRegion(World world, int xOrgin, int yOrgin, int zOrgin) {
            /*
            this.chunks = new Chunk[27];
            for(int x = 0; x <= 2; x++) {
                for (int y = 0; y <= 2; y++) {
                    for (int z = 0; z <= 2; z++) {
                        this.chunks[(y * Chunk.SIZE * Chunk.SIZE) + (z * Chunk.SIZE) + x] = this.getChunk(
                            xOrgin + (x - 1) * Chunk.SIZE,
                            yOrgin + (y - 1) * Chunk.SIZE,
                            zOrgin + (z - 1) * Chunk.SIZE);
                    }
                }
            }
            */

            this.lookup = new int[24389];
        }
    }
}
