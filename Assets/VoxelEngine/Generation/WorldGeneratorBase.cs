using fNbt;
using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public abstract class WorldGeneratorBase {
        public World world;
        public int seed;

        public WorldGeneratorBase(World world, int seed) {
            this.world = world;
            this.seed = seed;
        }

        public virtual Vector3 getSpawnPoint() {
            return Vector3.zero;
        }

        // Returns true if extra data was generated, that way we can save
        public virtual bool generateLevelData() {
            return false;
        }

        public abstract void generateChunk(Chunk chunk);

        public virtual void populateChunk(Chunk chunk) {
            chunk.isPopulated = true;
        }

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {

        }
    }
}
