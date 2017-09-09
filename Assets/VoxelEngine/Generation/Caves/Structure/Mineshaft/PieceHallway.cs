using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Blocks;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    // TODO bounds is not smaller when shaft is 3 high.  Only a minor optimization.
    public class PieceHallway : PieceBase {

        private const int minLength = 3;
        private const int maxLength = 5;

        private BlockPos end;
        private Direction pointing;
        private bool is3High;

        public bool successfullyGenerated;

        public PieceHallway(NbtCompound tag) : base(tag) {
            this.end = NbtHelper.readDirectBlockPos(tag, "end");
            this.pointing = Direction.all[tag.Get<NbtInt>("pointing").IntValue];
            this.is3High = tag.Get<NbtByte>("3High").Value == 1;
        }

        public PieceHallway(StructureMineshaft shaft, BlockPos start, Direction hallwayDirection, int piecesFromCenter) : base(shaft, start) {
            this.end = this.orgin + (hallwayDirection.blockPos * this.shaft.rnd.Next(PieceHallway.minLength, PieceHallway.minLength + 1) * 8);
            this.pointing = hallwayDirection;
            this.is3High = this.shaft.rnd.Next(0, 2) == 0;

            this.calculateBounds();

            piecesFromCenter++;
            if(this.addToShaftIfValid(piecesFromCenter)) {
                this.successfullyGenerated = true;

                if (piecesFromCenter <= 1) {
                    // If we are still close to the start, always go straight, so we arent wrapping back around the middle.
                    new PieceHallway(this.shaft, this.end + this.pointing.blockPos, this.pointing, piecesFromCenter);
                }
                else {
                    int i = this.shaft.rnd.Next(7);
                    if (i <= 1) {// 0, 1
                        this.addHallway(this.pointing.getClockwise(), piecesFromCenter);
                    }
                    else if (i <= 3) { // 2, 3
                        this.addHallway(this.pointing.getCounterClockwise(), piecesFromCenter);
                    }
                    else if (i <= 5) { // 4, 5
                        this.addRoom(piecesFromCenter);
                    }
                    else if (i == 6) { // 6
                        new PieceHallway(this.shaft, this.end + this.pointing.blockPos, this.pointing, piecesFromCenter);
                    }
                }
            }
        }

        private void addHallway(Direction hallwayDirection, int piecesFromCenter) {
            BlockPos pos = this.end + (this.pointing.blockPos * 3) + (hallwayDirection.getOpposite().blockPos * this.shaft.rnd.Next(1, 3) * 2);
            pos.y += this.randomHallwayStep(this.shaft.rnd);
            new PieceHallway(this.shaft, pos, hallwayDirection, piecesFromCenter);
        }

        /// <summary>
        /// Adds a random room to the end of the hallway.
        /// </summary>
        private void addRoom(int piecesFromCenter) {
            int i = this.shaft.rnd.Next(5);
            BlockPos p = this.end + this.pointing.blockPos;
            if(i < 2) { // 0, 1
                new PieceMobSpawner(this.shaft, p, this.pointing, piecesFromCenter);
            } else if(i == 2) { // 2
                new PieceCrossing(this.shaft, p, this.pointing, piecesFromCenter);
            } else { //3, 4
                new PieceShaft(this.shaft, p, this.pointing, piecesFromCenter, 0);
            }
        }

        /// <summary>
        /// Moves the hallway up or down one block randomly
        /// </summary>
        private int randomHallwayStep(System.Random rnd) {
            int i = rnd.Next(0, 6);
            if (i == 0) {
                return 1;
            } else if (i == 1) {
                return -1;
            } else {
                return 0;
            }
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos pos1 = this.getPosMin() + BlockPos.up;
            BlockPos pos2 = this.getPosMax() - (this.is3High ? BlockPos.up : BlockPos.zero);
            int i, j, k, x, y, z;
            int i1 = Mathf.Max(pos1.x, pos2.x);
            int j1 = Mathf.Max(pos1.y, pos2.y);
            int k1 = Mathf.Max(pos1.z, pos2.z);
            for (i = Mathf.Min(pos1.x, pos2.x); i <= i1; i++) {
                for (j = Mathf.Min(pos1.y, pos2.y); j <= j1; j++) {
                    for (k = Mathf.Min(pos1.z, pos2.z); k <= k1; k++) {
                        x = i - chunk.worldPos.x;
                        y = j - chunk.worldPos.y;
                        z = k - chunk.worldPos.z;
                        if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                            chunk.setBlock(x, y, z, Block.air);
                        }
                    }
                }
            }

            // Add the supports, torch and rails.
            BlockPos pos = this.orgin;
            BlockPos endPoint = this.end + this.pointing.blockPos;
            int axis = (int)(this.pointing.axis);
            int perpAxis = (int)(this.pointing.axis == EnumAxis.X ? EnumAxis.Z : EnumAxis.X); // Perpendicular to axis
            int distanceToSupport = 0;
            Direction right = this.pointing.getClockwise();
            Direction left = this.pointing.getCounterClockwise();
            BlockPos rightDir = right.blockPos;
            BlockPos leftDir = left.blockPos;
            i = (this.is3High ? 3 : 4);
            k = i - 1;
            do {
                distanceToSupport++;
                if (distanceToSupport == 3) {
                    if(chunk.isInChunk(pos.x, pos.y + 3, pos.z) && rnd.Next(0, 3) == 0) {
                        this.addTorch(chunk, pos.x, pos.y + k, pos.z, this.pointing);
                    }
                } else if (distanceToSupport == 4) {
                    // Top middle
                    this.setStateIfInChunk(chunk, pos.x, pos.y + k, pos.z, Block.wood, perpAxis);

                    // Column.
                    for (j = 0; j < i; j++) {
                        this.setStateIfInChunk(chunk, pos.x + rightDir.x * 2, pos.y + j, pos.z + rightDir.z * 2, Block.wood, j == 3 ? perpAxis : 1);
                        this.setStateIfInChunk(chunk, pos.x + leftDir.x * 2, pos.y + j, pos.z + leftDir.z * 2, Block.wood, j == 3 ? perpAxis : 1);
                    }

                    // Top beam, one away from middle.
                    this.setStateIfInChunk(chunk, pos.x + rightDir.x, pos.y + k, pos.z + rightDir.z, Block.wood, perpAxis);
                    this.setStateIfInChunk(chunk, pos.x + leftDir.x, pos.y + k, pos.z + leftDir.z, Block.wood, perpAxis);

                    // Web.
                    j = rnd.Next(0, 100);
                    if (j < 10) {
                        this.setStateIfInChunk(chunk, pos.x + rightDir.x, pos.y + k - 1, pos.z + rightDir.z, Block.cobweb, BlockCobweb.getMetaForState(right, true));
                    } else if(j < 20) {
                        this.setStateIfInChunk(chunk, pos.x + leftDir.x, pos.y + k - 1, pos.z + leftDir.z, Block.cobweb, BlockCobweb.getMetaForState(left, true));
                    }

                    distanceToSupport = -4;
                }
                // Rail
                if (rnd.Next(0, 10) != 0) {
                    this.setStateIfInChunk(chunk, pos.x, pos.y, pos.z, Block.rail, axis == 0 ? 0 : 1);
                }

                // Gravel
                this.placeRndGravel(pos, chunk);
                this.placeRndGravel(pos + rightDir, chunk);
                this.placeRndGravel(pos + leftDir, chunk);

                pos += this.pointing.blockPos;

            } while (!pos.Equals(endPoint));
        }

        public override void calculateBounds() {
            Vector3 pieceCenter = ((this.orgin.toVector() / 2) + (this.end.toVector() / 2));
            this.pieceBounds = new Bounds(
                pieceCenter + Vector3.up,
                MathHelper.absVec((this.orgin - this.end).toVector() + (this.pointing.getClockwise().blockPos.toVector() * 4) + new Vector3(0, 4, 0)));
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            NbtHelper.writeDirectBlockPos(tag, this.end, "end");
            tag.Add(new NbtInt("pointing", this.pointing.index - 1));
            tag.Add(new NbtByte("3High", (byte)(this.is3High ? 1 : 0)));
            return tag;
        }

        public override byte getPieceId() {
            return 2;
        }

        public override Color getPieceColor() {
            return new Color(0, 100, 0);
        }

        private void placeRndGravel(BlockPos pos, Chunk chunk) {
            if(this.rndGravel() != null) {
                this.setStateIfInChunk(chunk, pos.x, pos.y - 1, pos.z, Block.gravel, 0);
            }
        }
    }
}