namespace VoxelEngine.Util {

    public struct TexturePos {

        public const float ATLAS_TILE_SIZE = 0.0625f;
        /// <summary> Size of a single pixel on the atlas. </summary>
        public const float PIXEL_SIZE = ATLAS_TILE_SIZE / 32;

        public const int MIRROR_X = 1;
        public const int MIRROR_Y = 2;
        public const int MIRROR_XY = MIRROR_X | MIRROR_Y; // 3

        public int x;
        public int y;
        public int rotation;
        /// <summary> First bit is for x mirror, second is for y mirror. </summary>
        public int mirrorFlags; 

        public TexturePos(int x, int y) {
            this.x = x;
            this.y = y;
            this.rotation = 0;
            this.mirrorFlags = 0;
        }

        /// <summary>
        /// Rotation is in amounts of 90 degrees.
        /// </summary>
        public TexturePos(int x, int y, int rotation) {
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.mirrorFlags = 0;
        }

        public TexturePos(int x, int y, int rotation, int mirrorFlags) {
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.mirrorFlags = mirrorFlags;
        }
    }
}
