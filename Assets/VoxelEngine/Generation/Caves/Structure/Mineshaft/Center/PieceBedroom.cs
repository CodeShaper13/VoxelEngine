using fNbt;
using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft.Center {

    public class PieceBedroom : PieceBase {

        private Vector3 bottomCenter;

        public PieceBedroom(NbtCompound tag) : base(tag) {
        }

        public PieceBedroom(BlockPos shaftCenter) : base(new BlockPos(shaftCenter.x + 8, shaftCenter.y, shaftCenter.z)) {
            this.calculateBounds();
        }

        public override void calculateBounds() {
            this.pieceBounds = new Bounds(
                new Vector3(this.orgin.x, this.orgin.y + 2, this.orgin.z + 1),
                new Vector3(8, 4, 8));
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.orgin - new BlockPos(4, 0, 4);
            BlockPos p2 = this.orgin + new BlockPos(4, 4, 4);
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

                            if (offsetY == 0) {
                                b = Block.wood;
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
            return Color.cyan;
        }

        public override byte getPieceId() {
            return 8;
        }
    }
}
