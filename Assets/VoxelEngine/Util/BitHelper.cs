namespace VoxelEngine.Util {

    /// <summary>
    /// Helper class for working with bits.
    /// </summary>
    public static class BitHelper {

        /// <summary>
        /// Returns bit from integer at position bitNumber.
        /// </summary>
        public static bool getBit(int integer, int bitNumber) {
            return ((integer >> bitNumber) & 1) == 1;
        }

        /// <summary>
        /// Sets bit at position bitNumber to value, defaulting to 1.
        /// </summary>
        public static int setBit(int integer, int bitPosition, bool value) {
            if(value) {
                return (integer |= 1 << bitPosition);
            } else {
                return integer &= ~(1 << bitPosition);
            }
        }
    }
}
