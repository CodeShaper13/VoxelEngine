using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Blocks;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    // Unused as of now.
    public class PieceRoom : PieceBase {

        // 0 = X, 1 = 0
        public int axis;

        public PieceRoom(NbtCompound tag) : base(tag) { }

        public PieceRoom(StructureMineshaft shaft, BlockPos hallwayPoint, Direction hallwayDir, int piecesFromCenter)
            : base(shaft, hallwayPoint + (hallwayDir.direction * 8)) {

            piecesFromCenter += 1;
            if (this.func(piecesFromCenter)) {
                this.generateHallwaysAroundPoint(hallwayDir.getOpposite(), this.orgin, 5, piecesFromCenter);
            }
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos s = this.getPosMin();
            BlockPos e = this.getPosMax();
            Block block;
            byte meta;
            int chunkCoordX, chunkCoordY, chunkCoordZ, offsetX, offsetY, offsetZ;
            for (int i = s.x; i <= e.x; i++) {
                for (int j = s.y; j <= e.y; j++) {
                    for (int k = s.z; k <= e.z; k++) {
                        if (chunk.isInChunk(i, j, k)) {
                            block = Block.air;
                            meta = 0;
                            chunkCoordX = i - chunk.pos.x;
                            chunkCoordY = j - chunk.pos.y;
                            chunkCoordZ = k - chunk.pos.z;
                            offsetX = i - this.orgin.x;
                            offsetY = j - this.orgin.y;
                            offsetZ = k - this.orgin.z;
                            chunk.setBlock(chunkCoordX, chunkCoordY, chunkCoordZ, block);
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

        public override void calculateBounds() {
            this.setPieceSize(0, 6, 5);
        }
    }
}
