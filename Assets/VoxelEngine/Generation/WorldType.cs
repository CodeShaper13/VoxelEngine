using System.Collections.Generic;
using VoxelEngine.Generation.Caves;
using VoxelEngine.Generation.Island;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class WorldType {

        public static List<WorldType> typeList = new List<WorldType>();

        public static readonly WorldType CAVE = new WorldType(0, "Caves");
        public static readonly WorldType FLAT = new WorldType(1, "Flat");
        public static readonly WorldType HILLS = new WorldType(2, "Hills").setHidden();
        public static readonly WorldType ISLAND = new WorldType(3, "Island").setHidden();

        public int id;
        public string name;
        public bool isHidden;

        private WorldType(int id, string name) {
            this.id = id;
            this.name = name;
            WorldType.typeList.Add(this);
        }

        public WorldGeneratorBase getGenerator(World world, int seed) {
            switch(this.id) {
                case 0:
                    return new WorldGeneratorCaves(world, seed);
                case 1:
                    return new WorldGeneratorFlat(world, seed);
                case 2:
                    return new WorldGeneratorHills(world, seed);
                case 3:
                    return new WorldGeneratorIsland(world, seed);
            }
            return null;
        }

        public WorldType setHidden() {
            this.isHidden = true;
            return this;
        }

        public static WorldType getFromId(int worldType) {
            return WorldType.typeList[worldType];
        }
    }
}
