using fNbt;
using UnityEngine;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public abstract class WorldGeneratorBase {
        public World world;
        public int seed;

        public WorldGeneratorBase(World world, int seed) {
            this.world = world;
            this.seed = seed;
        }

        public abstract void generateChunk(Chunk chunk);

        public abstract ChunkLoaderBase getChunkLoader(EntityPlayer player);

        public virtual Vector3 getSpawnPoint(World world) {
            return Vector3.zero;
        }

        /// <summary>
        /// Returns true if extra data was generated, that way we can save the world.
        /// </summary>
        public virtual bool generateLevelData() {
            return false;
        }

        public virtual void populateChunk(Chunk chunk) {

        }

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) { }

        /// <summary>
        /// Sets the fog distance for this world type.
        /// </summary>
        public void setFogDistance(float distance) {
            RenderSettings.fogDensity = distance;
        }
    }
}
