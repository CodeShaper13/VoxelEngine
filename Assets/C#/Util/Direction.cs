//Represents a direction
public class Direction {

    public static Direction NORTH = new Direction(BlockPos.north, Direction.SOUTH);
    public static Direction EAST = new Direction(BlockPos.east, Direction.WEST);
    public static Direction SOUTH = new Direction(BlockPos.south, Direction.NORTH);
    public static Direction WEST = new Direction(BlockPos.west, Direction.EAST);
    public static Direction UP = new Direction(BlockPos.up, Direction.DOWN);
    public static Direction DOWN = new Direction(BlockPos.down, Direction.UP);

    public static Direction[] xzPlane = new Direction[] {Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST};
    public static Direction[] all = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN};

    public BlockPos direction;
    public Direction opposite;

    public Direction(BlockPos pos, Direction opposite) {
        this.direction = pos;
        this.opposite = opposite;
    }
}

