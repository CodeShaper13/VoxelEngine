//Represents a direction
public class Direction {

    public static Direction NORTH = new Direction(new BlockPos(0, 0, 1), Direction.SOUTH);
    public static Direction EAST = new Direction(new BlockPos(1, 0, 0), Direction.WEST);
    public static Direction SOUTH = new Direction(new BlockPos(0, 0, -1), Direction.NORTH);
    public static Direction WEST = new Direction(new BlockPos(-1, 0, 0), Direction.EAST);
    public static Direction UP = new Direction(new BlockPos(0, 1, 0), Direction.DOWN);
    public static Direction DOWN = new Direction(new BlockPos(0, -1, 0), Direction.UP);

    public static Direction[] xzPlane = new Direction[] {Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST};
    public static Direction[] all = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN};

    public BlockPos direction;
    public Direction opposite;

    public Direction(BlockPos pos, Direction opposite) {
        this.direction = pos;
        this.opposite = opposite;
    }
}

