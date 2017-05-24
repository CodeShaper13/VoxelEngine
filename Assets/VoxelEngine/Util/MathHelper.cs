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
            if(angle == Quaternion.identity) {
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
        /// Rounds all components of the passed vector and returns it.
        /// </summary>
        public static Vector3 roundVector3(Vector3 pos) {
            return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
        }

        public static int clamp(int value, int min, int max) {
            if(value < min) {
                return min;
            } else if(value > max) {
                return max;
            }
            return value;
        }
    }
}
