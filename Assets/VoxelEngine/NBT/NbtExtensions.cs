using fNbt;

namespace VoxelEngine.NBT {

    public static class NbtExtensions {

        public static float getFloat(this NbtCompound tag, string tagName, float defaultValue = 0) {
            NbtFloat t;
            if(tag.TryGet<NbtFloat>(tagName, out t)) {
                return t.Value;
            } else {
                return defaultValue;
            }
        }
    }
}
