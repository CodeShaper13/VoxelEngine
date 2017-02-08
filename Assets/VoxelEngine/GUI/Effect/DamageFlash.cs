using UnityEngine;
using UnityEngine.UI;

namespace VoxelEngine.GUI.Effect {

    public class DamageFlash : MonoBehaviour {

        public Image image;
        private int timer;
        private bool flag;

        public void Update() {
            if(this.flag) {
                float a = this.image.color.a - Time.deltaTime * 1.5f; // Fade speed
                if(a <= 0) {
                    this.clearEffect();
                }
                this.setAlpha(a);
            }
        }

        public void startEffect() {
            this.flag = true;
            this.setAlpha(0.75f);
        }

        public void clearEffect() {
            this.flag = false;
            this.setAlpha(0);
        }

        private void setAlpha(float a) {
            this.image.color = new Color(this.image.color.r, this.image.color.g, this.image.color.b, a);
        }
    }
}
