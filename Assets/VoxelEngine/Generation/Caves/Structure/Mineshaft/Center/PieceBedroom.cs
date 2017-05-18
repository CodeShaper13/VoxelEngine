using fNbt;
using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public class PieceBedroom : PieceBase {

        private bool left;
        private bool right;
        private int length;
        /// <summary> 0 = stone, 1 = planks </summary>
        private int floorType;

        public PieceBedroom(NbtCompound tag) : base(tag) {
            this.right = tag.Get<NbtByte>("right").ByteValue == 1 ? true : false;
            this.left = tag.Get<NbtByte>("left").ByteValue == 1 ? true : false;
            this.length = tag.Get<NbtInt>("left").IntValue;
            this.floorType = tag.Get<NbtInt>("floorType").IntValue;
        }

        public PieceBedroom(StructureMineshaft shaft, BlockPos shaftCenter) : base(shaft, new BlockPos(shaftCenter.x + 4, shaftCenter.y, shaftCenter.z)) {
            int i = this.shaft.rnd.Next(0, 5);
            if(i < 2) {
                this.right = true;
            } else if(i == 2) {
                this.right = true;
                this.left = true;
            } else if(i > 2) {
                this.left = true;
            }

            this.length = 7 + (this.shaft.rnd.Next(0, 3) * 2);
            this.floorType = this.shaft.rnd.Next(0, 4) == 0 ? 1 : 0;

            this.calculateBounds();
        }

        public override void calculateBounds() {
            this.setPieceSize(1, 4, this.length, 0, 3, 3);
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
                            if(offsetX == 0 && (Math.Abs(offsetZ) > 1 || offsetY > 3)) {
                                block = Block.stone;
                            }
                            // Floor
                            else if(offsetY == 0) {
                                block = this.floorType == 0 ? Block.stone : Block.plank;
                            }
                            // Wall stuff
                            else if(offsetY == 1 && offsetX > 0) {
                                if (((this.left && offsetZ == 3) || (this.right && offsetZ == -3))) {
                                    if (offsetX % 2 == 1) {
                                        if (rnd.Next(0, 10) == 0) {
                                            block = null;
                                            RandomChest.MINESHAFT_BEDRROM_CHEST.makeChest(chunk.world, x, y, z, offsetZ > 0 ? Direction.SOUTH : Direction.NORTH, rnd);
                                        } else {
                                            block = Block.wood;
                                            meta = 1;
                                        }
                                    }
                                    else {
                                        block = Block.bed;
                                    }
                                }
                                else if (((this.left && offsetZ == 2) || (this.right && offsetZ == -2)) && offsetX % 2 == 0) {
                                    block = Block.bed;
                                }
                            }                 
                            // Torch
                            else if (offsetX == this.length && offsetY == 3 && offsetZ == 0 && rnd.Next(0, 15) != 0) {
                                block = Block.torch;
                                meta = 2; // East
                            }

                            if (block != null) {
                                chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, block);
                                chunk.setMeta(chunkCoordX, chunkCoordY, chunkCoordZ, meta);
                            }
                        }
                    }
                }
            }
        }

        public override Color getPieceColor() {
            return Color.cyan;
        }

        public override byte getPieceId() {
            return 8;
        }
    }
}
