using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;
using System;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceShaft : PieceBase {
                
        protected bool addedToList = false;

        // -1 = bottom piece of stack, 1 = top of stack
        private int specialFlag = 0;
        // 0 = NE, 1 = SE, 2 = SW, 3 = NW
        private int floor1Ladder;
        private int floor2Ladder;
        // -1 if there is not a ladder above/below
        private int floorBelowLadderFlag = -1;

        public PieceShaft(NbtCompound tag) : base(tag) {
            this.specialFlag = tag.Get<NbtInt>("flag").IntValue;
            this.floor1Ladder = tag.Get<NbtInt>("f1l").IntValue;
            this.floor2Ladder = tag.Get<NbtInt>("f2l").IntValue;
            this.floorBelowLadderFlag = tag.Get<NbtInt>("fbl").IntValue;
        }

        public PieceShaft(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd, int flag)
            : base(hallwayPoint + (hallwayDir.direction * 4)) {

            this.calculateBounds();

            if (this.isIntersecting(pieces)) {
                return;
            }
            pieces.Add(this);
            this.addedToList = true;

            piecesFromStart++;
            if (piecesFromStart > StructureMineshaft.SIZE_CAP) {
                return;
            }

            this.specialFlag = flag;
            this.floor1Ladder = rnd.Next(4);
            this.floor2Ladder = rnd.Next(4);

            if(this.specialFlag == 0) {
                //This is the middle/first piece
                PieceShaft up = new PieceShaft(new BlockPos(hallwayPoint.x, hallwayPoint.y + 16, hallwayPoint.z), hallwayDir, pieces, piecesFromStart, rnd, this.specialFlag + 1);
                PieceShaft down = new PieceShaft(new BlockPos(hallwayPoint.x, hallwayPoint.y - 16, hallwayPoint.z), hallwayDir, pieces, piecesFromStart, rnd, this.specialFlag - 1);
                if (!up.addedToList && !down.addedToList) {
                    //Both pieces failed, remove this piece. The whole stack failed, so nothing should be here
                    pieces.RemoveAt(pieces.Count - 1);
                } else {
                    // We added at least one piece, up or down or both
                    if (up.addedToList) {
                        up.floorBelowLadderFlag = this.floor2Ladder;
                    } else {
                        this.specialFlag = 1;
                    }
                    if (down.addedToList) {
                        this.floorBelowLadderFlag = down.floor2Ladder;
                    } else {
                        this.specialFlag = -1;
                    }
                }
            } else if(flag == -1) {
                //TODO chance for water filled bottom piece
            }

            this.generateHallways(hallwayDir.getOpposite(), this.orgin, 5, 5, pieces, piecesFromStart, rnd);
        }

        public override Color getPieceColor() {
            return Color.cyan;
        }

        public override byte getPieceId() {
            return 4;
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = new BlockPos(this.orgin.x - 4, this.orgin.y - 1, this.orgin.z - 4);
            BlockPos p2 = new BlockPos(this.orgin.x + 4, this.orgin.y + 15, this.orgin.z + 4);
            Direction torchDir = Direction.yPlane[rnd.Next(0, 4)];
            BlockPos torchPos = this.orgin + (torchDir.direction * 4);
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetZ;
            Block b;
            byte woodMeta = 0;
            for (int i = p1.x; i <= p2.x; i++) {
                for (int j = p1.y; j <= p2.y; j++) {
                    for (int k = p1.z; k <= p2.z; k++) {
                        if(chunk.isInChunk(i, j, k)) {
                            b = Block.air;
                            chunkCoordX = i - chunk.pos.x;
                            chunkCoordY = j - chunk.pos.y;
                            chunkCoordZ = k - chunk.pos.z;
                            offsetX = i - this.orgin.x;
                            offsetZ = k - this.orgin.z;

                            // Random gravel on ground
                            if (chunkCoordY == 0 && this.specialFlag == -1) {
                                if(rnd.Next(4) == 0) {
                                    b = Block.gravel;
                                } else {
                                    b = null;
                                }
                            }
                            // Top room code block
                            else if (this.specialFlag == 1 && chunkCoordY > 8) {
                                if(chunkCoordY == 15) {
                                    if ((Mathf.Abs(offsetX) < 3 || Mathf.Abs(offsetZ) < 3) && rnd.Next(3) > 0) {
                                        b = null;
                                    }
                                } else if(chunkCoordY == 14) {
                                    if(Mathf.Abs(offsetZ) == 4 || Mathf.Abs(offsetX) == 4) {
                                        if(rnd.Next(2) == 0) {
                                            b = null;
                                        }
                                    }
                                } else if(chunkCoordY == 13) {
                                    if(Mathf.Abs(offsetX) < 4 && offsetZ == 0) {
                                        b = Block.wood;
                                        woodMeta = 0;
                                    }
                                } else if(chunkCoordY == 12) {
                                    if(Mathf.Abs(offsetX) == 2 && (Mathf.Abs(offsetZ) <= 4)) {
                                        b = Block.wood;
                                        woodMeta = 2;
                                    }
                                } else { // 11, 10, 9
                                    int xAbs = Mathf.Abs(offsetX);
                                    int zAbs = Mathf.Abs(offsetZ);
                                    if (xAbs == 2 && zAbs == 4) {
                                        b = Block.wood;
                                        woodMeta = 1;
                                    } else if(chunkCoordY == 9) {
                                        if(xAbs == 3 && zAbs <= 2 && rnd.Next(10) != 0) {
                                            b = Block.fence;
                                        } else if(xAbs == 3 && zAbs == 4 && rnd.Next(25) == 0) {
                                            RandomChest.SPAWN_CHEST.makeChest(chunk.world, i, j, k, k > this.orgin.z ? Direction.NORTH : Direction.SOUTH, rnd);
                                            continue;
                                        }
                                    }
                                }
                            }
                            // Torch
                            else if (i == torchPos.x && k == torchPos.z && chunkCoordY == 11) {
                                chunk.world.setBlock(i, j, k, Block.torch, BlockTorch.getMetaFromDirection(torchDir), false);
                                continue;
                            }
                            // Railing
                            else if (chunkCoordY == 9 || (chunkCoordY == 1 && this.specialFlag != -1)) {
                                int xAbs = Mathf.Abs(offsetX);
                                int zAbs = Mathf.Abs(offsetZ);
                                if (((xAbs == 3 && zAbs < 3) || (zAbs == 3 && xAbs < 3)) && rnd.Next(20) != 0) {
                                    b = Block.fence;
                                }
                            }
                            // Floor
                            else if(chunkCoordY == 8 || (chunkCoordY == 0)) {
                                if(Mathf.Abs(offsetX) > 2 || Mathf.Abs(offsetZ) > 2) {
                                    b = Block.wood;
                                    woodMeta = 1;
                                }
                            }

                            if (b != null) {
                                chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, b);
                                if (b == Block.wood) {
                                    chunk.setMeta(chunkCoordX, chunkCoordY, chunkCoordZ, woodMeta);
                                }
                            }
                        }
                    }
                }
            }
            this.placeLadder(this.floor1Ladder, chunk, this.orgin.x, this.orgin.z, true);
            if(this.specialFlag != 1) {
                this.placeLadder(this.floor2Ladder, chunk, this.orgin.x, this.orgin.z, false);
            }
        }

        // Places the ladder for a "floor"
        private void placeLadder(int ladderFlag, Chunk chunk, int orginX, int orginZ, bool isBottomFloor) {
            BlockPos p = this.getLadderOffset(ladderFlag, orginX, orginZ);
            int x1, z1, y;
            if (chunk.isInChunkIgnoreY(p.x, p.z)) {
                x1 = p.x - chunk.pos.x;
                z1 = p.z - chunk.pos.z;
                //int ladderBottom = (isBottomFloor ? (this.specialFlag == -1 ? 1 : 1) : 9);
                int ladderBottom = (isBottomFloor ? 1 : 9);
                int ladderTop = (isBottomFloor ? 11 : (this.specialFlag == 1 ? 11 : 16));
                for (y = ladderBottom; y < ladderTop; y++) {                    
                    chunk.setBlock(x1, y, z1, Block.ladder);
                    chunk.setMeta(x1, y, z1, (byte)ladderFlag);
                }
            }

            // Place the ladder stub
            p = this.getLadderOffset(this.floorBelowLadderFlag, orginX, orginZ);
            if (chunk.isInChunkIgnoreY(p.x, p.z)) {
                if (isBottomFloor && this.floorBelowLadderFlag != -1) {
                    x1 = p.x - chunk.pos.x;
                    z1 = p.z - chunk.pos.z;
                    for (y = 0; y < 3; y++) {
                        chunk.setBlock(x1, y, z1, Block.ladder);
                        chunk.setMeta(x1, y, z1, (byte)this.floorBelowLadderFlag);
                    }
                }
            }
        }

        private BlockPos getLadderOffset(int ladderFlag, int orginX, int orginZ) {
            if (ladderFlag == 0) {
                return new BlockPos(orginX + 4, 0, orginZ + 4);
            } else if (ladderFlag == 1) {
                return new BlockPos(orginX + 4, 0, orginZ + -4);
            } else if (ladderFlag == 2) {
                return new BlockPos(orginX + -4, 0, orginZ + -4);
            } else { // 3
                return new BlockPos(orginX + -4, 0, orginZ + 4);
            }
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("flag", this.specialFlag));
            tag.Add(new NbtInt("f1l", this.floor1Ladder));
            tag.Add(new NbtInt("f2l", this.floor2Ladder));
            tag.Add(new NbtInt("fbl", this.floorBelowLadderFlag));
            return tag;
        }

        public override void calculateBounds() {
            this.pieceBounds = new Bounds(
                new Vector3(this.orgin.x, this.orgin.y + 6.5f, this.orgin.z),
                new Vector3(8, 15, 8));
        }
    }
}