using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceCrossing : PieceIntersection {

        public PieceCrossing(NbtCompound tag) : base(tag) {
            this.sizeRadius = this.getSizeRadius(null); //This doesnt use the rnd param
        }

        public PieceCrossing(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd)
            : base(hallwayPoint, hallwayDir, pieces, piecesFromStart, rnd) {
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos s = new BlockPos(this.orgin.x - 3, this.orgin.y, this.orgin.z - 3);
            BlockPos e = new BlockPos(this.orgin.x + 3, this.orgin.y + 6, this.orgin.z + 3);
            Block b;
            for (int i = s.x; i <= e.x; i++) {
                for (int j = s.y; j <= e.y; j++) {
                    for (int k = s.z; k <= e.z; k++) {
                        b = Block.air;
                        int x = i - chunk.pos.x;
                        int y = j - chunk.pos.y;
                        int z = k - chunk.pos.z;
                        if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                            if (i == this.orgin.x && k == this.orgin.z && y == 4) {
                                chunk.world.setBlock(i, j, k, Block.torch, 0, false);
                                continue;
                            }
                            if (y == 0 && (i == this.orgin.x || k == this.orgin.z) && rnd.Next(0, 10) != 0) {
                                b = Block.rail;
                                //TODO set rail meta
                            }
                            chunk.setBlock(x, y, z, b);
                        }
                    }
                }
            }
        }

        public override byte getPieceId() {
            return 1;
        }

        protected override BlockPos getSizeRadius(System.Random rnd) {
            return new BlockPos(3, 0, 3);
        }

        protected override int getHeight() {
            return 6;
        }

        public override Color getPieceColor() {
            return Color.green;
        }
    }
}
