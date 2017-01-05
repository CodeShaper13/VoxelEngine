using UnityEngine;

namespace VoxelEngine.Util {

    public class HudCamera : MonoBehaviour {
        public static new Camera camera;

        void Awake() {
            if (HudCamera.camera == null) {
                HudCamera.camera = this.GetComponent<Camera>();
            }
        }
    }
}
