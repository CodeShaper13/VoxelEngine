namespace VoxelEngine.Util {

    public class DirectionNeighbors {

        
        public static Direction[] getNeighbors(Direction direction) {
            if(direction == Direction.NORTH || direction == Direction.SOUTH) {
                return new Direction[] {Direction.UP, Direction.EAST, Direction.DOWN, Direction.WEST};
            }
            if (direction == Direction.EAST || direction == Direction.WEST) {
                return new Direction[] { Direction.UP, Direction.NORTH, Direction.DOWN, Direction.SOUTH };
            }
            if (direction == Direction.UP || direction == Direction.DOWN) {
                return new Direction[] { Direction.NORTH, Direction.EAST, Direction.WEST, Direction.SOUTH };
            }
            return null;
        }
    }
}
