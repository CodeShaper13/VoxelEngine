using fNbt;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceMobSpawner : PieceIntersection {

        public bool northGate;
        public bool eastGate;
        public bool southGate;
        public bool westGate;

        public PieceMobSpawner(NbtCompound tag) : base(tag) {
            this.sizeRadius = this.getSizeRadius(null); //This doesnt use the rnd param
            this.northGate = tag.Get<NbtByte>("ng").ByteValue == 1;
            this.eastGate = tag.Get<NbtByte>("eg").ByteValue == 1;
            this.southGate = tag.Get<NbtByte>("sg").ByteValue == 1;
            this.westGate = tag.Get<NbtByte>("wg").ByteValue == 1;
        }

        public PieceMobSpawner(BlockPos hallwayPoint, Direction hallwayDir, List<PieceBase> pieces, int piecesFromStart, System.Random rnd)
            : base(hallwayPoint, hallwayDir, pieces, piecesFromStart, rnd) {
            this.northGate = rnd.Next(2) == 0;
            this.eastGate = rnd.Next(2) == 0;
            this.southGate = rnd.Next(2) == 0;
            this.westGate = rnd.Next(2) == 0;
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos s = new BlockPos(this.orgin.x - this.sizeRadius.x, this.orgin.y, this.orgin.z - this.sizeRadius.x);
            BlockPos e = new BlockPos(this.orgin.x + this.sizeRadius.z, this.orgin.y + 6, this.orgin.z + this.sizeRadius.z);
            Block b;
            for (int i = s.x; i <= e.x; i++) {
                for (int j = s.y; j <= e.y; j++) {
                    for (int k = s.z; k <= e.z; k++) {
                        b = Block.air;
                        int x = i - chunk.pos.x;
                        int y = j - chunk.pos.y;
                        int z = k - chunk.pos.z;
                        if (x >= 0 && x < Chunk.SIZE && y >= 0 && y < Chunk.SIZE && z >= 0 && z < Chunk.SIZE) {
                            if (i == this.orgin.x && k == this.orgin.z && y == 1) {
                                b = Block.placeholder;
                            }

                            if(true) { // (this.northGate) {
                                int i1 = this.orgin.z + 3;
                                int i2 = this.orgin.x + 2;
                                int i3 = this.orgin.z + 3;
                                if(z == i1 && x >= i2 && x <= i3) {
                                    b = this.rndGate(rnd);
                                }
                            }
                            if(this.eastGate) {

                            }
                            if(this.southGate) {

                            }
                            if(this.westGate) {

                            }

                            chunk.setBlock(x, y, z, b);
                        }
                    }
                }
            }
        }

        public override Color getPieceColor() {
            return Color.black;
        }

        public override byte getPieceId() {
            return 5;
        }

        protected override int getHeight() {
            return 6;
        }

        protected override BlockPos getSizeRadius(System.Random rnd) {
            return new BlockPos(3, 0, 3);
        }

        private Block rndGate(System.Random rnd) {
            if (rnd.Next(5) != 0) {
                return Block.fence;
            }
            return Block.air;
        }

        private int getMobId(System.Random rnd) {
            return 0;
        }
    }
}
