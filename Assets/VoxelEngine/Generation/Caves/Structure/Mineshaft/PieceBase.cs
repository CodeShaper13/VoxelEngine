using fNbt;
using System;
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
            return new BlockPos(this.pieceBounds.min);
        }

        public BlockPos getPosMax() {
            return new BlockPos(this.pieceBounds.max);
        }

        /// <summary>
        /// Standard gravel odds, 1 in 3 is gravel, rest is null.
        /// </summary>
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
        /// Same as setState() but with a safety check that the location is withing the passed chunk.
        /// If it is out of bounds nothing happens.
        /// </summary>
        protected void setStateIfInChunk(Chunk chunk, int x, int y, int z, Block block, int meta) {
            if (chunk.isInChunk(x, y, z)) {
                this.setState(chunk, x, y, z, block, meta);
            }
        }

        /// <summary>
        /// Sets the passed block and meta at the passed coords in the passed chunk.
        /// </summary>
        protected void setState(Chunk chunk, int x, int y, int z, Block block, int meta) {
            if(block != null) {
                if(block is BlockTileEntity) {
                    throw new Exception("Trying to place a TileEntity via Chunk.setBlock() from PieceBase.setState().  This will throw the game out of sync!");
                }
                int localX = x - chunk.worldPos.x;
                int localY = y - chunk.worldPos.y;
                int localZ = z - chunk.worldPos.z;
                chunk.setBlock(localX, localY, localZ, block);
                chunk.setMeta(localX, localY, localZ, meta);
            }
        }
        
        /// <summary>
        /// Generates a hallway with a chance to fail, returning true if one was made.
        /// </summary>
        protected bool generateSingleHallway(BlockPos pos, Direction direction, int piecesFromCenter) {
            if (this.shaft.rnd.Next(0, 6) > 1) { // 2 in 5 fail.
                return (new PieceHallway(this.shaft, pos, direction, piecesFromCenter)).successfullyGenerated;
            } else {
                return false;
            }
        }

        protected int generateHallwaysAroundPoint(Direction ignoreDirection, BlockPos orgin, int offsetRadius, int piecesFromCenter) {
            return this.generateHallwaysAroundPoint(ignoreDirection, orgin, offsetRadius, offsetRadius, offsetRadius, offsetRadius, piecesFromCenter);
        }

        /// <summary>
        /// Generates hallways around a centeral point, returning the directions that hallways generated in the form of a bit mask where 1 = generated in order of NESW.
        /// </summary>
        protected int generateHallwaysAroundPoint(Direction ignoreDirection, BlockPos floorPoint, int posX, int posZ, int negX, int negZ, int piecesFromCenter) {
            int bits = 0;
            if (ignoreDirection != Direction.EAST && this.generateSingleHallway(new BlockPos(floorPoint.x + posX, floorPoint.y, floorPoint.z), Direction.EAST, piecesFromCenter)) {
                bits |= 1;
            }
            if (ignoreDirection != Direction.WEST && this.generateSingleHallway(new BlockPos(floorPoint.x - negX, floorPoint.y, floorPoint.z), Direction.WEST, piecesFromCenter)) {
                bits |= 2;
            }
            if (ignoreDirection != Direction.NORTH && this.generateSingleHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z + posZ), Direction.NORTH, piecesFromCenter)) {
                bits |= 4;
            }
            if (ignoreDirection != Direction.SOUTH && this.generateSingleHallway(new BlockPos(floorPoint.x, floorPoint.y, floorPoint.z - negZ), Direction.SOUTH, piecesFromCenter)) {
                bits |= 8;
            }
            return bits;
        }

        protected void addTorch(Chunk chunk, int worldX, int worldY, int worldZ, Direction direction) {
            chunk.world.setBlock(worldX, worldY, worldZ, Block.torch, BlockTorch.getMetaFromDirection(direction), false, false);
        }
    }
}
