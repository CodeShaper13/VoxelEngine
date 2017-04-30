namespace Assets.VoxelEngine.Level {

    public struct CellLightData {

        /// <summary>
        ///  0=outside of area.
        ///  1=inside.
        ///  2=edge.
        /// 
        ///  3=inside but light has been calculated.
        ///  4=edge but light has been calculated.
        /// </summary>
        public int state;
        public int light;
    }
}
