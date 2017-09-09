using fNbt;
using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public abstract class PieceOrginBase : PieceBase {

        protected bool right;
        protected bool left;
        /// <summary> 0 = stone, 1 = planks </summary>
        protected int floorType;

        public PieceOrginBase(NbtCompound tag) : base(tag) {
            this.right = tag.Get<NbtByte>("right").ByteValue == 1 ? true : false;
            this.left = tag.Get<NbtByte>("left").ByteValue == 1 ? true : false;
            this.floorType = tag.Get<NbtInt>("floorType").IntValue;
        }

        public PieceOrginBase(StructureMineshaft shaft, BlockPos shaftCenter) : base(shaft, new BlockPos(shaftCenter.x + 4, shaftCenter.y, shaftCenter.z)) {
            int i = 2; // this.shaft.rnd.Next(0, 5);
            if (i < 2) { // 0, 1
                this.right = true;
            }
            else if (i == 2) { // 2
                this.right = true;
                this.left = true;
            }
            else if (i > 2) { // 3, 4
                this.left = true;
            }

            this.floorType = this.shaft.rnd.Next(0, 4) == 0 ? 1 : 0;
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtByte("right", this.right ? (byte)1 : (byte)0));
            tag.Add(new NbtByte("left", this.left ? (byte)1 : (byte)0));
            tag.Add(new NbtInt("floorType", this.floorType));
            return tag;
        }

        public override Color getPieceColor() {
            return Color.cyan;
        }
    }
}
