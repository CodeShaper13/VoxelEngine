using VoxelEngine.Blocks;

namespace Assets.VoxelEngine.Generation.Island.Biome {

    public class BiomeBase {

        private static BiomeBase[] biomes;

        public int id;
        public Block topBlock;
        public Block fillerBlock;

        public BiomeBase(int id, Block topBlock, Block fillerBlock) {
            this.id = id;
            this.topBlock = topBlock;
            this.fillerBlock = fillerBlock;

            BiomeBase.biomes[this.id] = this;
        }

        public static BiomeBase getBiomeFromId(int id) {
            return BiomeBase.biomes[id];
        }
    }
}
