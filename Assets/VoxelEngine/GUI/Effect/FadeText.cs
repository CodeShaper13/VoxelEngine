using UnityEngine;
using UnityEngine.UI;

namespace VoxelEngine.GUI.Effect {

    public class FadeText : MonoBehaviour {

        public Text text;
        private float timer;
        private Color originalColor;

        public void Awake() {
            this.originalColor = this.text.color;
        }

        public void Update() {
            if (this.timer > 0) {
                this.timer -= Time.deltaTime;
            }
            if (this.timer <= 0) {
                this.text.color = Color.Lerp(this.text.color, Color.clear, 3 * Time.deltaTime);
            }
        }

        public void showAndStartFade(string s, float time) {
            this.text.color = this.originalColor;
            this.text.text = s;
            this.timer = time;
        }
    }
}
