using UnityEngine;

namespace VoxelEngine.GUI.Effect {

    /// <summary>
    /// Rotates the gears on the title screen.
    /// </summary>
    public class GearRotate : MonoBehaviour {

        public float currentSpeed;
        public float accelerateSpeed;
        public float maxSpeed;
        public bool countClockwise;

        private void Update() {
            if(this.currentSpeed <= this.maxSpeed) {
                this.currentSpeed += (this.accelerateSpeed * Time.deltaTime);
                this.currentSpeed = Mathf.Clamp(this.currentSpeed, -1000, this.maxSpeed); // Don't let it go over!
            }

            if(this.currentSpeed > 0) {
                this.transform.Rotate(0, 0, this.currentSpeed * Time.deltaTime * (this.countClockwise ? -1 : 1));
            }
        }
    }
}
