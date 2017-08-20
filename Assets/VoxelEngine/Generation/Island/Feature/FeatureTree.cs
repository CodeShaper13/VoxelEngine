using System;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Island.Feature {

    public abstract class FeatureTree : IFeature {

        public void generate(Chunk chunk, Random rnd) {
            int x, z;
            Block groundBlock;
            for(int i = 0; i < 3; i++) {
                for(int j =  0; j < 4; j++) {
                    x = i * 4 + rnd.Next(0, 4);
                    z = j * 4 + rnd.Next(0, 4);
                    if(rnd.Next(0, 2) != 0) {
                        for(int y = 0; y < Chunk.SIZE; y++) {
                            BlockPos pos = new BlockPos(x, y, z) + chunk.worldPos;
                            groundBlock = chunk.world.getBlock(pos.move(Direction.DOWN));
                            if (chunk.world.getBlock(pos) == Block.air && (groundBlock == Block.dirt || groundBlock == Block.grass)) {
                                this.makeTree(chunk.world, rnd, pos);
                            }
                        }
                    }
                }
            }
        }

        protected bool func(Block block) {
            return block == Block.air || block == Block.wood || block == Block.leaves;
        }

        protected abstract void makeTree(World world, Random rnd, BlockPos pos);
    }
}
