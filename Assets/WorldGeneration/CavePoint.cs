using UnityEngine;
using VoxelEngine.Util;

namespace WorldGeneration {

    public class CavePoint : IDebugDisplayable {

        public Vector3 orgin;
        public Vector3 direction;

        public CavePoint(Vector3 orgin, Vector3 direction) {
            this.orgin = orgin;
            this.direction = direction;
        }

        public void debugDisplay() {
            Debug.DrawLine(this.orgin, this.orgin + this.direction, Color.blue);
        }
    }
}
