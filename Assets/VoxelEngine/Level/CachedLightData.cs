using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace Assets.VoxelEngine.Level {

    public class CachedLightData {

        public CellLightData[,,] data;
        public int diameter;
        private World world;
        private BlockPos orgin;

        public CachedLightData(World world, BlockPos orgin, int size) {
            this.diameter = size + size + 1;
            this.data = new CellLightData[this.diameter, this.diameter, this.diameter];
            this.world = world;
            this.orgin = orgin;
            
            // Find edges
            this.func_01(size, size, size, size);

            // Calculate region light from outside sources
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    for (int z = 0; z < size; z++) {
                        if(this.data[x, y, z].state == 2) { // Edge
                            //this.func_02(x, y, z);
                        }
                    }
                }
            }

            // Add light from lights within region
        }

        private void func_01(int x, int y, int z, int i) {
            if(x < 0 || y < 0 || z < 0 || x > this.diameter || y > this.diameter || z > this.diameter) {
                return;
            }
            if(this.data[x, y, z].state == 1) {
                return;
            }

            i--;

            this.data[x, y, z].state = (i <= 1 ? 2 : 1);

            this.func_01(x + 1, y, z, i);
            this.func_01(x - 1, y, z, i);
            this.func_01(x, y + 1, z, i);
            this.func_01(x, y - 1, z, i);
            this.func_01(x, y, z + 1, i);
            this.func_01(x, y, z - 1, i);
        }

        private void func_02(int x, int y, int z, int lightLevel) {
            // Return if the new light is less than 0.
            if (lightLevel <= 0) {
                return;
            }

            // Return if the block is solid, it can't have light and light can't spread through it.
            if (this.getBlock(x, y, z).isSolid) {
                return;
            }

            /*
            int oldLight = chunk.getLight(localizedX, localizedY, localizedZ);

            if (oldLight >= lightLevel) {
                return; // The block's light is greater than the new one, we shouldn't change it or keep spreading
            } else {
                this.data[x, y, z].light = lightLevel;
            }
            */

            lightLevel -= 1;
            this.func_02(x + 1, y, z, lightLevel);
            this.func_02(x - 1, y, z, lightLevel);
            this.func_02(x, y + 1, z, lightLevel);
            this.func_02(x, y - 1, z, lightLevel);
            this.func_02(x, y, z + 1, lightLevel);
            this.func_02(x, y, z - 1, lightLevel);
        }

        private Block getBlock(int x, int y, int z) {
            return null;
        }
    }
}
