using System;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Island.Feature {

    public class FeatureTreeBasic : FeatureTree {

        protected override void makeTree(CachedChunk3x3 region, Random rnd, BlockPos pos) {

            int height = pos.y + 4 + rnd.Next(3);

            // Make sure the area is clear.
            /*
            for (int y = pos.y; y < height; y++) {
                int i = y <= 2 ? 0 : 2;
                for (int x = pos.x - i; x <= pos.x + i; x++) {
                    for (int z = pos.z - i; z <= pos.z + i; z++) {
                        if(!(this.isReplacable(world.getBlock(x, y, z)))) {
                            return;
                        }
                    }
                }
            }
            */

            // Generate tree
            for (int y = pos.y; y <= height; y++) {
                if (y == height - 3 || y == height - 2) {
                    for (int x = pos.x - 2; x <= pos.x + 2; x++) {
                        for (int z = pos.z - 2; z <= pos.z + 2; z++) {
                            if (Math.Abs(x) == 2 && Math.Abs(z) == 2 && rnd.Next(2) == 0) {
                                continue;
                            }
                            region.setBlock(x, y, z, Block.leaves);
                        }
                    }
                }
                else if (y == height - 1) {
                    for (int x = pos.x - 1; x <= pos.x + 1; x++) {
                        for (int z = pos.z - 1; z <= pos.z + 1; z++) {
                            region.setBlock(x, y, z, Block.leaves);
                        }
                    }
                }
                else if (y == height) {
                    region.setBlock(pos.x + 1, y, pos.z, Block.leaves);
                    region.setBlock(pos.x - 1, y, pos.z, Block.leaves);
                    region.setBlock(pos.x, y, pos.z + 1, Block.leaves);
                    region.setBlock(pos.x, y, pos.z - 1, Block.leaves);
                    region.setBlock(pos.x, y, pos.z, Block.leaves);
                }

                if (y != height) {
                    region.setBlock(pos.x, y, pos.z, Block.wood, 1);
                }
            }
        }
    }
}
