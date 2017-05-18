using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Util;
using VoxelEngine.Generation.Caves.Structure.Mineshaft.Center;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class StructureMineshaft : StructureBase, IDebugDisplayable {

        public const int SIZE_CAP = 20;

        public List<PieceBase> pieces;
        public System.Random rnd;

        private BlockPos shaftOrgin;
        

        public StructureMineshaft() {
            this.pieces = new List<PieceBase>();
        }

        public StructureMineshaft(Vector3 shaftOrgin, int seed) : this() {
            this.shaftOrgin = new BlockPos((int)shaftOrgin.x, (int)shaftOrgin.y, (int)shaftOrgin.z);
            this.rnd = new System.Random(seed);

            // Make the starting piece
            new PieceCenter(this, this.shaftOrgin);
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            NbtHelper.writeDirectBlockPos(tag, this.shaftOrgin, "shaftOrgin");

            NbtList list = new NbtList("pieces", NbtTagType.Compound);
            foreach(PieceBase p in this.pieces) {
                list.Add(p.writeToNbt(new NbtCompound()));
            }
            tag.Add(list);

            return base.writeToNbt(tag);
        }

        public override void readFromNbt(NbtCompound tag) {
            this.shaftOrgin = NbtHelper.readDirectBlockPos(tag, "shaftOrgin");

            foreach (NbtCompound compound in tag.Get<NbtList>("pieces")) {
                byte id = compound.Get<NbtByte>("id").ByteValue;
                PieceBase p = null;
                switch(id) {
                    case 0:
                        p = new PieceCenter(compound);
                        break;
                    case 1:
                        p = new PieceCrossing(compound);
                        break;
                    case 2:
                        p = new PieceHallway(compound);
                        break;
                    case 3:
                        p = new PieceRoom(compound);
                        break;
                    case 4:
                        p = new PieceShaft(compound);
                        break;
                    case 7:
                        p = new PieceSmallShaft(compound);
                        break;
                    case 8:
                        p = new PieceBedroom(compound);
                        break;
                    case 9:
                        p = new PieceSmallStoreRoom(compound);
                        break;
                }

                if(p != null) {
                    p.shaft = this;
                    p.calculateBounds();
                    this.pieces.Add(p);
                }
            }
        }

        public override string getStructureName() {
            return "mineshaft";
        }

        public void debugDisplay() {
            for(int i = 0; i < this.pieces.Count; i++) {
                this.pieces[i].debugDisplay();
            }
        }
    }
}