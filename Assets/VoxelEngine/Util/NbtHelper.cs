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

        /// <summary>
        /// Reads a Vector3 compound from the passed tag.
        /// </summary>
        public static Vector3 readVector3(NbtCompound tag) {
            return new Vector3(
                tag.Get<NbtFloat>("x").FloatValue,
                tag.Get<NbtFloat>("y").FloatValue,
                tag.Get<NbtFloat>("z").FloatValue);
        }

        /// <summary>
        /// Writes the passed Vector3 to the passed tag, appending "x", "y" and "z" to the prefix to get the tag name.
        /// </summary>
        public static NbtCompound writeDirectVector3(NbtCompound tag, Vector3 vec, string prefix) {
            tag.Add(new NbtFloat(prefix + "x", vec.x));
            tag.Add(new NbtFloat(prefix + "y", vec.y));
            tag.Add(new NbtFloat(prefix + "z", vec.z));
            return tag;
        }

        public static Vector3 readDirectVector3(NbtCompound tag, string prefix) {
            return new Vector3(
                tag.Get<NbtFloat>(prefix + "x").FloatValue,
                tag.Get<NbtFloat>(prefix + "y").FloatValue,
                tag.Get<NbtFloat>(prefix + "z").FloatValue);
        }

        /// <summary>
        /// Writes the passed BlockPos as a compound with passed name.
        /// </summary>
        public static NbtCompound writeBlockPos(string tagName, BlockPos pos) {
            NbtCompound tag = new NbtCompound(tagName);
            tag.Add(new NbtInt("x", pos.x));
            tag.Add(new NbtInt("y", pos.y));
            tag.Add(new NbtInt("z", pos.z));
            return tag;
        }

        /// <summary>
        /// Reads a BlockPos compound from the passed tag.
        /// </summary>
        public static BlockPos readBlockPos(NbtCompound tag, string compoundName) {
            NbtCompound tag1 = tag.Get<NbtCompound>(compoundName);
            return new BlockPos(
                tag1.Get<NbtInt>("x").IntValue,
                tag1.Get<NbtInt>("y").IntValue,
                tag1.Get<NbtInt>("z").IntValue);
        }

        /// <summary>
        /// Writes the passed BlockPos to the passed tag, appending "x", "y" and "z" to the prefix to get the tag name.
        /// </summary>
        public static NbtCompound writeDirectBlockPos(NbtCompound tag, BlockPos pos, string prefix) {
            tag.Add(new NbtInt(prefix + "x", pos.x));
            tag.Add(new NbtInt(prefix + "y", pos.y));
            tag.Add(new NbtInt(prefix + "z", pos.z));
            return tag;
        }

        public static BlockPos readDirectBlockPos(NbtCompound tag, string prefix) {
            return new BlockPos(
                tag.Get<NbtInt>(prefix + "x").IntValue,
                tag.Get<NbtInt>(prefix + "y").IntValue,
                tag.Get<NbtInt>(prefix + "z").IntValue);
        }
    }
}
