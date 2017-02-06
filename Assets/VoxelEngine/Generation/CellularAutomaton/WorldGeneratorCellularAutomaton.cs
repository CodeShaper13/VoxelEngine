using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;

namespace VoxelEngine.Generation.CellularAutomaton {

    public class WorldGeneratorCellularAutomaton : WorldGeneratorBase {

        public GeneratorOptions options;
        private CaveGenerator caveGenerator;

        public WorldGeneratorCellularAutomaton(World world, int seed) : base(world, seed) {
            this.options = GeneratorOptions.DEFAULT_OPTIONS;

            this.caveGenerator = new CaveGenerator();
            this.caveGenerator.generateMap(false);
        }

        public override Vector3 getSpawnPoint() {
            return new Vector3(8, 25, 13);
        }

        public override void generateChunk(Chunk chunk) {
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        int x1 = chunk.pos.x + x;
                        int y1 = chunk.pos.y + y;
                        int z1 = chunk.pos.z + z;
                        chunk.setBlock(x, y, z, this.caveGenerator.map[x1][y1][z1] == 1 ? Block.stone : Block.air);                            
                    }
                }
            }
        }
    }
}
