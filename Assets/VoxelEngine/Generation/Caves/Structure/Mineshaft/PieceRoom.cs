using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Blocks;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceRoom : PieceIntersection {

        public PieceRoom(NbtCompound tag) : base(tag) {
            this.sizeRadius = new BlockPos(
                tag.Get<NbtInt>("sizeRx").IntValue, 0,
                tag.Get<NbtInt>("sizeRz").IntValue);
        }

        public PieceRoom(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd)
            : base(hallwayPoint, hallwayDir, pieces, piecesFromStart, rnd) {
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos s = new BlockPos(this.orgin.x - 3, this.orgin.y, this.orgin.z - 3);
            BlockPos e = new BlockPos(this.orgin.x + 3, this.orgin.y + 6, this.orgin.z + 3);
            for (int i = s.x; i <= e.x; i++) {
                for (int j = s.y; j <= e.y; j++) {
                    for (int k = s.z; k <= e.z; k++) {
                        int x = i - chunk.pos.x;
                        int y = j - chunk.pos.y;
                        int z = k - chunk.pos.z;
                        if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                            chunk.setBlock(x, y, z, Block.air);
                        }
                    }
                }
            }
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("sizeRx", this.sizeRadius.x));
            tag.Add(new NbtInt("sizeRz", this.sizeRadius.z));
            return tag;
        }

        public override byte getPieceId() {
            return 3;
        }

        public override Color getPieceColor() {
            return Color.red;
        }

        protected override BlockPos getSizeRadius(System.Random rnd) {
            return new BlockPos(3 + rnd.Next(0, 3), 0, 3 + rnd.Next(0, 3));
        }

        protected override int getHeight() {
            return 6;
        }
    }
}
