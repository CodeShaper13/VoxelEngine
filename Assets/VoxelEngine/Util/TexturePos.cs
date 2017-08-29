namespace VoxelEngine.Util {

    public struct TexturePos {

        public const float BLOCK_SIZE = 0.0625f;
        public const float ITEM_SIZE = 0.0625f;
        /// <summary> Size of a single pixel </summary>
        public const float PIXEL_SIZE = ITEM_SIZE / 32;

        public int x;
        public int y;
        public int rotation;

        public TexturePos(int x, int y) {
            this.x = x;
            this.y = y;
            this.rotation = 0;
        }

        /// <summary>
        /// Rotation is in amounts of 90 degrees.
        /// </summary>
        public TexturePos(int x, int y, int rotation) {
            this.x = x;
            this.y = y;
            this.rotation = rotation;
        }
    }
}
