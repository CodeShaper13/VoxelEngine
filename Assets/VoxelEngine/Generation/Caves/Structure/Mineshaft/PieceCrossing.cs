using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceCrossing : PieceBase {

        public PieceCrossing(NbtCompound tag) : base(tag) { }

        public PieceCrossing(StructureMineshaft shaft, BlockPos hallwayPoint, Direction hallwayDir, int piecesFromCenter)
            : base(shaft, hallwayPoint + (hallwayDir.blockPos * 4)) {
            
            piecesFromCenter += 1;
            if (this.addToShaftIfValid(piecesFromCenter)) {
                this.generateHallwaysAroundPoint(hallwayDir.getOpposite(), this.orgin, 6, piecesFromCenter);
            }
        }

        public override void calculateBounds() {
            this.setPieceSize(1, 6, 4);
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.getPosMin();
            BlockPos p2 = this.getPosMax();
            Block block;
            int meta;
            int offsetX, offsetY, offsetZ, absX, absZ;
            for (int x = p1.x; x <= p2.x; x++) {
                for (int y = p1.y; y <= p2.y; y++) {
                    for (int z = p1.z; z <= p2.z; z++) {
                        if (chunk.isInChunk(x, y, z)) {
                            block = Block.air;
                            meta = 0;
                            offsetX = x - this.orgin.x;
                            offsetY = y - this.orgin.y;
                            offsetZ = z - this.orgin.z;
                            absX = Mathf.Abs(offsetX);
                            absZ = Mathf.Abs(offsetZ);
                            
                            // Column
                            if (absX == 3 && absZ == 3 && offsetY < 4) {
                                block = Block.wood;
                                meta = 1;
                            }
                            // Rail
                            else if (offsetY == 0 && (offsetX == 0 || offsetZ == 0) && rnd.Next(0, 10) != 0) {
                                block = Block.rail;
                                meta = offsetX == 0 ? (byte)1 : (byte)0;
                            }
                            //Beams
                            else if(offsetY == 4 && absX == 3 && absZ < 5) {
                                block = Block.wood;
                                meta = 2;
                            } else if(offsetY == 5 && absZ == 3 && absX < 5) {
                                block = Block.wood;
                                meta = 0;
                            }
                            // Ceiling
                            else if(offsetY == 6 && rnd.Next(4) > 0) {
                                block = null;
                            }
                            // Torch
                            else if (offsetY == 2) {
                                if (absX == 3 && absZ == 2 && rnd.Next(0, 8) == 0) {
                                    this.addTorch(chunk, x, y, z, offsetZ == 2 ? Direction.NORTH : Direction.SOUTH);
                                    block = null;
                                } else if(absX == 2 && absZ == 3 && rnd.Next(0, 8) == 0) {
                                    this.addTorch(chunk, x, y, z, offsetX == 2 ? Direction.EAST : Direction.WEST);
                                    block = null;
                                }
                            }
                            // Floor
                            else if (offsetY == -1) {
                                block = this.rndGravel();
                            }

                            this.setState(chunk, x, y, z, block, meta);
                        }
                    }
                }
            }
        }

        public override byte getPieceId() {
            return 1;
        }

        public override Color getPieceColor() {
            return Color.red;
        }
    }
}
