using UnityEngine;

namespace VoxelEngine.Util {

    /// <summary>
    /// Static helper math functions for numbers and vectors.
    /// </summary>
    public static class MathHelper {

        /// <summary>
        /// Returns a vector with the absolute values of the passed Vector3.
        /// </summary>
        public static Vector3 absVec(Vector3 vec) {
            return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
        }

        /// <summary>
        /// Rotates a vector around a pivot and returns it.
        /// </summary>
        public static Vector3 rotateVecAround(Vector3 point, Vector3 pivot, Quaternion angle) {
            if(angle == new Quaternion(0, 0, 0, 1)) {
                return point;
            }
            return angle * (point - pivot) + pivot;
        }

        /// <summary>
        /// Converts the passed pixel length to world space.  Used in block models.
        /// </summary>
        public static float pixelToWorld(float f) {
            return (1f / 32f) * f;
        }

        /// <summary>
        /// Rounds all components of the passed vector to integers and returns it.
        /// </summary>
        public static Vector3 roundVector3(Vector3 pos) {
            return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
        }

        /// <summary>
        /// Clamps an integer between two values.
        /// </summary>
        public static int clamp(int value, int min, int max) {
            if(value < min) {
                return min;
            } else if(value > max) {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Faster version of Mathf.floor().
        /// </summary>
        public static int floor(float value) {
            int i = (int)value;
            return value < i ? i - 1 : i;
        }

        /// <summary>
        /// Takes in the angle passed to Block.isValidPlaceLocation() or Block.adjustMetaOnPlace()
        /// and returns it as a direction.
        public static Direction angleToDirection(Vector3 angle) {
            if (Mathf.Abs(angle.x) > Mathf.Abs(angle.z)) { // X aixs
                return (angle.x > 0) ? Direction.EAST : Direction.WEST;
            } else { // Z axis
                return (angle.z > 0) ? Direction.NORTH : Direction.SOUTH;
            }
        }

        /// <summary>
        /// Increases the passed number and returns it, cycling it back down to the start if it
        /// passed blockSize.
        /// Example:
        /// MathHelper.scrollInt(3, 4) -> 0
        /// </summary>
        public static int scrollInt(int number, int blockSize) {
            number += 1;
            if(number > blockSize) {
                number -= blockSize;
            }
            return number;
        }

        ///
        public static int roundAwayFrom0(float f) {
            return f < 0 ? MathHelper.floor(f) : Mathf.CeilToInt(f);
        }
    }
}
