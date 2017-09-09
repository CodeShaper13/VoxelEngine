using UnityEngine;

namespace VoxelEngine.Render {

    public class LightColors {

        /// <summary> 36 is the serialized form of _LightColor in the Block shader. </summary>
        public const int SERIALIZED_LightColor = 36;

        private Color[] normalLightColors;
        private Color[] debugLightColors;
        private bool useDebugColor;

        public LightColors() {
            this.normalLightColors = References.list.lightColorSheet.GetPixels();
            this.debugLightColors = References.list.debugLightColorSheet.GetPixels();
        }

        public void toggleUseDebugColors() {
            this.useDebugColor = !this.useDebugColor;
        }

        /// <summary>
        /// Returns the color corresponding to a brightness
        /// </summary>
        public Color getColorFromBrightness(int lightLevel) {
            return this.useDebugColor ? this.debugLightColors[lightLevel] : this.normalLightColors[lightLevel];
        }
    }
}
