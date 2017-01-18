using System.Collections.Generic;
using VoxelEngine.Generation.CellularAutomaton;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class WorldType {
        public static List<WorldType> typeList = new List<WorldType>();

        public static WorldType HILLS = new WorldType("Hills", 0);
        public static WorldType FLAT = new WorldType("Flat", 0);
        public static WorldType CAVE_1 = new WorldType("Caves", 1);
        public static WorldType CAVE_2 = new WorldType("Cellular Automata", 2);

        public int id;
        public string name;
        public int chunkLoaderType;
        public WorldType generatorType;

        public WorldType(string name, int chunkLoaderType) {
            this.id = WorldType.typeList.Count;
            this.name = name;
            this.chunkLoaderType = chunkLoaderType;
            WorldType.typeList.Add(this);
        }

        public WorldGeneratorBase getGenerator(World world, long seed) {
            if(this == WorldType.HILLS) {
                return new WorldGeneratorHills(world, seed);
            } else if(this == WorldType.FLAT) {
                return new WorldGeneratorFlat(world, seed);
            } else if(this == WorldType.CAVE_1) {
                return new WorldGeneratorCaves(world, seed);
            } else if(this == WorldType.CAVE_2) {
                return new WorldGeneratorCellularAutomaton(world, seed);
            }
            return new WorldGeneratorFlat(world, seed);
        }

        public static WorldType getFromId(int worldType) {
            return WorldType.typeList[worldType];
        }
    }
}
