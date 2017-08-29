using fNbt;

namespace VoxelEngine.Generation.Caves.Structure {

    public abstract class StructureBase {

        public abstract string getStructureName();

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) { }
    }
}
