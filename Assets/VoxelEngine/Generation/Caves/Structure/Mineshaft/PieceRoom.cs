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
            : base(shaft, hallwayPoint + (hallwayDir.blockPos * 8)) {

            piecesFromCenter += 1;
            if (this.addToShaftIfValid(piecesFromCenter)) {
                this.generateHallwaysAroundPoint(hallwayDir.getOpposite(), this.orgin, 8, piecesFromCenter);
            }
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos s = this.getPosMin();
            BlockPos e = this.getPosMax();
            int offsetX, offsetY, offsetZ;
            for (int x = s.x; x <= e.x; x++) {
                for (int y = s.y; y <= e.y; y++) {
                    for (int z = s.z; z <= e.z; z++) {
                        if (chunk.isInChunk(x, y, z)) {
                            offsetX = x - this.orgin.x;
                            offsetY = y - this.orgin.y;
                            offsetZ = z - this.orgin.z;

                            this.setState(chunk, x, y, z, Block.air, 0);
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
