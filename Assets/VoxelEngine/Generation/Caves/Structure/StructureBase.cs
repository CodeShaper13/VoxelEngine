using fNbt;
using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure {

    public abstract class StructureBase : IDebugDisplayable {

        /// <summary>
        /// Note!  Don't use this for realtime generation, only the outline data as it will
        /// be null if the structure was loaded from disk.
        /// </summary>
        public System.Random rnd;
        public Bounds structureBoundingBox;

        /// <summary>
        /// Called for structures that are loaded from disk.
        /// </summary>
        public StructureBase() { }

        /// <summary>
        /// Called on newly generated structures.
        /// </summary>
        public StructureBase(System.Random rnd) {
            this.rnd = rnd;
        }

        public abstract void calculateStructureBounds();

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            NbtHelper.writeDirectVector3(tag, this.structureBoundingBox.center, "sbbc");
            NbtHelper.writeDirectVector3(tag, this.structureBoundingBox.size, "sbbs");

            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {
            this.structureBoundingBox = new Bounds(
                NbtHelper.readDirectVector3(tag, "sbbc"),
                NbtHelper.readDirectVector3(tag, "sbbs"));
        }

        /// <summary>
        /// Make sure you call the base method!
        /// </summary>
        public void debugDisplay() {
            DebugDrawer.bounds(this.structureBoundingBox, Color.white);
        }
    }
}
