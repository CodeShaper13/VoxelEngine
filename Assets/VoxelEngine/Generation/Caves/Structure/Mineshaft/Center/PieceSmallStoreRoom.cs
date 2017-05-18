using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public class PieceSmallStoreRoom : PieceBase {

        private int unitsLong;

        public PieceSmallStoreRoom(NbtCompound tag) : base(tag) {
            this.unitsLong = tag.Get<NbtInt>("unitsLong").IntValue;
        }

        public PieceSmallStoreRoom(StructureMineshaft shaft, BlockPos shaftCenter, System.Random rnd) : base(shaft, new BlockPos(shaftCenter.x + 4, shaftCenter.y, shaftCenter.z)) {

        }

        public override void calculateBounds() {
            
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("unitsLong", this.unitsLong));
            return tag;
        }

        public override Color getPieceColor() {
            return Color.green;
        }

        public override byte getPieceId() {
            return 9;
        }
    }
}
