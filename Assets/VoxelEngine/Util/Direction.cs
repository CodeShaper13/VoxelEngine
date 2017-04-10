namespace VoxelEngine.Util {

    public class Direction {

        public const int NONE_ID = 0;
        public const int NORTH_ID = 1;
        public const int EAST_ID = 2;
        public const int SOUTH_ID = 3;
        public const int WEST_ID = 4;
        public const int UP_ID = 5;
        public const int DOWN_ID = 6;

        public static Direction NONE =  new Direction(BlockPos.zero,  "NONE",  EnumAxis.NONE, NONE_ID, NONE_ID, NONE_ID, NONE_ID);
        public static Direction NORTH = new Direction(BlockPos.north, "North", EnumAxis.Z,    SOUTH_ID, EAST_ID, WEST_ID, NORTH_ID);
        public static Direction EAST =  new Direction(BlockPos.east,  "East",  EnumAxis.X,    WEST_ID, SOUTH_ID, NORTH_ID, EAST_ID);
        public static Direction SOUTH = new Direction(BlockPos.south, "South", EnumAxis.Z,    NORTH_ID, WEST_ID, EAST_ID, SOUTH_ID);
        public static Direction WEST =  new Direction(BlockPos.west,  "West",  EnumAxis.X,    EAST_ID, NORTH_ID, SOUTH_ID, WEST_ID);
        public static Direction UP =    new Direction(BlockPos.up,    "Up",    EnumAxis.Y,    DOWN_ID, UP_ID, UP_ID, UP_ID);
        public static Direction DOWN =  new Direction(BlockPos.down,  "Down",  EnumAxis.Y,    UP_ID, DOWN_ID, DOWN_ID, DOWN_ID);

        public static Direction[] yPlane = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        public static Direction[] all = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };
        public static Direction[] allIncludeNone = new Direction[] { Direction.NONE, Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };

        public BlockPos direction;
        public string name;
        public EnumAxis axis;
        private int oppositeIndex;
        private int clockwiseIndex;
        private int counterClockwiseIndex;
        public int directionId;

        public Direction(BlockPos pos, string name, EnumAxis axis, int opposite, int clockwise, int counterClockwise, int directionId) {
            this.direction = pos;
            this.name = name;
            this.axis = axis;
            this.oppositeIndex = opposite;
            this.clockwiseIndex = clockwise;
            this.counterClockwiseIndex = counterClockwise;
            this.directionId = directionId;
        }

        public Direction getOpposite() {
            return Direction.allIncludeNone[this.oppositeIndex];           
        }

        public Direction getClockwise() {
            return Direction.allIncludeNone[this.clockwiseIndex];
        }

        public Direction getCounterClockwise() {
            return Direction.allIncludeNone[this.counterClockwiseIndex];
        }

        public override string ToString() {
            return this.name;
        }
    }
}

