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
            this.northGate = tag.Get<NbtByte>("ng").ByteValue == 1;
            this.eastGate = tag.Get<NbtByte>("eg").ByteValue == 1;
            this.southGate = tag.Get<NbtByte>("sg").ByteValue == 1;
            this.westGate = tag.Get<NbtByte>("wg").ByteValue == 1;
        }

        public PieceMobSpawner(StructureMineshaft shaft, BlockPos hallwayPoint, Direction hallwayDir, int piecesFromCenter)
            : base(shaft, hallwayPoint + (hallwayDir.direction * 4)) {

            this.northGate = true; // rnd.Next(2) == 0;
            this.eastGate = true; //rnd.Next(2) == 0;
            this.southGate = true; // rnd.Next(2) == 0;
            this.westGate = true; // rnd.Next(2) == 0;

            piecesFromCenter += 1;
            if (this.func(piecesFromCenter)) {
                this.generateHallwaysAroundPoint(hallwayDir.getOpposite(), this.orgin, 5, piecesFromCenter);
            }
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.getPosMin();
            BlockPos p2 = this.getPosMax();
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetY, offsetZ, absX, absZ;
            Block block;
            for (int x = p1.x; x <= p2.x; x++) {
                for (int y = p1.y; y <= p2.y; y++) {
                    for (int z = p1.z; z <= p2.z; z++) {
                        if (chunk.isInChunk(x, y, z)) {
                            block = null;
                            chunkCoordX = x - chunk.pos.x;
                            chunkCoordY = y - chunk.pos.y;
                            chunkCoordZ = z - chunk.pos.z;
                            offsetX = x - this.orgin.x;
                            offsetY = y - this.orgin.y;
                            offsetZ = z - this.orgin.z;
                            absX = Mathf.Abs(offsetX);
                            absZ = Mathf.Abs(offsetZ);

                            // Gates
                            if(offsetY <= 4) {
                                if (offsetZ == 4 && absX <= 2) {
                                    if(this.northGate) {
                                        block = this.rndGate(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                                if(offsetX == 4 && absZ <= 2) {
                                    if (this.eastGate) {
                                        block = this.rndGate(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                                if (offsetZ == -4 && absZ <= 2) {
                                    if (this.southGate) {
                                        block = this.rndGate(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                                if (offsetX == -4 && absX <= 2) {
                                    if (this.westGate) {
                                        block = this.rndGate(rnd);
                                    } else {
                                        block = Block.air;
                                    }
                                }
                            }

                            // Floor
                            if(offsetY == 0) {
                                block = rnd.Next(0, 3) == 0 ? Block.gravel : null;
                            }
                            else if(absX < 4 && absZ < 4) {
                                block = Block.air;
                            }
                
                            if(block != null) {
                                chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, block);
                            }
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
            tag.Add(new NbtByte("ng", this.northGate ? (byte)1 : (byte)0));
            tag.Add(new NbtByte("eg", this.eastGate ? (byte)1 : (byte)0));
            tag.Add(new NbtByte("sg", this.southGate ? (byte)1 : (byte)0));
            tag.Add(new NbtByte("wg", this.westGate ? (byte)1 : (byte)0));
            return tag;
        }

        private Block rndGate(System.Random rnd) {
            if (rnd.Next(5) != 0) {
                return Block.fence;
            } else {
                return Block.air;
            }
        }
    }
}
