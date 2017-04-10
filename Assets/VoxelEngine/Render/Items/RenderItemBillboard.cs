using UnityEngine;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Render.Items {

    public class RenderItemBillboard : IRenderItem {

        private static Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
        private static Vector3[] verts = new Vector3[] {
            new Vector3(0.5f, -0.5f, 0),  // Bottom right
            new Vector3(0.5f, 0.5f, 0),   // Top right
            new Vector3(-0.5f, 0.5f, 0),  // Top left
            new Vector3(-0.5f, -0.5f, 0), // Bottom left
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(0.5f, -0.5f, 0) };
        private static int[] tris = new int[] {
            0, 1, 2,
            0, 2, 3,
            4, 5, 6,
            4, 6, 7 };        
        private static Vector2[] uv2 = new Vector2[] {
            new Vector2(0.9375f, 0.9375f),
            new Vector2(0.9375f, 1),
            new Vector2(1, 1),
            new Vector2(1, 0.9375f),

            new Vector2(0.9375f, 0.9375f),
            new Vector2(0.9375f, 1),
            new Vector2(1, 1),
            new Vector2(1, 0.9375f), };

        public Mesh renderItem(Item item, byte meta) {
            float x = TexturePos.ITEM_SIZE * item.texturePos.x;
            float y = TexturePos.ITEM_SIZE * item.texturePos.y;

            Vector2[] uvArray = new Vector2[8];
            for(int i = 0; i < 5; i += 4) {
                uvArray[i]      = new Vector2(x, y);
                uvArray[i + 1]  = new Vector2(x, y + TexturePos.ITEM_SIZE);
                uvArray[i + 2]  = new Vector2(x + TexturePos.ITEM_SIZE, y + TexturePos.ITEM_SIZE);
                uvArray[i + 3]  = new Vector2(x + TexturePos.ITEM_SIZE, y);
            }

            Mesh m = new Mesh();
            m.vertices = RenderItemBillboard.verts;
            m.triangles = RenderItemBillboard.tris;
            m.uv = uvArray;
            m.uv2 = RenderItemBillboard.uv2;

            return m;
        }

        public Matrix4x4 getMatrix(Vector3 pos) {
            return Matrix4x4.TRS(pos, Quaternion.identity, RenderItemBillboard.scale);
        }
    }
}
