using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Blocks;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceHallway : PieceBase {

        private const int minLength = 3;
        private const int maxLength = 5;

        private BlockPos start;
        private BlockPos end;
        private Direction pointing;

        public PieceHallway(NbtCompound tag) {
            this.start = NbtHelper.readDirectBlockPos(tag, "start");
            this.end = NbtHelper.readDirectBlockPos(tag, "end");
            this.pointing = Direction.all[tag.Get<NbtInt>("pointing").IntValue];
        }

        public PieceHallway(BlockPos start, Direction direction, List<PieceBase> pieces, int piecesFromStart, System.Random rnd) {
            this.start = start;
            this.end = this.start + (direction.direction * rnd.Next(PieceHallway.minLength, PieceHallway.minLength + 1) * 8);
            this.pointing = direction;

            this.calculateBounds();

            if(this.isIntersecting(pieces)) {
                return;
            } else {
                pieces.Add(this);
            }

            piecesFromStart++;
            if (piecesFromStart > StructureMineshaft.SIZE_CAP) {
                return;
            }

            if (piecesFromStart < 2) {
                // If we are still close to the start, always go straight, so we arent wrapping back around the middle
                new PieceHallway(this.end + this.pointing.direction, this.pointing, pieces, piecesFromStart, rnd);
            } else if(piecesFromStart < 4) {
                if(rnd.Next(2) == 0) {
                    new PieceRoom(this.end + this.pointing.direction, this.pointing, pieces, piecesFromStart, rnd);
                } else {
                    this.addRoom(pieces, piecesFromStart, rnd);
                }
            } else {
                int i = rnd.Next(7);
                if (i == 0 || i == 1) {
                    this.addHallway(this.pointing.getClockwise(), pieces, piecesFromStart, rnd);
                }
                else if (i == 2 || i == 3) {
                    this.addHallway(this.pointing.getCounterClockwise(), pieces, piecesFromStart, rnd);
                }
                else if (i == 4) {
                    new PieceHallway(this.end + this.pointing.direction, this.pointing, pieces, piecesFromStart, rnd);
                }
                else if (i == 5 || i == 6) {
                    this.addRoom(pieces, piecesFromStart, rnd);
                }
            }
        }

        private void addHallway(Direction dFace, List<PieceBase> pieces, int piecesFromStart, System.Random rnd) {
            new PieceHallway(this.end + (this.pointing.direction * 3) + (dFace.getOpposite().direction * rnd.Next(1, 3) * 2), dFace, pieces, piecesFromStart, rnd);
        }

        private void addRoom(List<PieceBase> pieces, int piecesFromStart, System.Random rnd) {
            int i = rnd.Next(6);
            BlockPos p = this.end + this.pointing.direction;
            if(i < 2) { // 0, 1
                new PieceRoom(p, this.pointing, pieces, piecesFromStart, rnd);
            } else if(i == 2) { // 2
                new PieceShaft(p, this.pointing, pieces, piecesFromStart, rnd, 0);
            } else { //3, 4
                new PieceCrossing(p, this.pointing, pieces, piecesFromStart, rnd);
            }
        }

        // Unused
        private int randomHallwayStep(System.Random rnd) {
            int i = rnd.Next(0, 6);
            if (i == 0) {
                return 1;
            }
            else if (i == 1) {
                return -1;
            }
            else {
                return 0;
            }
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos pos1 = this.start + this.pointing.getClockwise().direction * 2; //bottom right
            BlockPos pos2 = this.end + this.pointing.getCounterClockwise().direction * 2 + new BlockPos(0, 3, 0);
            int i, j, k, x, y, z;
            int i1 = Mathf.Max(pos1.x, pos2.x);
            int j1 = Mathf.Max(pos1.y, pos2.y);
            int k1 = Mathf.Max(pos1.z, pos2.z);
            for (i = Mathf.Min(pos1.x, pos2.x); i <= i1; i++) {
                for (j = Mathf.Min(pos1.y, pos2.y); j <= j1; j++) {
                    for (k = Mathf.Min(pos1.z, pos2.z); k <= k1; k++) {
                        x = i - chunk.pos.x;
                        y = j - chunk.pos.y;
                        z = k - chunk.pos.z;
                        if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                            chunk.setBlock(x, y, z, Block.air);
                        }
                    }
                }
            }
            BlockPos pos = this.start;
            BlockPos endPoint = this.end + this.pointing.direction;
            byte axis = (byte)(this.pointing.axis == EnumAxis.X ? EnumAxis.Z : EnumAxis.X);
            i = 0;
            do {
                i++;
                if (i == 3 && rnd.Next(4) == 0) {
                    x = pos.x - chunk.pos.x;
                    y = pos.y + 4 - chunk.pos.y;
                    z = pos.z - chunk.pos.z;
                    if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                        chunk.world.setBlock(pos.x, pos.y + 3, pos.z, Block.lantern, false);
                    }
                }
                if (i == 4) {
                    this.setStateIfInChunk(chunk, pos.x, pos.y + 3, pos.z, Block.wood, axis);
                    pos1 = pos + this.pointing.getClockwise().direction * 2;
                    for (j = 0; j < 4; j++) {
                        this.setStateIfInChunk(chunk, pos1.x, pos1.y + j, pos1.z, Block.wood, j == 3 ? axis : (byte)0);
                    }
                    pos1 = pos + this.pointing.getCounterClockwise().direction * 2;
                    for (j = 0; j < 4; j++) {
                        this.setStateIfInChunk(chunk, pos1.x, pos1.y + j, pos1.z, Block.wood, j == 3 ? axis : (byte)0);
                    }
                    pos1 = pos + this.pointing.getClockwise().direction;
                    this.setStateIfInChunk(chunk, pos1.x, pos1.y + 3, pos1.z, Block.wood, axis);
                    pos1 = pos + this.pointing.getCounterClockwise().direction;
                    this.setStateIfInChunk(chunk, pos1.x, pos1.y + 3, pos1.z, Block.wood, axis);

                    i = -4;
                }
                if (rnd.Next(0, 10) != 0) {
                    this.setIfInChunk(chunk, pos.x, pos.y, pos.z, Block.rail);
                }
                pos += this.pointing.direction;

            } while (!pos.Equals(endPoint));
        }

        public override void calculateBounds() {
            Vector3 pieceCenter = ((this.start.toVector() / 2) + (this.end.toVector() / 2));
            this.pieceBounds = new Bounds(
                new Vector3(pieceCenter.x, pieceCenter.y + 1.5f, pieceCenter.z),
                MathHelper.absVec((this.start - this.end).toVector() + (this.pointing.getClockwise().direction.toVector() * 4) + new Vector3(0, 3, 0)));

        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            NbtHelper.writeDirectBlockPos(tag, this.start, "start");
            NbtHelper.writeDirectBlockPos(tag, this.end, "end");
            tag.Add(new NbtInt("pointing", this.pointing.directionId - 1));
            return tag;
        }

        public override byte getPieceId() {
            return 2;
        }

        public override Color getPieceColor() {
            return Color.blue;
        }
    }
}