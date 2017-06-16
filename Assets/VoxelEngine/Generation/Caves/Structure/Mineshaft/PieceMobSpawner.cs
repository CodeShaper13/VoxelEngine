using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceMobSpawner : PieceBase {

        public bool northGate;
        public bool eastGate;
        public bool southGate;
        public bool westGate;

        public PieceMobSpawner(NbtCompound tag) : base(tag) {
            this.northGate = tag.Get<NbtByte>("nGateg").ByteValue == 1;
            this.eastGate = tag.Get<NbtByte>("eGateg").ByteValue == 1;
            this.southGate = tag.Get<NbtByte>("sGateg").ByteValue == 1;
            this.westGate = tag.Get<NbtByte>("wGateg").ByteValue == 1;
        }

        public PieceMobSpawner(StructureMineshaft shaft, BlockPos hallwayPoint, Direction hallwayDir, int piecesFromCenter)
            : base(shaft, hallwayPoint + (hallwayDir.direction * 4)) {

            piecesFromCenter += 1;
            if (this.func(piecesFromCenter)) {
                int i = this.generateHallwaysAroundPoint(hallwayDir.getOpposite(), this.orgin, 4, piecesFromCenter);
                this.northGate = this.func02(i, hallwayDir, Direction.NORTH);
                this.eastGate = this.func02(i, hallwayDir, Direction.EAST);
                this.southGate = this.func02(i, hallwayDir, Direction.SOUTH);
                this.westGate = this.func02(i, hallwayDir, Direction.WEST);
            }
        }

        private bool func02(int b, Direction hallwayDir, Direction dir) {
            return BitHelper.getBit(b, dir.directionId - 1) == 1 || hallwayDir.getOpposite() == dir;
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.getPosMin();
            BlockPos p2 = this.getPosMax();
            int offsetX, offsetY, offsetZ, absX, absZ;
            Block block;
            for (int x = p1.x; x <= p2.x; x++) {
                for (int y = p1.y; y <= p2.y; y++) {
                    for (int z = p1.z; z <= p2.z; z++) {
                        if (chunk.isInChunk(x, y, z)) {
                            block = null;
                            offsetX = x - this.orgin.x;
                            offsetY = y - this.orgin.y;
                            offsetZ = z - this.orgin.z;
                            absX = Mathf.Abs(offsetX);
                            absZ = Mathf.Abs(offsetZ);

                            // Gates
                            if(offsetY <= 3) {
                                if (offsetZ == 4 && absX <= 2) {
                                    if(this.northGate) {
                                        block = this.randomGateBlock(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                                if(offsetX == 4 && absZ <= 2) {
                                    if (this.eastGate) {
                                        block = this.randomGateBlock(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                                if (offsetZ == -4 && absX <= 2) {
                                    if (this.southGate) {
                                        block = this.randomGateBlock(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                                if (offsetX == -4 && absZ <= 2) {
                                    if (this.westGate) {
                                        block = this.randomGateBlock(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                            }

                            // Floor
                            if(offsetY == -1) {
                                block = this.rndGravel();
                                block = rnd.Next(0, 3) == 0 ? Block.gravel : null;
                            }
                            else if(absX < 4 && absZ < 4) {
                                block = Block.air;
                            }

                            this.setState(chunk, x, y, z, block, 0);
                        }
                    }
                }
            }
        }

        public override Color getPieceColor() {
            return Color.yellow;
        }

        public override byte getPieceId() {
            return 5;
        }

        public override void calculateBounds() {
            this.setPieceSize(1, 6, 4);
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtByte("nGate", this.northGate ? (byte)1 : (byte)0));
            tag.Add(new NbtByte("eGateg", this.eastGate ? (byte)1 : (byte)0));
            tag.Add(new NbtByte("sGateg", this.southGate ? (byte)1 : (byte)0));
            tag.Add(new NbtByte("wGateg", this.westGate ? (byte)1 : (byte)0));
            return tag;
        }

        private Block randomGateBlock(System.Random rnd) {
            if (rnd.Next(5) != 0) {
                return Block.fence;
            } else {
                return Block.air;
            }
        }
    }
}
