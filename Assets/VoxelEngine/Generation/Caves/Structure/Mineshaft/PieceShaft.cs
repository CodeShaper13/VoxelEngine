using System;
using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceShaft : PieceIntersection {
                
        protected bool addedToList = false;
        // -1 = bottom piece of stack, 1 = top of stack
        private int specialFlag = 0;

        public PieceShaft(NbtCompound tag) : base(tag) {
            this.specialFlag = tag.Get<NbtInt>("flag").IntValue;
        }

        public PieceShaft(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd, int distance)
            : base(hallwayPoint, hallwayDir, pieces, piecesFromStart, rnd) {

            distance++;

            if (distance == 1) {
                PieceShaft up = new PieceShaft(new BlockPos(hallwayPoint.x, hallwayPoint.y + 16, hallwayPoint.z), hallwayDir, pieces, piecesFromStart, rnd, distance);
                PieceShaft down = new PieceShaft(new BlockPos(hallwayPoint.x, hallwayPoint.y - 16, hallwayPoint.z), hallwayDir, pieces, piecesFromStart, rnd, distance);
                if(up.addedToList == false && down.addedToList == false) {
                    //Both pieces failed, remove this piece. The whole stack failed
                    pieces.RemoveAt(pieces.Count - 1);
                } else {
                    if(up.addedToList) {
                        up.specialFlag = 1;
                    } else {
                        this.specialFlag = 1;
                    }

                    if(down.addedToList) {
                        down.specialFlag = -1;
                    } else {
                        this.specialFlag = -1;
                    }
                }
            }
        }

        protected override void pieceAddedCallback() {
            this.addedToList = true;
        }

        public override Color getPieceColor() {
            return Color.cyan;
        }

        public override byte getPieceId() {
            return 4;
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = new BlockPos(this.orgin.x - 4, this.orgin.y, this.orgin.z - 4);
            BlockPos p2 = new BlockPos(this.orgin.x + 4, this.orgin.y + 16, this.orgin.z + 4);
            BlockPos torchPos = this.orgin + (Direction.all[rnd.Next(1, 5)].direction * 4);
            int x, y, z, i1, k1;
            Block b;
            for (int i = p1.x; i <= p2.x; i++) {
                for (int j = p1.y; j <= p2.y; j++) {
                    for (int k = p1.z; k <= p2.z; k++) {
                        b = Block.air;
                        x = i - chunk.pos.x;
                        y = j - chunk.pos.y;
                        z = k - chunk.pos.z;
                        if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                            if(i == torchPos.x && k == torchPos.z && y == 10) {
                                chunk.world.setBlock(i, j, k, Block.torch, false);
                                continue;
                            } else if (i == this.orgin.x + 4 && k == this.orgin.z + 4) {
                                b = Block.ladder;
                            } else if (y == 8 || (y == 0 && this.specialFlag != -1)) {
                                i1 = Mathf.Abs(this.orgin.x - i);
                                k1 = Mathf.Abs(this.orgin.z - k);
                                if ((i1 == 3 && k1 < 3) || (k1 == 3 && i1 < 3)) {
                                    b = Block.placeholder;
                                }
                            } else if(y == 7 || (y == 15 && this.specialFlag != 1)) {
                                if(Mathf.Abs(this.orgin.x - i) > 2 || Mathf.Abs(this.orgin.z - k) > 2) {
                                    b = Block.wood;
                                }
                            }
                            chunk.setBlock(x, y, z, b);
                            if(b == Block.ladder) {
                                chunk.setMeta(x, y, z, 1); //TODO meta
                            }
                        }
                    }
                }
            }
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("flag", this.specialFlag));
            return tag;
        }

        protected override BlockPos getSizeRadius(System.Random rnd) {
            return new BlockPos(4, 0, 4);
        }

        protected override int getHeight() {
            return 15;
        }
    }
}
