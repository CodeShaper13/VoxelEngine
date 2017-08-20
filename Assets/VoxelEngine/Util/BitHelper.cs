namespace VoxelEngine.Util {

    /// <summary>
    /// Helper class for working with bits.
    /// </summary>
    public static class BitHelper {

        /// <summary>
        /// Returns bit from integer at position bitNumber.
        /// </summary>
        public static int getBit(int integer, int bitNumber) {
            return ((integer >> bitNumber) & 1);
        }

        /// <summary>
        /// Sets bit at position bitNumber to value (defaults to 1/true).
        /// </summary>
        public static int setBit(int integer, int bitNumber, int value = 1) {
            return (integer |= value << bitNumber);
        }
    }
}
