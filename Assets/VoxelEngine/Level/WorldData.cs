using fNbt;
using System;
using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Level {

    public class WorldData {
        public string worldName;
        public int seed;
        public Vector3 spawnPos;
        public int worldType;
        public DateTime lastLoaded;
        /// <summary> If true, the world will not be writen to the disk.  Used in debugging. </summary>
        public bool writeToDisk;

        public WorldData(string worldName) {
            this.worldName = worldName;
        }

        public WorldData(string worldName, int seed, int worldType, bool writeToDisk) : this(worldName) {
            this.seed = (int)DateTime.Today.ToBinary();
            this.worldType = worldType;
            this.lastLoaded = DateTime.Now;
            this.writeToDisk = writeToDisk;
        }

        public NbtCompound writeToNbt() {
            NbtCompound tag = new NbtCompound("world");
            tag.Add(new NbtInt("seed", this.seed));
            NbtHelper.writeDirectVector3(tag, this.spawnPos, "spawn");
            tag.Add(new NbtInt("worldType", this.worldType));
            tag.Add(new NbtLong("lastLoaded", this.lastLoaded.ToBinary()));
            return tag;
        }

        public void readFromNbt(NbtCompound tag) {
            this.seed = tag.Get<NbtInt>("seed").IntValue;
            this.spawnPos = NbtHelper.readDirectVector3(tag, "spawn");
            this.worldType = tag.Get<NbtInt>("worldType").IntValue;
            this.lastLoaded = DateTime.FromBinary(tag.Get<NbtLong>("lastLoaded").LongValue);
        }
    }
}
