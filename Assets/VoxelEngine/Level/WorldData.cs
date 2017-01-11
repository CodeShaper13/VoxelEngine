using fNbt;
using System;

namespace VoxelEngine.Level {

    public class WorldData {
        public string worldName;
        public int seed;
        public float spawnX;
        public float spawnY;
        public float spawnZ;
        public int worldType;
        public DateTime lastLoaded;
        public StoneLayers stoneLayers;
        public bool dontWriteToDisk;

        public WorldData(string worldName) {
            this.worldName = worldName;
            this.stoneLayers = new StoneLayers();
        }

        public WorldData(string worldName, int seed, bool dontWriteToDisk) : this(worldName) {
            this.seed = (int)DateTime.Today.ToBinary();
            this.lastLoaded = DateTime.Now;
            this.dontWriteToDisk = dontWriteToDisk;
        }

        public NbtCompound writeToNbt() {
            NbtCompound tag = new NbtCompound("world");
            tag.Add(new NbtInt("seed", this.seed));
            tag.Add(new NbtFloat("spawnX", this.spawnX));
            tag.Add(new NbtFloat("spawnY", this.spawnY));
            tag.Add(new NbtFloat("spawnZ", this.spawnZ));
            tag.Add(new NbtInt("worldType", this.worldType));
            tag.Add(new NbtLong("lastLoaded", this.lastLoaded.ToBinary()));
            tag.Add(this.stoneLayers.writeToNbt(new NbtCompound("stoneLayers")));
            return tag;
        }

        public void readFromNbt(NbtCompound tag) {
            this.seed = tag.Get<NbtInt>("seed").IntValue;
            this.spawnX = tag.Get<NbtFloat>("spawnX").FloatValue;
            this.spawnY = tag.Get<NbtFloat>("spawnY").FloatValue;
            this.spawnZ = tag.Get<NbtFloat>("spawnZ").FloatValue;
            this.worldType = tag.Get<NbtInt>("worldType").IntValue;
            this.lastLoaded = DateTime.FromBinary(tag.Get<NbtLong>("lastLoaded").LongValue);
            this.stoneLayers.readFromNbt(tag.Get<NbtCompound>("stoneLayers"));
        }
    }
}
