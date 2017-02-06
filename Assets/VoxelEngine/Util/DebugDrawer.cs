using UnityEngine;

namespace VoxelEngine.Util {

    public static class DebugDrawer {

        public static void box(Vector3 center, Vector3 size, Color color) {
            DebugDrawer.plane(new Vector3(center.x, center.y + size.y, center.z), size, color);
            DebugDrawer.plane(new Vector3(center.x, center.y - size.y, center.z), size, color);
            Debug.DrawLine(new Vector3(center.x + size.x, center.y - size.y, center.z + size.z), new Vector3(center.x + size.x, center.y + size.y, center.z + size.z), color);
            Debug.DrawLine(new Vector3(center.x - size.x, center.y - size.y, center.z + size.z), new Vector3(center.x - size.x, center.y + size.y, center.z + size.z), color);
            Debug.DrawLine(new Vector3(center.x + size.x, center.y - size.y, center.z - size.z), new Vector3(center.x + size.x, center.y + size.y, center.z - size.z), color);
            Debug.DrawLine(new Vector3(center.x - size.x, center.y - size.y, center.z - size.z), new Vector3(center.x - size.x, center.y + size.y, center.z - size.z), color);
        }

        public static void plane(Vector3 center, Vector3 size, Color color) {
            Vector3 c1 = new Vector3(center.x + size.x, center.y, center.z + size.z);
            Vector3 c2 = new Vector3(center.x - size.x, center.y, center.z + size.z);
            Vector3 c3 = new Vector3(center.x + size.x, center.y, center.z - size.z);
            Vector3 c4 = new Vector3(center.x - size.x, center.y, center.z - size.z);
            Debug.DrawLine(c1, c2, color);
            Debug.DrawLine(c2, c4, color);
            Debug.DrawLine(c3, c4, color);
            Debug.DrawLine(c1, c3, color);
        }

        public static void line(Vector3 start, Vector3 end, Color color) {
            Debug.DrawLine(start, end, color);
        }

        public static void bounds(Bounds bounds, Color color, Color? crossLineColor = null) {
            if(crossLineColor != null) {
                Debug.DrawLine(bounds.min, bounds.max, Color.green);
            }
            DebugDrawer.box(bounds.center, bounds.extents, color);
        }
    }
}
