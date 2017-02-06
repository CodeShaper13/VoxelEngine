using UnityEngine;

namespace VoxelEngine.Util {

    public static class MathHelper {

        public static Vector3 absVec(Vector3 vec) {
            return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
        }
    }
}
