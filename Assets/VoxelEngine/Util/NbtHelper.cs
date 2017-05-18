using fNbt;
using UnityEngine;

namespace VoxelEngine.Util {

    /// <summary>
    /// Helper methods for dealing with the reading and writing of NBT.
    /// </summary>
    public static class NbtHelper {

        /// <summary>
        /// Writes the passed Vector3 as a compound with passed name.
        /// </summary>
        public static NbtCompound writeVector3(string tagName, Vector3 vec) {
            NbtCompound tag = new NbtCompound(tagName);
            tag.Add(new NbtFloat("x", vec.x));
            tag.Add(new NbtFloat("y", vec.y));
            tag.Add(new NbtFloat("z", vec.z));
            return tag;
        }

        public static Vector3 readVector3(NbtCompound tag) {
            return new Vector3(
                tag.Get<NbtFloat>("x").FloatValue,
                tag.Get<NbtFloat>("y").FloatValue,
                tag.Get<NbtFloat>("z").FloatValue);
        }

        public static NbtCompound writeDirectVector3(NbtCompound tag, Vector3 vec, string prefix) {
            tag.Add(new NbtFloat(prefix + "X", vec.x));
            tag.Add(new NbtFloat(prefix + "Y", vec.y));
            tag.Add(new NbtFloat(prefix + "Z", vec.z));
            return tag;
        }

        public static Vector3 readDirectVector3(NbtCompound tag, string prefix) {
            return new Vector3(tag.Get<NbtFloat>(prefix + "X").FloatValue, tag.Get<NbtFloat>(prefix + "Y").FloatValue, tag.Get<NbtFloat>(prefix + "Z").FloatValue);
        }

        public static NbtCompound writeBlockPos(string tagName, BlockPos pos) {
            NbtCompound tag = new NbtCompound(tagName);
            tag.Add(new NbtInt("x", pos.x));
            tag.Add(new NbtInt("y", pos.y));
            tag.Add(new NbtInt("z", pos.z));
            return tag;
        }

        public static BlockPos readBlockPos(NbtCompound tag, string compoundName) {
            NbtCompound tag1 = tag.Get<NbtCompound>(compoundName);
            return new BlockPos(
                tag1.Get<NbtInt>("x").IntValue,
                tag1.Get<NbtInt>("y").IntValue,
                tag1.Get<NbtInt>("z").IntValue);
        }

        /// <summary>
        /// Writes the passed BlockPos to the passed tag, appending "X", "Y" and "Z" to the prefix to get the tag name.
        /// </summary>
        public static NbtCompound writeDirectBlockPos(NbtCompound tag, BlockPos pos, string prefix) {
            tag.Add(new NbtInt(prefix + "X", pos.x));
            tag.Add(new NbtInt(prefix + "Y", pos.y));
            tag.Add(new NbtInt(prefix + "Z", pos.z));
            return tag;
        }

        public static BlockPos readDirectBlockPos(NbtCompound tag, string prefix) {
            return new BlockPos(tag.Get<NbtInt>(prefix + "X").IntValue, tag.Get<NbtInt>(prefix + "Y").IntValue, tag.Get<NbtInt>(prefix + "Z").IntValue);
        }
    }
}
