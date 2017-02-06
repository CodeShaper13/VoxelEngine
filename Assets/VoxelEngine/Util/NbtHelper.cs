using fNbt;
using UnityEngine;

namespace VoxelEngine.Util {

    public static class NbtHelper {

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

        public static NbtCompound writeBlockPos(string tagName, BlockPos pos) {
            NbtCompound tag = new NbtCompound(tagName);
            tag.Add(new NbtInt("x", pos.x));
            tag.Add(new NbtInt("y", pos.y));
            tag.Add(new NbtInt("z", pos.z));
            return tag;
        }

        public static BlockPos readBlockPos(NbtCompound tag) {
            return new BlockPos(
                tag.Get<NbtInt>("x").IntValue,
                tag.Get<NbtInt>("y").IntValue,
                tag.Get<NbtInt>("z").IntValue);
        }

        public static NbtCompound writeDirectBlockPos(NbtCompound tag, BlockPos pos, string prefix) {
            tag.Add(new NbtInt(prefix + "X", pos.x));
            tag.Add(new NbtInt(prefix + "Y", pos.y));
            tag.Add(new NbtInt(prefix + "Z", pos.z));
            return tag;
        }

        public static BlockPos readDirectBlockPos(NbtCompound tag, string prefix) {
            return new BlockPos(tag.Get<NbtInt>(prefix + "X").IntValue, tag.Get<NbtInt>(prefix + "Y").IntValue, tag.Get<NbtInt>(prefix + "Z").IntValue);
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
    }
}
