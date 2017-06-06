using UnityEngine;

namespace VoxelEngine.Util {

    /// <summary>
    /// A struct with a combined position, rotation and scale for a GameObject.
    /// </summary>
    public struct MutableTransform {

        public static MutableTransform DEFAULT = new MutableTransform(Vector3.zero, Quaternion.identity, Vector3.one);

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public MutableTransform(Vector3 position, Quaternion rotation, Vector3 scale) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        /// <summary>
        /// Converts the MutableTransform to a Matrix4x4 and returns it.
        /// </summary>
        public Matrix4x4 toMatrix4x4() {
            return Matrix4x4.TRS(this.position, this.rotation, this.scale);
        }
    }
}
