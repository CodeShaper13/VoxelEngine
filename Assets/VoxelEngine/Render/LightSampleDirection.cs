using VoxelEngine.Util;

namespace VoxelEngine.Render {

    /// <summary>
    /// Constants for light sampling.
    /// </summary>
    public static class LightSampleDirection {
        
        public const int SELF = Direction.NONE_ID;
        public const int NORTH = Direction.NORTH_ID;
        public const int EAST = Direction.EAST_ID;
        public const int SOUTH = Direction.SOUTH_ID;
        public const int WEST = Direction.WEST_ID;
        public const int UP = Direction.UP_ID;
        public const int DOWN = Direction.DOWN_ID;
    }
}
