using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class WorldGeneratorFlat : WorldGeneratorBase {

        public WorldGeneratorFlat(World world, long seed) : base(world, seed) {}

        public override Vector3 getSpawnPoint() {
            return new Vector3(0, 21, 0);
        }

        public override void generateChunk(Chunk chunk) {
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        chunk.setBlock(x, y, z, this.getBlockForHeight(y + chunk.pos.y));
                    }
                }
            }
        }

        private Block getBlockForHeight(int y) {
            if (y < 16) {
                return Block.stone;
            }
            else if (y < 18) {
                return Block.dirt;
            }
            else if (y < 19) {
                return Block.grass;
            }
            else {
                return Block.air;
            }
        }
    }
}
