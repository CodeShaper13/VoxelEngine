namespace VoxelEngine.Util {

    public class Direction {
        public static Direction NONE = new Direction(BlockPos.zero, "none");
        public static Direction NORTH = new Direction(BlockPos.north, "north");
        public static Direction EAST = new Direction(BlockPos.east, "east");
        public static Direction SOUTH = new Direction(BlockPos.south, "south");
        public static Direction WEST = new Direction(BlockPos.west, "west");
        public static Direction UP = new Direction(BlockPos.up, "up");
        public static Direction DOWN = new Direction(BlockPos.down, "down");

        public static Direction[] xzPlane = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        public static Direction[] all = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };

        public BlockPos direction;
        public string name;

        public Direction(BlockPos pos, string name) {
            this.direction = pos;
            this.name = name;
        }

        public Direction getOpposite() {
            if (this == Direction.NORTH) {
                return Direction.SOUTH;
            }
            else if (this == Direction.EAST) {
                return Direction.WEST;
            }
            else if (this == Direction.SOUTH) {
                return Direction.NORTH;
            }
            else if (this == Direction.WEST) {
                return Direction.EAST;
            }
            else if (this == Direction.UP) {
                return Direction.DOWN;
            }
            else if (this == Direction.DOWN) {
                return Direction.UP;
            }
            else {
                return Direction.NONE;
            }
        }

        public override string ToString() {
            return this.name;
        }
    }
}

