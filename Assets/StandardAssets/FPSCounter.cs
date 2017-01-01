using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility {
    public class FPSCounter : MonoBehaviour {
        const float fpsMeasurePeriod = 0.5f;
        private int fpsAccumulator = 0;
        private float fpsNextPeriod = 0;
        private int currentFps;
        const string formatString = "{0} FPS";
        private Text uiText;

        private void Start() {
            fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            uiText = GetComponent<Text>();
        }

        private void Update() {
            // measure average frames per second
            fpsAccumulator++;
            if (Time.realtimeSinceStartup > fpsNextPeriod) {
                currentFps = (int) (fpsAccumulator/fpsMeasurePeriod);
                fpsAccumulator = 0;
                fpsNextPeriod += fpsMeasurePeriod;
                uiText.text = string.Format(formatString, currentFps);
            }
        }
    }
}
