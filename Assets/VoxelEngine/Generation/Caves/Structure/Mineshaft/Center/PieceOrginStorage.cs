using fNbt;
using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public class PieceOrginStorage : PieceOrginBase {

        private int unitsLong;

        public PieceOrginStorage(NbtCompound tag) : base(tag) {
            this.unitsLong = tag.Get<NbtInt>("unitsLong").IntValue;
        }

        public PieceOrginStorage(StructureMineshaft shaft, BlockPos shaftCenter) : base(shaft, shaftCenter) {
            this.unitsLong = this.shaft.rnd.Next(0, 2) + 2;

            this.calculateBounds();
        }

        public override void calculateBounds() {
            this.setPieceSize(0, 4, (this.unitsLong * 3) + 1, 0, this.left ? 3 : 2, this.right ? 3 : 2);
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.getPosMin();
            BlockPos p2 = this.getPosMax();
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetY, offsetZ;
            Block block;
            int meta;
            for (int x = p1.x; x <= p2.x; x++) {
                for (int y = p1.y; y <= p2.y; y++) {
                    for (int z = p1.z; z <= p2.z; z++) {
                        if (chunk.isInChunk(x, y, z)) {
                            block = Block.air;
                            meta = 0;
                            chunkCoordX = x - chunk.pos.x;
                            chunkCoordY = y - chunk.pos.y;
                            chunkCoordZ = z - chunk.pos.z;
                            offsetX = x - this.orgin.x;
                            offsetY = y - this.orgin.y;
                            offsetZ = z - this.orgin.z;

                            // Door
                            if (offsetX == 0 && (Math.Abs(offsetZ) > 1 || offsetY > 3)) {
                                block = Block.stone;
                            }
                            // Floor
                            else if (offsetY == 0) {
                                block = this.floorType == 0 ? Block.stone : Block.plank;
                            }
                            // Wall stuff
                            else if (offsetY > 0 && offsetX > 0) {
                                if (((this.left && offsetZ == 3) || (this.right && offsetZ == -3))) {
                                    if (offsetX % 3 == 1) {
                                        block = Block.wood;
                                        meta = 1;
                                    } else {
                                        if((offsetY == 1 || offsetY == 3) && rnd.Next(0, 2) == 0) {
                                            block = null;
                                            RandomChest.MINESHAFT_STOREROOM.makeChest(chunk.world, x, y, z, offsetZ > 0 ? Direction.SOUTH : Direction.NORTH, rnd);
                                        } else if(offsetY == 2) {
                                            block = Block.plankSlab;
                                            meta = 4;
                                        }
                                    }
                                }
                            }
                            // Torch
                            else if (offsetX == (this.unitsLong * 3) + 1 && offsetY == 3 && offsetZ == 0 && rnd.Next(0, 15) != 0) {
                                this.addTorch(chunk, x, y, z, Direction.EAST);
                                block = null;
                            }

                            this.setState(chunk, x, y, z, block, meta);
                        }
                    }
                }
            }
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("unitsLong", this.unitsLong));
            return tag;
        }

        public override Color getPieceColor() {
            return Color.green;
        }

        public override byte getPieceId() {
            return 9;
        }
    }
}
