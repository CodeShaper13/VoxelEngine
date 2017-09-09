using UnityEngine;

namespace VoxelEngine.Generation.Caves.Structure {

    /// <summary>
    /// Faster intersection testing replacement for UnityEngine.Bounds.  Not yet implemented.
    /// </summary>
    public struct StructureBounds {

        public Vector3 min;
        public Vector3 max;

        public StructureBounds(Vector3 min, Vector3 max) {
            this.min = min;
            this.max = max;
        }

        public bool intersects(StructureBounds otherBounds) {
            if (this.max.x < otherBounds.min.x) return false;
            if (this.min.x > otherBounds.max.x) return false;
            if (this.max.y < otherBounds.min.y) return false;
            if (this.min.y > otherBounds.max.y) return false;
            if (this.max.z < otherBounds.min.z) return false;
            if (this.min.z > otherBounds.max.z) return false;
            return true; // boxes overlap
        }
    }
}
