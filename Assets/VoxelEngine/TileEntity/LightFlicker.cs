using UnityEngine;

namespace VoxelEngine.TileEntity {

    public class LightFlicker : MonoBehaviour {

        public Light lightObj;
        public float minIntensity = 1f;
        public float maxIntensity = 2f;
        public float flickerSpeed = 1f;

        public void Update() {
            if(!Main.singleton.isPaused) {
                this.lightObj.intensity = Mathf.PingPong(Time.time * this.flickerSpeed, this.maxIntensity - this.minIntensity) + this.minIntensity;
            }
        }
    }
}
