using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Generation.Caves.Structure.Mineshaft.Center;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public class PieceCenter : PieceBase {

        // 0 = Storage, 1 = Bedroom
        private int topFloor;
        private int bottomFloor;
        private bool useFartherEntrance;

        public PieceCenter(NbtCompound tag) : base(tag) {
            this.topFloor = tag.Get<NbtInt>("topFloor").IntValue;
            this.bottomFloor = tag.Get<NbtInt>("bottomFloor").IntValue;
            this.useFartherEntrance = tag.Get<NbtByte>("useFarEntrance").ByteValue == 1;
        }

        public PieceCenter(StructureMineshaft shaft, BlockPos center) : base(shaft, center) {                  
            // Get random floors
            if(this.shaft.rnd.Next(2) == 1) {
                this.topFloor = 0;
                this.bottomFloor = 1;
            } else {
                this.topFloor = 1;
                this.bottomFloor = 0;
            }

            // Pick entrance to use.
            this.useFartherEntrance = this.shaft.rnd.Next(2) == 0;

            this.calculateBounds();
            this.shaft.pieces.Add(this);

            BlockPos shaftOrgin = new BlockPos(this.orgin.x - 8, this.orgin.y, this.orgin.z);

            // Add starting rooms above and below.
            this.shaft.pieces.Add(new PieceSmallShaft(this.shaft, shaftOrgin, 9, false));
            this.shaft.pieces.Add(new PieceSmallShaft(this.shaft, shaftOrgin + new BlockPos(0, 9, 0), 6, false));
            this.shaft.pieces.Add(new PieceSmallShaft(this.shaft, shaftOrgin + new BlockPos(0, -6, 0), 6, true));

            this.func(shaftOrgin + new BlockPos(0, 9, 0));
            this.func(shaftOrgin + new BlockPos(0, -6, 0));

            // Add hallways.
            new PieceHallway(this.shaft, this.orgin + new BlockPos(-5, 1, -8), Direction.WEST, 0);
            new PieceHallway(this.shaft, this.orgin + new BlockPos(5, 1, this.useFartherEntrance ? 0 : -8), Direction.EAST, 0);
            new PieceHallway(this.shaft, this.orgin + new BlockPos(0, 1, 5), Direction.NORTH, 0);
            new PieceHallway(this.shaft, this.orgin + new BlockPos(0, 1, -15), Direction.SOUTH, 0);
        }

        public override void carvePiece(Chunk chunk, System.Random rnd) {
            BlockPos p1 = this.getPosMin();
            BlockPos p2 = this.getPosMax();
            int offsetX, offsetY, offsetZ;
            Block block;
            byte meta;
            for (int x = p1.x; x <= p2.x; x++) {
                for (int y = p1.y; y <= p2.y; y++) {
                    for (int z = p1.z; z <= p2.z; z++) {
                        if(chunk.isInChunk(x, y, z)) {
                            block = null;
                            meta = 0;
                            offsetX = x - this.orgin.x;
                            offsetY = y - this.orgin.y;
                            offsetZ = z - this.orgin.z;

                            // Floor
                            if(offsetY == 0) {
                                block = this.rndGravel();
                            }
                            // Columns
                            else if(offsetY < 5 && (
                                (offsetX == 3 && offsetZ == 3) ||
                                (offsetX == -3 && offsetZ == 3) ||
                                (offsetX == 3 && offsetZ == -4) ||
                                (offsetX == -3 && offsetZ == -4) ||
                                (offsetX == 3 && offsetZ == -11) ||
                                (offsetX == -3 && offsetZ == -11) ||
                                (offsetX == 3 && offsetZ == -13) ||
                                (offsetX == -3 && offsetZ == -13))) {
                                    block = Block.wood;
                                    meta = 1;
                            }
                            // Crosspiece lower
                            else if(offsetY == 5 && Mathf.Abs(offsetX) < 5 && (
                                offsetZ == 3 || offsetZ == -4 || offsetZ == -11 || offsetZ ==-13)) {
                                    block = Block.wood;
                                    meta = 0;
                            }
                            // Higher
                            else if (offsetY == 6 && Mathf.Abs(offsetX) == 3) {
                                block = Block.wood;
                                meta = 2;
                            }
                            // Random chest
                            else if(offsetZ == -12 && offsetY == 1 && (offsetX == 3 || offsetX == -3) && rnd.Next(0, 1) == 0) {
                                RandomChest.MINESHAFT_START_ROOM.makeChest(chunk.world, x, y, z, offsetX > 0 ? Direction.WEST : Direction.EAST, rnd);
                            }
                            else {
                                block = Block.air;
                            }

                            this.setState(chunk, x, y, z, block, meta);
                        }
                    }
                }
            }
        }

        public override void calculateBounds() {
            this.setPieceSize(0, 6, 4, 4, 4, 14);
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            NbtHelper.writeDirectBlockPos(tag, this.orgin, "center");
            tag.Add(new NbtInt("topFloor", this.topFloor));
            tag.Add(new NbtInt("bottomFloor", this.bottomFloor));
            tag.Add(new NbtByte("useFarEntrance", this.useFartherEntrance ? (byte)1 : (byte)0));
            return tag;
        }

        public override byte getPieceId() {
            return 0;
        }

        public override Color getPieceColor() {
            return Color.white;
        }

        private void func(BlockPos pos) {
            if(this.shaft.rnd.Next(0, 2) == 0) {
                this.shaft.pieces.Add(new PieceOrginStorage(this.shaft, pos));
            } else {
                this.shaft.pieces.Add(new PieceOrginBedroom(this.shaft, pos));
            }
        }
    }
}
