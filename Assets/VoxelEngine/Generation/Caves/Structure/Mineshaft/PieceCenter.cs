using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceCenter : PieceBase {

        private BlockPos center;

        public PieceCenter(NbtCompound tag) {
            this.center = NbtHelper.readDirectBlockPos(tag, "center");
        }

        public PieceCenter(BlockPos center, List<PieceBase> pieces, int piecesFromStart, System.Random rnd) {
            this.center = center;
            this.calculateBounds();
            pieces.Add(this);

            new PieceHallway(new BlockPos(this.center.x + 9, this.center.y, this.center.z), Direction.EAST, pieces, piecesFromStart, rnd);
            new PieceHallway(new BlockPos(this.center.x - 9, this.center.y, this.center.z), Direction.WEST, pieces, piecesFromStart, rnd);
            new PieceHallway(new BlockPos(this.center.x, this.center.y, this.center.z + 9), Direction.NORTH, pieces, piecesFromStart, rnd);
            new PieceHallway(new BlockPos(this.center.x, this.center.y, this.center.z - 9), Direction.SOUTH, pieces, piecesFromStart, rnd);
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos s = this.center - new BlockPos(8, 8, 8);
            BlockPos e = this.center + new BlockPos(8, 8, 8);
            Block b;
            for (int i = s.x; i <= e.x; i++) {
                for (int j = s.y; j <= e.y; j++) {
                    for (int k = s.z; k <= e.z; k++) {
                        b = j == -1 ? Block.wood : Block.air; //TODO hardcoded, replace
                        int x = i - chunk.pos.x;
                        int y = j - chunk.pos.y;
                        int z = k - chunk.pos.z;
                        if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                            chunk.setBlock(x, y, z, b);
                        }
                    }
                }
            }
        }

        public override void calculateBounds() {
            this.pieceBounds = new Bounds(this.center.toVector(), new Vector3(16, 16, 16));
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            NbtHelper.writeDirectBlockPos(tag, this.center, "center");
            return tag;
        }

        public override byte getPieceId() {
            return 0;
        }

        public override Color getPieceColor() {
            return Color.white;
        }
    }
}
