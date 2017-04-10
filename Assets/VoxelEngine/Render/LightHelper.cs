using UnityEngine;

namespace VoxelEngine.Render {

    public class LightHelper {

        /// <summary> The size of a pixel on the color sheet </summary>
        public const float PIXEL_SIZE = 0.0625f;
        /// <summary> 36 is the serialized form of _LightColor </summary>
        public const int COLOR_ID = 36;

        private Color[] cachedColors;

        public LightHelper(Texture2D lightColorSheet) {
            this.cachedColors = lightColorSheet.GetPixels();
        }

        /// <summary>
        /// Returns the color coresponding to a brightness
        /// </summary>
        public Color getColorFromBrightness(int lightLevel) {
            int x = lightLevel;
            int y = lightLevel;
            return this.cachedColors[(x * 16) + y];
        }
    }
}
