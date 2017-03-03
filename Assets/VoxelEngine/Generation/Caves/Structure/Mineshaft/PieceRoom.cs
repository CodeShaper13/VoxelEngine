using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Blocks;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceRoom : PieceIntersection {

        // 0 = X, 1 = 0
        public int axis;

        public PieceRoom(NbtCompound tag) : base(tag) {
            this.sizeRadius = new BlockPos(
                tag.Get<NbtInt>("sizeRx").IntValue, 0,
                tag.Get<NbtInt>("sizeRz").IntValue);
        }

        public PieceRoom(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd)
            : base(hallwayPoint, hallwayDir, pieces, piecesFromStart, rnd) {
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos s = new BlockPos(this.orgin.x - this.sizeRadius.x, this.orgin.y, this.orgin.z - this.sizeRadius.z);
            BlockPos e = new BlockPos(this.orgin.x + this.sizeRadius.x, this.orgin.y + 6, this.orgin.z + this.sizeRadius.z);
            Block b;
            byte meta;
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetY, offsetZ;
            for (int i = s.x; i <= e.x; i++) {
                for (int j = s.y; j <= e.y; j++) {
                    for (int k = s.z; k <= e.z; k++) {
                        if (chunk.isInChunk(i, j, k)) {
                            b = Block.air;
                            meta = 0;
                            chunkCoordX = i - chunk.pos.x;
                            chunkCoordY = j - chunk.pos.y;
                            chunkCoordZ = k - chunk.pos.z;
                            offsetX = i - this.orgin.x;
                            offsetY = j - this.orgin.y;
                            offsetZ = k - this.orgin.z;
                            chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, Block.air);
                        }
                    }
                }
            }
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtInt("axis", this.axis));
            return tag;
        }

        public override byte getPieceId() {
            return 3;
        }

        public override Color getPieceColor() {
            return Color.red;
        }

        protected override BlockPos getSizeRadius(System.Random rnd) {
            if(rnd.Next(2) == 0) {
                return new BlockPos(9, 0, 4); // X
            }
            return new BlockPos(4, 0, 9); // Z
        }

        protected override int getHeight() {
            return 6;
        }
    }
}
