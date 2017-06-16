using UnityEngine;

namespace VoxelEngine.Util {

    /// <summary>
    /// Helper class for drawing debug lines and boxes
    /// </summary>
    public static class DebugDrawer {

        public static void box(Vector3 center, Vector3 size, Color color, float duration = 0) {
            DebugDrawer.plane(new Vector3(center.x, center.y + size.y, center.z), size, color, duration);
            DebugDrawer.plane(new Vector3(center.x, center.y - size.y, center.z), size, color, duration);
            Debug.DrawLine(new Vector3(center.x + size.x, center.y - size.y, center.z + size.z), new Vector3(center.x + size.x, center.y + size.y, center.z + size.z), color, duration);
            Debug.DrawLine(new Vector3(center.x - size.x, center.y - size.y, center.z + size.z), new Vector3(center.x - size.x, center.y + size.y, center.z + size.z), color, duration);
            Debug.DrawLine(new Vector3(center.x + size.x, center.y - size.y, center.z - size.z), new Vector3(center.x + size.x, center.y + size.y, center.z - size.z), color, duration);
            Debug.DrawLine(new Vector3(center.x - size.x, center.y - size.y, center.z - size.z), new Vector3(center.x - size.x, center.y + size.y, center.z - size.z), color, duration);
        }

        public static void plane(Vector3 center, Vector3 size, Color color, float duration = 0) {
            Vector3 corner1 = new Vector3(center.x + size.x, center.y, center.z + size.z);
            Vector3 corner2 = new Vector3(center.x - size.x, center.y, center.z + size.z);
            Vector3 corner3 = new Vector3(center.x + size.x, center.y, center.z - size.z);
            Vector3 corner4 = new Vector3(center.x - size.x, center.y, center.z - size.z);
            Debug.DrawLine(corner1, corner2, color, duration);
            Debug.DrawLine(corner2, corner4, color, duration);
            Debug.DrawLine(corner3, corner4, color, duration);
            Debug.DrawLine(corner1, corner3, color, duration);
        }

        public static void line(Vector3 start, Vector3 end, Color color, float duration = 0) {
            Debug.DrawLine(start, end, color, duration);
        }

        public static void bounds(Bounds bounds, Color color, Color? crossLineColor = null, float duration = 0) {
            if(crossLineColor != null) {
                Debug.DrawLine(bounds.min, bounds.max, (Color)crossLineColor, duration);
            }
            DebugDrawer.box(bounds.center, bounds.extents, color, duration);
        }
    }
}
