using UnityEngine;

namespace VoxelEngine.Util {

    public class Direction {

        public const int NONE_ID = 0;
        public const int NORTH_ID = 1;
        public const int EAST_ID = 2;
        public const int SOUTH_ID = 3;
        public const int WEST_ID = 4;
        public const int UP_ID = 5;
        public const int DOWN_ID = 6;

        public static Direction NONE =  new Direction(BlockPos.zero,  Vector3.zero,    "NONE",  EnumAxis.NONE, NONE_ID,  NONE_ID,  NONE_ID,  NONE_ID);
        /// <summary> +Z </summary>
        public static Direction NORTH = new Direction(BlockPos.north, Vector3.forward, "North", EnumAxis.Z,    SOUTH_ID, EAST_ID,  WEST_ID,  NORTH_ID);
        /// <summary> +X </summary>
        public static Direction EAST =  new Direction(BlockPos.east,  Vector3.right,   "East",  EnumAxis.X,    WEST_ID,  SOUTH_ID, NORTH_ID, EAST_ID);
        /// <summary> -Z </summary>
        public static Direction SOUTH = new Direction(BlockPos.south, Vector3.back,    "South", EnumAxis.Z,    NORTH_ID, WEST_ID,  EAST_ID,  SOUTH_ID);
        /// <summary> -X </summary>
        public static Direction WEST =  new Direction(BlockPos.west,  Vector3.left,    "West",  EnumAxis.X,    EAST_ID,  NORTH_ID, SOUTH_ID, WEST_ID);
        /// <summary> +Y </summary>
        public static Direction UP =    new Direction(BlockPos.up,    Vector3.up,      "Up",    EnumAxis.Y,    DOWN_ID,  UP_ID,    UP_ID,    UP_ID);
        /// <summary> -Y </summary>
        public static Direction DOWN =  new Direction(BlockPos.down,  Vector3.down,    "Down",  EnumAxis.Y,    UP_ID,    DOWN_ID,  DOWN_ID,  DOWN_ID);

        public static Direction[] yPlane = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        /// <summary> All of the normal direction, NESWUD. </summary>
        public static Direction[] all = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };
        public static Direction[] allIncludeNone = new Direction[] { Direction.NONE, Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST, Direction.UP, Direction.DOWN };

        public BlockPos direction;
        public Vector3 vector;
        public string name;
        public EnumAxis axis;
        private int oppositeIndex;
        private int clockwiseIndex;
        private int counterClockwiseIndex;
        public int index;

        private Direction(BlockPos pos, Vector3 vec, string name, EnumAxis axis, int opposite, int clockwise, int counterClockwise, int directionId) {
            this.direction = pos;
            this.vector = vec;
            this.name = name;
            this.axis = axis;
            this.oppositeIndex = opposite;
            this.clockwiseIndex = clockwise;
            this.counterClockwiseIndex = counterClockwise;
            this.index = directionId;
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

