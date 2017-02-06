using fNbt;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public abstract class PieceBase : IDebugDisplayable {

        public Bounds pieceBounds;

        public abstract void carvePiece(Chunk chunk, System.Random rnd);

        public abstract void calculateBounds();

        public abstract byte getPieceId();

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtByte("id", this.getPieceId()));
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

        protected void setIfInChunk(Chunk chunk, int x, int y, int z, Block block) {
            int i = x - chunk.pos.x;
            int j = y - chunk.pos.y;
            int k = z - chunk.pos.z;
            if (i >= 0 && i < Chunk.SIZE && j >= 0 && j < Chunk.SIZE && k >= 0 && k < Chunk.SIZE) {
                chunk.setBlock(i, j, k, block);
            }
        }

        protected void setStateIfInChunk(Chunk chunk, int x, int y, int z, Block block, byte meta) {
            int i = x - chunk.pos.x;
            int j = y - chunk.pos.y;
            int k = z - chunk.pos.z;
            if (i >= 0 && i < Chunk.SIZE && j >= 0 && j < Chunk.SIZE && k >= 0 && k < Chunk.SIZE) {
                chunk.setBlock(i, j, k, block);
                chunk.setMeta(i, j, k, meta);
            }
        }

    }
}
