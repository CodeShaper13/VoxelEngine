using UnityEngine;

namespace VoxelEngine.Util {

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
            Vector3 c1 = new Vector3(center.x + size.x, center.y, center.z + size.z);
            Vector3 c2 = new Vector3(center.x - size.x, center.y, center.z + size.z);
            Vector3 c3 = new Vector3(center.x + size.x, center.y, center.z - size.z);
            Vector3 c4 = new Vector3(center.x - size.x, center.y, center.z - size.z);
            Debug.DrawLine(c1, c2, color, duration);
            Debug.DrawLine(c2, c4, color, duration);
            Debug.DrawLine(c3, c4, color, duration);
            Debug.DrawLine(c1, c3, color, duration);
        }

        public static void line(Vector3 start, Vector3 end, Color color, float duration = 0) {
            Debug.DrawLine(start, end, color, duration);
        }

        public static void bounds(Bounds bounds, Color color, Color? crossLineColor = null, float duration = 0) {
            if(crossLineColor != null) {
                Debug.DrawLine(bounds.min, bounds.max, Color.green, duration);
            }
            DebugDrawer.box(bounds.center, bounds.extents, color, duration);
        }
    }
}
