using UnityEngine;
using UnityEngine.UI;

namespace VoxelEngine.GUI.Effect {

    /// <summary>
    /// Fades the text in on the title screen.
    /// </summary>
    public class RevealText : MonoBehaviour {

        public float revealSpeed;

        private Image image;
        private Text text;
        private Color targetColor;
        private float timeSinceAwake;

        private void Awake() {
            this.image = this.GetComponent<Image>();
            this.text = this.GetComponentInChildren<Text>(); // Assume this is attached to the text.
            this.targetColor = this.text.color;
            this.image.color -= new Color(0, 0, 0, 1);
            this.text.color -= new Color(0, 0, 0, 1);
        }

        private void Update() {
            this.timeSinceAwake += Time.deltaTime;
            if(true) { // this.text.color != this.targetColor) {
                float f = Mathf.Lerp(0, 1, this.timeSinceAwake * this.revealSpeed);
                Color c = new Color(0, 0, 0, f);
                this.image.color += c;
                this.text.color += c;
            }
        }
    }
}
