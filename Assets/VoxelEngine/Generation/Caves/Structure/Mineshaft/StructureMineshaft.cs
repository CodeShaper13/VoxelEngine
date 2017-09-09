using System.Collections.Generic;
using fNbt;
using VoxelEngine.Util;
using VoxelEngine.Generation.Caves.Structure.Mineshaft.Center;
using UnityEngine;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class StructureMineshaft : StructureBase, IDebugDisplayable {

        public const int SIZE_CAP = 15;

        public List<PieceBase> pieces;
        private BlockPos shaftOrgin;        

        public StructureMineshaft() {
            this.pieces = new List<PieceBase>();
        }

        public StructureMineshaft(BlockPos shaftOrgin, System.Random rnd) : base(rnd) {
            this.pieces = new List<PieceBase>();

            this.shaftOrgin = shaftOrgin;

            // Make the starting piece
            new PieceCenter(this, this.shaftOrgin);

            this.calculateStructureBounds();
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            NbtHelper.writeDirectBlockPos(tag, this.shaftOrgin, "orgin");

            NbtList nbtList = new NbtList("pieces", NbtTagType.Compound);
            foreach(PieceBase p in this.pieces) {
                nbtList.Add(p.writeToNbt(new NbtCompound()));
            }
            tag.Add(nbtList);

            return base.writeToNbt(tag);
        }

        public override void readFromNbt(NbtCompound tag) {
            this.shaftOrgin = NbtHelper.readDirectBlockPos(tag, "orgin");

            foreach (NbtCompound compound in tag.Get<NbtList>("pieces")) {
                byte id = compound.Get<NbtByte>("id").ByteValue;
                PieceBase p = this.getPieceFromId(id, compound);

                if(p != null) {
                    p.shaft = this;
                    p.calculateBounds();
                    this.pieces.Add(p);
                }
            }
        }

        public override void calculateStructureBounds() {
            this.structureBoundingBox = new Bounds(
                this.pieces[0].pieceBounds.center,
                this.pieces[0].pieceBounds.size);

            for (int i = 1; i < this.pieces.Count; i++) {
                this.structureBoundingBox.Encapsulate(this.pieces[i].pieceBounds);                
            }
        }

        public new void debugDisplay() {
            base.debugDisplay();
            for (int i = 0; i < this.pieces.Count; i++) {
                this.pieces[i].debugDisplay();
            }
        }

        private PieceBase getPieceFromId(int id, NbtCompound compound) {
            switch (id) {
                case 0:
                    return new PieceCenter(compound);
                case 1:
                    return new PieceCrossing(compound);
                case 2:
                    return new PieceHallway(compound);
                case 3:
                    return new PieceRoom(compound);
                case 4:
                    return new PieceShaft(compound);
                case 5:
                    return new PieceMobSpawner(compound);
                case 7:
                    return new PieceSmallShaft(compound);
                case 8:
                    return new PieceOrginBedroom(compound);
                case 9:
                    return new PieceOrginStorage(compound);
                default:
                    return null;
            }
        }
    }
}