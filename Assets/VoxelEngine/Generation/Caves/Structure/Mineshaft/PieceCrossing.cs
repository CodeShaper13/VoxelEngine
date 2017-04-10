using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceCrossing : PieceIntersection {

        private int roomHeight;

        public PieceCrossing(NbtCompound tag) : base(tag) {
            this.roomHeight = tag.Get<NbtInt>("h").IntValue;
            this.sizeRadius = this.getSizeRadius(null); //This doesnt use the rnd param
        }

        public PieceCrossing(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd)
            : base(hallwayPoint, hallwayDir, pieces, piecesFromStart, rnd) {
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = new BlockPos(this.orgin.x - 4, this.orgin.y, this.orgin.z - 4);
            BlockPos p2 = new BlockPos(this.orgin.x + 4, this.orgin.y + 6, this.orgin.z + 4);
            Block b;
            byte meta;
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetY, offsetZ, absX, absZ;
            for (int i = p1.x; i <= p2.x; i++) {
                for (int j = p1.y; j <= p2.y; j++) {
                    for (int k = p1.z; k <= p2.z; k++) {
                        if (chunk.isInChunk(i, j, k)) {
                            b = Block.air;
                            meta = 0;
                            chunkCoordX = i - chunk.pos.x;
                            chunkCoordY = j - chunk.pos.y;
                            chunkCoordZ = k - chunk.pos.z;
                            offsetX = i - this.orgin.x;
                            offsetY = j - this.orgin.y;
                            offsetZ = k - this.orgin.z;
                            absX = Mathf.Abs(offsetX);
                            absZ = Mathf.Abs(offsetZ);
                            
                            // Column
                            if (absX == 3 && absZ == 3 && offsetY < 4) {
                                b = Block.wood;
                                meta = 1;
                            }
                            // Rail
                            else if (offsetY == 0 && (offsetX == 0 || offsetZ == 0) && rnd.Next(0, 10) != 0) {
                                b = Block.rail;
                                meta = offsetX == 0 ? (byte)1 : (byte)0;
                            }
                            //Beams
                            else if(offsetY == 4 && absX == 3 && absZ < 5) {
                                b = Block.wood;
                                meta = 2;
                            } else if(offsetY == 5 && absZ == 3 && absX < 5) {
                                b = Block.wood;
                                meta = 0;
                            }
                            // Ceiling
                            else if(offsetY == 6 && rnd.Next(4) > 0) {
                                b = null;
                            }
                            // Torch
                            else if (offsetY == 2) {
                                if (absX == 3 && absZ == 2 && rnd.Next(0, 8) == 0) {
                                    b = Block.torch;
                                    meta = (byte)(offsetZ == 2 ? 1 : 3);
                                    //chunk.world.setBlock(i, j, k, Block.torch, offsetZ == 2 ? (byte)1 : (byte)3, false);
                                    continue;
                                } else if(absX == 2 && absZ == 3 && rnd.Next(0, 8) == 0) {
                                    b = Block.torch;
                                    meta = (byte)(offsetX == 2 ? 2 : 4);
                                    //chunk.world.setBlock(i, j, k, Block.torch, offsetX == 2 ? (byte)2 : (byte)4, false);
                                    continue;
                                }
                            }

                            if(b != null) {
                                chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, b);
                                chunk.setMeta(chunkCoordX, chunkCoordY, chunkCoordZ, meta);
                            }
                        }
                    }
                }
            }
        }

        public override byte getPieceId() {
            return 1;
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("h", this.roomHeight));
            return tag;
        }

        protected override BlockPos getSizeRadius(System.Random rnd) {
            return new BlockPos(4, 0, 4);
        }

        protected override int getHeight() {
            return 6;
        }

        public override Color getPieceColor() {
            return Color.green;
        }
    }
}
