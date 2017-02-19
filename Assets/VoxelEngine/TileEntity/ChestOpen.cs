using UnityEngine;

namespace VoxelEngine.TileEntity {

    public class ChestOpen : MonoBehaviour {

        private float hingeSpeed = 1f;

        public Transform lid;
        private int i;

        public void Update() {
            if(!Main.singleton.isPaused) {
                if(this.i == 1) { // Open
                    this.lid.eulerAngles = new Vector3(Mathf.Lerp(this.lid.localEulerAngles.x, 70, 1 * Time.deltaTime), 0, 0);
                } else if(this.i == -1) { // Close
                    this.lid.eulerAngles = new Vector3(Mathf.Lerp(this.lid.localEulerAngles.x, 0, 1 * Time.deltaTime), 0, 0);
                }
            }
        }

        public void setOpen(bool open) {
            this.i = open ? 1 : -1;
        }
    }
}
