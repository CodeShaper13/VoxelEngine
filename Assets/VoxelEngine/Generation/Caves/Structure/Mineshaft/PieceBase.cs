using fNbt;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public abstract class PieceBase : IDebugDisplayable {

        /// <summary> References to the mineshaft that this belongs too. </summary>
        public StructureMineshaft shaft;

        public Bounds pieceBounds;
        protected BlockPos orgin;

        /// <summary>
        /// Ctor when loading a piece from the save.
        /// </summary>
        public PieceBase(NbtCompound tag) {
            this.orgin = NbtHelper.readDirectBlockPos(tag, "orgin");
        }

        /// <summary>
        /// Ctor when the piece is added from the generation classes.
        /// </summary>
        public PieceBase(StructureMineshaft shaft, BlockPos orgin) {
            this.shaft = shaft;
            this.orgin = orgin;
        }

        /// <summary>
        /// Called to actually place the blocks in the first generation phase. 
        /// </summary>
        public abstract void carvePiece(Chunk chunk, System.Random rnd);

        /// <summary>
        /// Sets the pieces bounds via this.setPieceSize()
        /// </summary>
        public abstract void calculateBounds();

        /// <summary>
        /// Called to return the piece's id.  Used in saving to disk.
        /// </summary>
        public abstract byte getPieceId();

        /// <summary>
        /// Returns the color for a piece to draw it's outline with.  Used for debugging.
        /// </summary>
        public abstract Color getPieceColor();

        /// <summary>
        /// Called to save the piece to disk.
        /// </summary>
        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtByte("id", this.getPieceId()));
            NbtHelper.writeDirectBlockPos(tag, this.orgin, "orgin");
            return tag;
        }

        /// <summary>
        /// Checks if this piece intersects with any others.
        /// </summary>
        public bool isIntersecting(List<PieceBase> pieces) {
            for(int i = 0; i < pieces.Count; i++) {
                if (this.pieceBounds.Intersects(pieces[i].pieceBounds)) {
                    return true;
                }
            }
            return false;
        }

        public void setPieceSize(int belowFloor, int aboveFloor, int radius) {
            this.setPieceSize(belowFloor, aboveFloor, radius, radius, radius, radius);
        }

        public void setPieceSize(int belowFloor, int aboveFloor, int posX, int negX, int posZ, int negZ) {
            this.pieceBounds = new Bounds();
            this.pieceBounds.SetMinMax(
                new Vector3(this.orgin.x - negX, this.orgin.y - belowFloor, this.orgin.z - negZ),
                new Vector3(this.orgin.x + posX, this.orgin.y + aboveFloor, this.orgin.z + posZ));
        }

        public BlockPos getPosMin() {
            return BlockPos.fromVec(this.pieceBounds.min);
        }

        public BlockPos getPosMax() {
            return BlockPos.fromVec(this.pieceBounds.max);
        }

        public Block rndGravel() {
            return this.shaft.rnd.Next(0, 3) == 0 ? Block.gravel : null;
        }

        public bool func(int piecesFromCenter) {
            this.calculateBounds();

            // The first piece coming out should never fail.
            if (piecesFromCenter == 1 || !this.isIntersecting(this.shaft.pieces)) {
                this.shaft.pieces.Add(this);
                if (piecesFromCenter > StructureMineshaft.SIZE_CAP) {
                    return false;
                }
            } else {
                return false;
            }

            return true;
        }

        public void debugDisplay() {
            DebugDrawer.bounds(this.pieceBounds, this.getPieceColor());
        }

        /// <summary>
        /// Sets the passed block and meta at the passed coords, if it's in the passed chunk.
        /// </summary>
        protected void setStateIfInChunk(Chunk chunk, int x, int y, int z, Block block, int meta) {
            if (chunk.isInChunk(x, y, z)) {
                this.setState(chunk, x, y, z, block, meta);
            }
        }

        protected void setState(Chunk chunk, int x, int y, int z, Block block, int meta) {
            if(block != null) {
                int i = x - chunk.pos.x;
                int j = y - chunk.pos.y;
                int k = z - chunk.pos.z;
                chunk.setBlock(i, j, k, block);
                chunk.setMeta(i, j, k, meta);
            }
        }
        
        /// <summary>
        /// Generates a hallway with a chance to fail.
        /// </summary>
        protected void generateSingleHallway(BlockPos pos, Direction direction, int piecesFromCenter) {
            if (this.shaft.rnd.Next(0, 6) > 1) { // 2 in 5 fail.
                new PieceHallway(this.shaft, pos, direction, piecesFromCenter);
            }
        }

        protected void generateHallwaysAroundPoint(Direction ignoreDirection, BlockPos orgin, int offsetRadius, int piecesFromCenter) {
            this.generateHallwaysAroundPoint(ignoreDirection, orgin, offsetRadius, offsetRadius, offsetRadius, offsetRadius, piecesFromCenter);
        }

        /// <summary>
        /// Generates hallways around a centeral point.
        /// </summary>
        protected void generateHallwaysAroundPoint(Direction ignoreDirection, BlockPos floorPoint, int posX, int posZ, int negX, int negZ, int piecesFromCenter) {
            if (ignoreDirection != Direction.EAST) {
                this.generateSingleHallway(new BlockPos(floorPoint.x + posX, floorPoint.y, floorPoint.z), Direction.EAST, piecesFromCenter);
            }
            if (ignoreDirection != Direction.WEST) {
                this.generateSingleHallway(new BlockPos(floorPoint.x - negX, floorPoint.y, floorPoint.z), Direction.WEST, piecesFromCenter);
            }
            if (ignoreDirection != Direction.NORTH) {
                this.generateSingleHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z + posZ), Direction.NORTH, piecesFromCenter);
            }
            if (ignoreDirection != Direction.SOUTH) {
                this.generateSingleHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z - negZ), Direction.SOUTH, piecesFromCenter);
            }
        }
    }
}
