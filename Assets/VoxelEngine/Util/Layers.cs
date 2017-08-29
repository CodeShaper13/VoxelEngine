namespace VoxelEngine.Util {

    /// <summary>
    /// List of all the Unity layers that are uesed.  Prevents typos and lets us search for uses of tags with Shift+F12.
    /// </summary>
    public static class Layers {

        // Built in.
        public const int DEFAULT = 0;
        public const int TRANSPARENT_FX = 1;
        public const int IGNORE_RAYCAST = (1 << 2);
        public const int WATER = (1 << 4);
        public const int UI = (1 << 5);
        // User Defined.
        public const int HOTBAR = (1 << 8);
        public const int ENTITY = (1 << 9);
        public const int ENTITY_ITEM = (1 << 10);
        public const int ISLAND_MESH = (1 << 11);
        public const int ENTITY_PLAYER = (1 << 12);
        public const int BLOCKS = (1 << 13);
    }
}
