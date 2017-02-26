using UnityEngine;

namespace VoxelEngine {

    // More or less a copy of Unity Standard Asset FPSCounter
    public class FpsCounter {

        private const float fpsMeasurePeriod = 0.5f;
        private const string display = "{0} FPS";

        public int currentFps;

        private int fpsAccumulator = 0;
        private float fpsNextPeriod = 0;

        public FpsCounter() {
            this.fpsNextPeriod = Time.realtimeSinceStartup + FpsCounter.fpsMeasurePeriod;
        }

        public void updateCounter() {
            // measure average frames per second
            this.fpsAccumulator++;
            if (Time.realtimeSinceStartup > this.fpsNextPeriod) {
                this.currentFps = (int)(this.fpsAccumulator / FpsCounter.fpsMeasurePeriod);
                this.fpsAccumulator = 0;
                this.fpsNextPeriod += FpsCounter.fpsMeasurePeriod;
            }
        }
    }
}
