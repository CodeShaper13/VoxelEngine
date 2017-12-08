using UnityEngine;
using UnityEngine.UI;

namespace Assets.VoxelEngine.GUI.Effect {

    public class SliderSpin : MonoBehaviour {

        private Slider slider;

        private void Awake() {
            this.slider = this.transform.parent.GetComponentInParent<Slider>();
        }

        private void Update() {
            float f = Mathf.Lerp(this.slider.minValue, this.slider.maxValue, this.slider.value);
            this.transform.rotation = Quaternion.Euler(0, 0, f * 180);
        }
    }
}
