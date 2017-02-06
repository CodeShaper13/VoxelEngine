using System.Collections.Generic;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Generation.Caves;
using VoxelEngine.Generation.CellularAutomaton;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class WorldType {
        public static List<WorldType> typeList = new List<WorldType>();

        public static WorldType HILLS = new WorldType("Hills", ChunkLoaderBase.LOCKED_Y);
        public static WorldType FLAT = new WorldType("Flat", ChunkLoaderBase.LOCKED_Y);
        public static WorldType CAVE_1 = new WorldType("Caves", ChunkLoaderBase.INFINITE);
        public static WorldType CAVE_2 = new WorldType("Cellular Automata", ChunkLoaderBase.REGION_DEBUG);

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

        public WorldGeneratorBase getGenerator(World world, int seed) {
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
