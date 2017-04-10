using UnityEngine;

namespace VoxelEngine.Render {

    public class HudCamera : MonoBehaviour {

        /// <summary> Reference to the attached camera </summary>
        public Camera orthoCamera;

        private void Awake() {
            if (RenderManager.instance.hudCamera == null) {
                RenderManager.instance.hudCamera = this;
                this.orthoCamera = this.GetComponent<Camera>();
            } else {
                Debug.Log("ERROR!  There are more than one game objects with HudCamera script!");
            }
        }

        public void bindToHudCamera(Canvas canvas) {
            canvas.transform.SetParent(this.orthoCamera.transform);
            canvas.worldCamera = this.orthoCamera;
        }
    }
}
