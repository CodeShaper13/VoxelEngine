namespace VoxelEngine.Util {

    public struct TexturePos {

        public const float BLOCK_SIZE = 0.0625f; //size of the texture sheet
        /// <summary> Size of an item tile, 32x32 pixel section. </summary>
        public const float ITEM_SIZE = 0.0625f;
        /// <summary> Size of a single pixel </summary>
        public const float ITEM_PIXEL_SIZE = ITEM_SIZE / 32;

        public int x;
        public int y;

        public TexturePos(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
}
