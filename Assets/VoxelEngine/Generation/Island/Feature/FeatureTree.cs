using System;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Island.Feature {

    public abstract class FeatureTree : IFeature {

        public void generate(Chunk chunk, Random rnd) {
            CachedChunk3x3 cc3x3 = CachedChunk3x3.getNewRegion(chunk.world, chunk);

            int x, z;
            Block groundBlock;
            BlockPos pos, pos1;
            for (int i = 0; i < 3; i++) {
                for(int j =  0; j < 3; j++) {
                    x = i * 5 + rnd.Next(0, 4);
                    z = j * 5 + rnd.Next(0, 4);
                    if(rnd.Next(0, 2) != 0) {
                        for(int y = 0; y < Chunk.SIZE; y++) {
                            pos = new BlockPos(x, y, z);
                            pos1 = pos.move(Direction.DOWN);
                            groundBlock = cc3x3.getBlock(pos1.x, pos1.y, pos1.z);
                            if (cc3x3.getBlock(pos.x, pos.y, pos.z) == Block.air && (groundBlock == Block.dirt || groundBlock == Block.grass)) {
                                this.makeTree(cc3x3, rnd, pos);
                            }
                        }
                    }
                }
            }
        }

        protected bool isReplacable(Block block) {
            return block == Block.air || block == Block.wood || block == Block.leaves;
        }

        protected abstract void makeTree(CachedChunk3x3 region, Random rnd, BlockPos pos);
    }
}
