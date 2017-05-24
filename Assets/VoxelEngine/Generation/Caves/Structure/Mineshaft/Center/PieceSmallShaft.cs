using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public class PieceSmallShaft : PieceBase {

        private int height;
        private bool isBottom;

        public PieceSmallShaft(NbtCompound tag) : base(tag) {
            this.height = tag.Get<NbtInt>("height").IntValue;
            this.isBottom = tag.Get<NbtByte>("isBottom").ByteValue == 1;
        }

        public PieceSmallShaft(StructureMineshaft shaft, BlockPos orgin, int height, bool isBottom) : base(shaft, orgin) {
            this.height = height;
            this.isBottom = isBottom;
            this.calculateBounds();
        }

        public override void calculateBounds() {
            this.setPieceSize(0, this.height - 1, 3);
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.getPosMin();
            BlockPos p2 = this.getPosMax();
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetY, offsetZ;
            Block b;
            byte meta;
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

                            // Columns
                            if (Mathf.Abs(offsetX) == 2 && Mathf.Abs(offsetZ) == 2) {
                                b = Block.wood;
                                meta = 1;
                            }
                            // Bottom floor, if it's there
                            else if (this.isBottom && offsetY == 0) {
                                if (Random.Range(0, 2) == 0) {
                                    b = Block.stone;
                                }
                                else {
                                    b = Block.gravel;
                                }
                            }
                            // Ladder
                            else if(offsetX == -3 && offsetZ == 3) {
                                b = Block.ladder;
                                meta = 0;
                            }
                            // Side logs
                            else if (!this.isBottom && offsetY == 0) {
                                int absX = Mathf.Abs(offsetX);
                                int absZ = Mathf.Abs(offsetZ);
                                if(absX == 3 || absZ == 3) {
                                    b = Block.plank;
                                } else if(absX == 2 && absZ < 2) {
                                    b = Block.wood;
                                    meta = 2;
                                } else if (absZ == 2 && absX < 2) {
                                    b = Block.wood;
                                    meta = 0;
                                }
                            }
                            // Railing
                            else if(!this.isBottom && offsetY == 1) {
                                int absX = Mathf.Abs(offsetX);
                                int absZ = Mathf.Abs(offsetZ);
                                if (((absX == 2 && absZ < 2) || (absZ == 2 && absX < 2)) && rnd.Next(10) != 0) {
                                    b = Block.fence;
                                }
                            }

                            chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, b);
                            chunk.setMeta(chunkCoordX, chunkCoordY, chunkCoordZ, meta);
                        }
                    }
                }
            }
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("height", this.height));
            tag.Add(new NbtByte("isBottom", this.isBottom ? (byte)1 : (byte)0));
            return tag;
        }

        public override Color getPieceColor() {
            return Color.red;
        }

        public override byte getPieceId() {
            return 7;
        }
    }
}
