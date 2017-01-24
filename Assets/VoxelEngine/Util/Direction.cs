namespace VoxelEngine.Util {

    public class Direction {
        private const int N = 1;
        private const int E = 2;
        private const int S = 3;
        private const int W = 4;
        private const int U = 5;
        private const int D = 6;

        public static Direction NONE = new Direction(BlockPos.zero, "none", EnumAxis.NONE, 0, 0, 0);
        public static Direction NORTH = new Direction(BlockPos.north, "north", EnumAxis.Z, S, E, W);
        public static Direction EAST = new Direction(BlockPos.east, "east", EnumAxis.X, W, S, N);
        public static Direction SOUTH = new Direction(BlockPos.south, "south", EnumAxis.Z, N, W, E);
        public static Direction WEST = new Direction(BlockPos.west, "west", EnumAxis.X, E, N, S);
        public static Direction UP = new Direction(BlockPos.up, "up", EnumAxis.Y, D, U, U);
        public static Direction DOWN = new Direction(BlockPos.down, "down", EnumAxis.Y, U, D, D);

        public static Direction[] xzPlane = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        public static Direction[] all = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };

        private static Direction[] interalAll = new Direction[] { Direction.NONE, Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };

        public BlockPos direction;
        public string name;
        public EnumAxis axis;
        private int oppositeIndex;
        private int clockwiseIndex;
        private int counterClockwiseIndex;

        public Direction(BlockPos pos, string name, EnumAxis axis, int opposite, int clockwise, int counterClockwise) {
            this.direction = pos;
            this.name = name;
            this.axis = axis;
            this.oppositeIndex = opposite;
            this.clockwiseIndex = clockwise;
            this.counterClockwiseIndex = counterClockwise;
        }

        public Direction getOpposite() {
            return Direction.interalAll[this.oppositeIndex];           
        }

        public Direction getClockwise() {
            return Direction.interalAll[this.clockwiseIndex];
        }

        public Direction getCounterClockwise() {
            return Direction.interalAll[this.counterClockwiseIndex];
        }

        public override string ToString() {
            return this.name;
        }

        public enum EnumAxis {
            NONE,
            X,
            Y,
            Z
        }
    }
}

