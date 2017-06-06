using fNbt;
using System;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public class PieceOrginBedroom : PieceOrginBase {

        private int length;

        public PieceOrginBedroom(NbtCompound tag) : base(tag) {
            this.length = tag.Get<NbtInt>("length").IntValue;
        }

        public PieceOrginBedroom(StructureMineshaft shaft, BlockPos shaftCenter) : base(shaft, shaftCenter) {
            this.length = 7 + (this.shaft.rnd.Next(0, 3) * 2);

            this.calculateBounds();
        }

        public override void calculateBounds() {
            this.setPieceSize(0, 4, this.length, 0, this.left ? 3 : 2, this.right ? 3 : 2);
        }

        public override void carvePiece(Chunk chunk, Random rnd) {
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
            tag.Add(new NbtInt("length", this.length));
            return tag;
        }

        public override byte getPieceId() {
            return 8;
        }
    }
}
