using System;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Island.Feature {

    public class FeatureTreeBasic : FeatureTree {

        protected override void makeTree(World world, Random rnd, BlockPos pos) {

            int height = pos.y + 4 + rnd.Next(3);

            bool flag = true;

            // Make sure the area is clear.
            for(int x = pos.x - 2; x <= pos.x + 2; x++) {
                for (int z = pos.z - 2; z <= pos.z + 2; z++) {
                    for(int y = pos.y; y < height; y++) {
                        if(!(this.func(world.getBlock(x, y, z)))) {
                            flag = false;
                        }
                    }
                }
            }

            if (flag) {
                // Generate tree
                for(int y = pos.y; y <= height; y++) {
                    if(y == height - 3 || y == height - 2) {
                        for (int x = pos.x - 2; x <= pos.x + 2; x++) {
                            for (int z = pos.z - 2; z <= pos.z + 2; z++) {
                                if(Math.Abs(x) == 2 && Math.Abs(z) == 2 && rnd.Next(2) == 0) {
                                    continue;
                                }
                                world.setBlock(x, y, z, Block.leaves, 0, false, false);
                            }
                        }
                    } else if(y == height - 1) {
                        for (int x = pos.x - 1; x <= pos.x + 1; x++) {
                            for (int z = pos.z - 1; z <= pos.z + 1; z++) {
                                world.setBlock(x, y, z, Block.leaves, 0, false, false);
                            }
                        }
                    } else if(y == height) {
                        world.setBlock(pos.x + 1, y, pos.z, Block.leaves, 0, false, false);
                        world.setBlock(pos.x - 1, y, pos.z, Block.leaves, 0, false, false);
                        world.setBlock(pos.x, y, pos.z + 1, Block.leaves, 0, false, false);
                        world.setBlock(pos.x, y, pos.z - 1, Block.leaves, 0, false, false);
                        world.setBlock(pos.x, y, pos.z, Block.leaves, 0, false, false);
                    }

                    if (y != height) {
                        world.setBlock(pos.x, y, pos.z, Block.wood, 1);
                    }
                }
            }            
        }
    }
}
