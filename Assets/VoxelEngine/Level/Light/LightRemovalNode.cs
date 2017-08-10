namespace VoxelEngine.Level.Light {

    /// <summary>
    /// Used in calculating lighting.
    /// </summary>
    public struct LightRemovalNode {

        public int x;
        public int y;
        public int z;
        public int lightLevel;

        public LightRemovalNode(int x, int y, int z, int i) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.lightLevel = i;
        }
    }
}
