using UnityEngine;

namespace VoxelEngine.TileEntity {

    public class ChestOpen : MonoBehaviour {

        private const float hingeSpeed = 1f;

        public Transform lid;
        // 1 = Opening, 0 = Closed, -1 = Closing
        private int lidState;

        public void Update() {
            if(!Main.singleton.isPaused) {
                if(this.lidState == 1 || this.lidState == -1) {
                    float f = Mathf.Lerp(this.lid.localEulerAngles.x, this.lidState == 1 ? 70 : 0, ChestOpen.hingeSpeed * Time.deltaTime);
                    if(f == 0) {
                        this.lidState = 0;
                    }
                    this.lid.localEulerAngles = new Vector3(f, 0, 0);
                }
            }
        }

        public void setOpen(bool open) {
            this.lidState = open ? 1 : -1;
        }
    }
}
