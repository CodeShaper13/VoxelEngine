using fNbt;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public abstract class PieceBase : IDebugDisplayable {

        public Bounds pieceBounds;
        protected BlockPos orgin;

        public PieceBase(NbtCompound tag) {
            this.orgin = NbtHelper.readDirectBlockPos(tag, "orgin");
        }

        public PieceBase(BlockPos orgin) {
            this.orgin = orgin;
        }

        public abstract void carvePiece(Chunk chunk, System.Random rnd);

        // Sets this.pieceBounds with the correct bounds
        public abstract void calculateBounds();

        public abstract byte getPieceId();

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtByte("id", this.getPieceId()));
            NbtHelper.writeDirectBlockPos(tag, this.orgin, "orgin");
            return tag;
        }

        public abstract Color getPieceColor();

        public bool isIntersecting(List<PieceBase> pieces) {
            foreach (PieceBase p in pieces) {
                if (this.pieceBounds.Intersects(p.pieceBounds)) {
                    return true;
                }
            }
            return false;
        }

        public void debugDisplay() {
            DebugDrawer.bounds(this.pieceBounds, this.getPieceColor());
        }

        // Standard hallway odds
        protected void tryGenerateHallway(BlockPos pos, Direction direction, List<PieceBase> pieces, int chanceToFail, Direction comingFrom, System.Random rnd) {
            if (rnd.Next(0, 6) > 1) { //4 out of 5
                new PieceHallway(pos, direction, pieces, chanceToFail, rnd);
            }
        }

        protected void generateHallways(HallwayStart[] hallwayPoints, List<PieceBase> pieces, int chanceToFail, System.Random rnd) {
            HallwayStart hs;
            for(int i = 0; i < hallwayPoints.Length; i++) {
                hs = hallwayPoints[i];
                this.tryGenerateHallway(hs.position, hs.direction, pieces, chanceToFail, hs.direction, rnd);
            }
        }

        // Generates hallways around a centeral point
        protected void generateHallways(Direction ignoreDirection, BlockPos floorPoint, int offsetX, int offsetZ, List<PieceBase> pieces, int piecesFromStart, System.Random rnd) {
            if (ignoreDirection != Direction.EAST) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x + offsetX, floorPoint.y, floorPoint.z), Direction.EAST, pieces, piecesFromStart, ignoreDirection, rnd);
            }
            if (ignoreDirection != Direction.WEST) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x - offsetX, floorPoint.y, floorPoint.z), Direction.WEST, pieces, piecesFromStart, ignoreDirection, rnd);
            }
            if (ignoreDirection != Direction.NORTH) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z + offsetZ), Direction.NORTH, pieces, piecesFromStart, ignoreDirection, rnd);
            }
            if (ignoreDirection != Direction.SOUTH) {
                this.tryGenerateHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z - offsetZ), Direction.SOUTH, pieces, piecesFromStart, ignoreDirection, rnd);
            }
        }
    }
}
