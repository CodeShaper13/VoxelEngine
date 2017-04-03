using System.Collections.Generic;
using fNbt;
using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public abstract class PieceIntersection : PieceBase {

        protected BlockPos sizeRadius;

        public PieceIntersection(NbtCompound tag) : base(tag) {

        }

        public PieceIntersection(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd) : base(BlockPos.zero) { // We set this to the correct value in two lines
            this.sizeRadius = this.getSizeRadius(rnd);
            this.orgin = hallwayPoint + (hallwayDir.direction * (hallwayDir.axis == EnumAxis.X ? this.sizeRadius.x : this.sizeRadius.z));

            this.calculateBounds();

            if (this.isIntersecting(pieces)) {
                return;
            } else {
                pieces.Add(this);
            }

            piecesFromStart++;
            if (piecesFromStart > StructureMineshaft.SIZE_CAP) {
                return;
            }

            this.generateBranches(hallwayDir.getOpposite(), this.orgin, pieces, piecesFromStart, rnd);
        }

        public override void calculateBounds() {
            this.pieceBounds = new Bounds(
                new Vector3(this.orgin.x, this.orgin.y + (this.getHeight() - 0.5f) / 2, this.orgin.z),
                new Vector3(this.sizeRadius.x * 2, this.getHeight() + 1, this.sizeRadius.z * 2));
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            NbtHelper.writeDirectBlockPos(tag, this.orgin, "orgin");
            return tag;
        }

        protected void generateBranches(Direction d, BlockPos floorPoint, List<PieceBase> pieces, int piecesFromStart, System.Random rnd) {
            if (d != Direction.EAST) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x + this.sizeRadius.x + 1, floorPoint.y, floorPoint.z), Direction.EAST, pieces, piecesFromStart, d, rnd);
            }
            if (d != Direction.WEST) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x - this.sizeRadius.x - 1, floorPoint.y, floorPoint.z), Direction.WEST, pieces, piecesFromStart, d, rnd);
            }
            if (d != Direction.NORTH) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z + this.sizeRadius.z + 1), Direction.NORTH, pieces, piecesFromStart, d, rnd);
            }
            if (d != Direction.SOUTH) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z - this.sizeRadius.z - 1), Direction.SOUTH, pieces, piecesFromStart, d, rnd);
            }
        }

        protected abstract BlockPos getSizeRadius(System.Random rnd);

        protected abstract int getHeight();
    }
}