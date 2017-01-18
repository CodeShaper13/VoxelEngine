using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public abstract class WorldGeneratorBase {
        public World world;
        public long seed;

        public WorldGeneratorBase(World world, long seed) {
            this.world = world;
            this.seed = seed;
        }

        public virtual Vector3 getSpawnPoint() {
            return Vector3.zero;
        }

        public abstract void generateChunk(Chunk chunk);

        public virtual void populateChunk(Chunk chunk) {
            chunk.isPopulated = true;
        }
    }
}
