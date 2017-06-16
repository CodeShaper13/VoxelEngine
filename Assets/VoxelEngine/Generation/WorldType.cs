using System;
using System.Collections.Generic;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Entities;
using VoxelEngine.Generation.Caves;
using VoxelEngine.Generation.Island;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class WorldType {
        public static List<WorldType> typeList = new List<WorldType>();

        public static WorldType HILLS = new WorldType("Hills");
        public static WorldType FLAT = new WorldType("Flat");
        public static WorldType CAVE = new WorldType("Caves");
        public static WorldType ISLAND = new WorldType("Island");

        public int id;
        public string name;

        public WorldType(string name) {
            this.id = WorldType.typeList.Count;
            this.name = name;
            WorldType.typeList.Add(this);
        }

        public WorldGeneratorBase getGenerator(World world, int seed) {
            switch(this.id) {
                case 0:
                    return new WorldGeneratorHills(world, seed);
                case 1:
                    return new WorldGeneratorFlat(world, seed);
                case 2:
                    return new WorldGeneratorCaves(world, seed);
                case 3:
                    return new WorldGeneratorIsland(world, seed);
            }
            return null;
        }

        public static WorldType getFromId(int worldType) {
            return WorldType.typeList[worldType];
        }
    }
}
