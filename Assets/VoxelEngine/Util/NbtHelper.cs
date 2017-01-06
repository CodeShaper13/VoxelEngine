using fNbt;
using UnityEngine;

namespace VoxelEngine.Util {

    public class NbtHelper {

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
    }
}
