using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Render {

    public class LightHelper {

        // Constants for light sample directions.
        public const int SELF = Direction.NONE_ID;
        public const int NORTH = Direction.NORTH_ID;
        public const int EAST = Direction.EAST_ID;
        public const int SOUTH = Direction.SOUTH_ID;
        public const int WEST = Direction.WEST_ID;
        public const int UP = Direction.UP_ID;
        public const int DOWN = Direction.DOWN_ID;

        /// <summary> The size of a pixel on the color sheet </summary>
        public const float PIXEL_SIZE = 0.0625f;
        /// <summary> 36 is the serialized form of _LightColor </summary>
        public const int COLOR_ID = 36;

        private Color[] normalCachedColors;
        private Color[] debugCachedColors;
        private bool useDebugColor;

        public LightHelper() {
            this.normalCachedColors = References.list.lightColorSheet.GetPixels();
            this.debugCachedColors = References.list.debugLightColorSheet.GetPixels();
        }

        public void toggleUseDebug() {
            this.useDebugColor = !this.useDebugColor;
        }

        /// <summary>
        /// Returns the color coresponding to a brightness
        /// </summary>
        public Color getColorFromBrightness(int lightLevel) {
            return this.useDebugColor ? this.debugCachedColors[lightLevel] : this.normalCachedColors[lightLevel];
        }
    }
}
