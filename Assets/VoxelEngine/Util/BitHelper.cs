namespace VoxelEngine.Util {

    /// <summary>
    /// Helper class for working with bits.
    /// </summary>
    public static class BitHelper {

        /// <summary>
        /// Returns bit from integer at position bitPosition.
        /// </summary>
        public static bool getBit(int integer, int bitPosition) {
            return ((integer >> bitPosition) & 1) == 1;
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

        /// <summary>
        /// Inverts the bit at position bitNumber in integer.
        /// </summary>
        public static int invertBit(int integer, int bitPosition) {
            return BitHelper.setBit(integer, bitPosition, !BitHelper.getBit(integer, bitPosition));
        }
    }
}
