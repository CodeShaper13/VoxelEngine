using VoxelEngine.Util;

namespace VoxelEngine.Generation.Caves.Structure.Mineshaft {

    public struct HallwayStart {

        public BlockPos position;
        public Direction direction;

        public HallwayStart(BlockPos pos, Direction dir) {
            this.position = pos;
            this.direction = dir;
        }
    }
}
