using UnityEngine;

namespace VoxelEngine.Render {

    /// <summary>
    /// Represents a rotation for a block model cube.
    /// </summary>
    public struct ComponentRotation {

        private float x;
        private float y;
        private float z;

        public ComponentRotation(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Checks if the angle is 0, 0, 0.
        /// </summary>
        public bool isZero() {
            return this.x == 0 && this.y == 0 && this.z == 0;
        }

        /// <summary>
        /// Returns the angle as a Quaternion.
        /// </summary>
        public Quaternion getAngle() {
            return Quaternion.Euler(this.x, this.y, this.z);
        }
    }
}
