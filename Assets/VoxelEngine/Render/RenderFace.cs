namespace VoxelEngine.Render {

    /// <summary>
    /// Constants for renderFace bit masks.
    /// </summary>
    public static class RenderFace {

        public const int NONE = 0;
        public const int N = 1;
        public const int E = 2;
        public const int S = 4;
        public const int W = 8;
        public const int U = 16;
        public const int D = 32;
        /// <summary> The y plane, N E S W. </summary>
        public const int Y = N | E | S | W;
        /// <summary> The y plane, N E S W and up, U. </summary>
        public const int YU = Y | U;
        public const int ALL = 63;
    }
}
