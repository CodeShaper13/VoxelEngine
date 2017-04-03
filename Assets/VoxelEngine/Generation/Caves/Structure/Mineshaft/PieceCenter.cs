using System;
using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Generation.Caves.Structure.Mineshaft.Center;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceCenter : PieceBase {

        // 0 = Storage, 1 = Bedroom
        private int topFloor;
        private int bottomFloor;
        private bool useFartherEntrance;

        public PieceCenter(NbtCompound tag) : base(tag) {
            this.topFloor = tag.Get<NbtInt>("topFloor").IntValue;
            this.bottomFloor = tag.Get<NbtInt>("bottomFloor").IntValue;
            this.useFartherEntrance = tag.Get<NbtInt>("ueas").ByteValue == 1;
        }

        public PieceCenter(BlockPos center, List<PieceBase> pieces, int piecesFromStart, System.Random rnd) : base(center) {            
            // Get random floors
            if(rnd.Next(2) == 1) {
                this.topFloor = 0;
                this.bottomFloor = 1;
            } else {
                this.topFloor = 1;
                this.bottomFloor = 0;
            }

            // Pick entrance to use
            this.useFartherEntrance = rnd.Next(2) == 0;

            this.calculateBounds();
            pieces.Add(this);

            BlockPos shaftOrgin = new BlockPos(this.orgin.x - 8, this.orgin.y, this.orgin.z);
            pieces.Add(new PieceSmallShaft(shaftOrgin, 9));
            pieces.Add(new PieceSmallShaft(shaftOrgin + new BlockPos(0, 9, 0), 6));
            pieces.Add(new PieceSmallShaft(shaftOrgin + new BlockPos(0, -6, 0), 6));

            pieces.Add(new PieceBedroom(shaftOrgin + new BlockPos(0, 9, 0)));

            new PieceHallway(new BlockPos(this.orgin.x - 5, this.orgin.y + 1, this.orgin.z - 8), Direction.WEST, pieces, piecesFromStart, rnd);
            new PieceHallway(new BlockPos(this.orgin.x + 5, this.orgin.y + 1, this.orgin.z + (this.useFartherEntrance ? 0 : -8)), Direction.EAST, pieces, piecesFromStart, rnd);
            new PieceHallway(new BlockPos(this.orgin.x, this.orgin.y + 1, this.orgin.z + 5), Direction.NORTH, pieces, piecesFromStart, rnd);
            new PieceHallway(new BlockPos(this.orgin.x, this.orgin.y + 1, this.orgin.z - 15), Direction.SOUTH, pieces, piecesFromStart, rnd);
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.orgin - new BlockPos(4, -1, 14);
            BlockPos p2 = this.orgin + new BlockPos(4, 6, 4);
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetY, offsetZ;
            Block b;
            byte meta;
            for (int i = p1.x; i <= p2.x; i++) {
                for (int j = p1.y; j <= p2.y; j++) {
                    for (int k = p1.z; k <= p2.z; k++) {
                        if(chunk.isInChunk(i, j, k)) {
                            b = Block.air;
                            meta = 0;
                            chunkCoordX = i - chunk.pos.x;
                            chunkCoordY = j - chunk.pos.y;
                            chunkCoordZ = k - chunk.pos.z;
                            offsetX = i - this.orgin.x;
                            offsetY = j - this.orgin.y;
                            offsetZ = k - this.orgin.z;

                            // Columns
                            if(offsetY < 5 && (
                                (offsetX == 3 && offsetZ == 3) ||
                                (offsetX == -3 && offsetZ == 3) ||
                                (offsetX == 3 && offsetZ == -4) ||
                                (offsetX == -3 && offsetZ == -4) ||
                                (offsetX == 3 && offsetZ == -11) ||
                                (offsetX == -3 && offsetZ == -11) ||
                                (offsetX == 3 && offsetZ == -13) ||
                                (offsetX == -3 && offsetZ == -13))) {
                                    b = Block.wood;
                                    meta = 1;
                            }
                            //Crosspiece
                            else if(offsetY == 5 && Mathf.Abs(offsetX) < 5 && (
                                offsetZ == 3 || offsetZ == -4 || offsetZ == -11 || offsetZ ==-13)) {
                                    b = Block.wood;
                                    meta = 0;
                            }
                            else if(offsetX == 0 && offsetY == 0 && offsetZ == 0) {
                                b = Block.grass;
                            } else {
                                b = Block.air;
                            }

                            //b = j == -1 ? Block.wood : Block.air;
                            if(b != null) {
                                chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, b);
                                chunk.setMeta(chunkCoordX, chunkCoordY, chunkCoordZ, meta);
                            }
                        }
                    }
                }
            }
        }

        // TODO change
        public override void calculateBounds() {
            this.pieceBounds = new Bounds(
                new Vector3(this.orgin.x, this.orgin.y + 4, this.orgin.z - 5),
                new Vector3(8, 5, 18));
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            NbtHelper.writeDirectBlockPos(tag, this.orgin, "center");
            tag.Add(new NbtInt("topFloor", this.topFloor));
            tag.Add(new NbtInt("bottomFloor", this.bottomFloor));
            tag.Add(new NbtByte("ueas", this.useFartherEntrance ? (byte)1 : (byte)0));
            return tag;
        }

        public override byte getPieceId() {
            return 0;
        }

        public override Color getPieceColor() {
            return Color.white;
        }
    }
}
