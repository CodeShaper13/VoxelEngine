using fNbt;
using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public class PieceSmallShaft : PieceBase {

        private int height;

        public PieceSmallShaft(NbtCompound tag) : base(tag) {
            tag.Add(new NbtInt("sh", this.height));
        }

        public PieceSmallShaft(BlockPos orgin, int height) : base(orgin) {
            this.height = height;
            this.calculateBounds();
        }

        public override void calculateBounds() {
            this.pieceBounds = new Bounds(
                new Vector3(this.orgin.x, this.orgin.y + (this.height / 2), this.orgin.z),
                new Vector3(6, this.height - 1, 6));
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.orgin - new BlockPos(3, 0, 3);
            BlockPos p2 = this.orgin + new BlockPos(3, this.height - 1, 3);
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
                            // Ladder
                            else if(offsetX == -3 && offsetZ == 3) {
                                b = Block.ladder;
                                meta = 0;
                            }
                            // Side logs
                            else if (offsetY == 0) {
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
                            else if(offsetY == 1) {
                                int absX = Mathf.Abs(offsetX);
                                int absZ = Mathf.Abs(offsetZ);
                                if (((absX == 2 && absZ < 2) || (absZ == 2 && absX < 2)) && rnd.Next(10) != 0) {
                                    b = Block.fence;
                                }
                            }

                            if (b != null) {
                                chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, b);
                                chunk.setMeta(chunkCoordX, chunkCoordY, chunkCoordZ, meta);
                            }
                        }
                    }
                }
            }
        }

        public override Color getPieceColor() {
            return Color.red;
        }

        public override byte getPieceId() {
            return 7;
        }
    }
}
