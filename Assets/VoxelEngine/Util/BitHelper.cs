namespace VoxelEngine.Util {

    /// <summary>
    /// Helper class for working with bits.
    /// </summary>
    public static class BitHelper {

        /// <summary>
        /// Returns bit from i at position bitNumber.
        /// </summary>
        public static int getBit(int i, int bitNumber) {
            return ((i >> bitNumber) & 1);
        }

        /// <summary>
        /// Sets bit at position bitNumber to value (defualts to 1).
        /// </summary>
        public static int setBit(int i, int bitNumber, int value = 1) {
            i |= value << bitNumber;
            return i;
        }
    }
}
