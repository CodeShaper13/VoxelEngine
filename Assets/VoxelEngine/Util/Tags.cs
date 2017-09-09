namespace VoxelEngine.Util {

    /// <summary>
    /// List of all the Unity tags that are uesed.  Prevents typos and lets us search for uses of tags with Shift+F12.
    /// </summary>
    public static class Tags {

        public const string ENTITY = "Entity";
        public const string CHUNK = "Chunk";
        /// <summary> Used on tile entities that have collider components that are not baked into chunks. </summary>
        public const string BLOCK = "Block";
    }
}
