using fNbt;
using System;

namespace VoxelEngine.Level {

    public class WorldData {
        public string worldName;
        public long seed;
        public float spawnX;
        public float spawnY;
        public float spawnZ;
        public int worldType;

        public WorldData(string worldName) {
            this.worldName = worldName;
        }

        public WorldData(string worldName, long seed) : this(worldName) {
            this.seed = DateTime.Today.ToBinary();
        }

        public NbtCompound writeToNbt() {
            NbtCompound tag = new NbtCompound("world");
            tag.Add(new NbtLong("seed", this.seed));
            tag.Add(new NbtFloat("spawnX", this.spawnX));
            tag.Add(new NbtFloat("spawnY", this.spawnY));
            tag.Add(new NbtFloat("spawnZ", this.spawnZ));
            tag.Add(new NbtInt("worldType", this.worldType));
            return tag;
        }

        public void readFromNbt(NbtCompound tag) {
            this.seed = tag.Get<NbtLong>("seed").LongValue;
            this.spawnX = tag.Get<NbtFloat>("spawnX").FloatValue;
            this.spawnY = tag.Get<NbtFloat>("spawnY").FloatValue;
            this.spawnZ = tag.Get<NbtFloat>("spawnZ").FloatValue;
            this.worldType = tag.Get<NbtInt>("worldType").IntValue;
        }
    }
}
