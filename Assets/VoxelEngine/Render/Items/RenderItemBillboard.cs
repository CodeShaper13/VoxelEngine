using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Render.Items {

    public class RenderItemBillboard : IRenderItem {

        private Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
        private Vector3[] verts;
        private int[] tris;

        public RenderItemBillboard() {
            this.verts = new Vector3[] {
                new Vector3(0.5f, -0.5f, 0),  // Bottom right
                new Vector3(0.5f, 0.5f, 0),   // Top right
                new Vector3(-0.5f, 0.5f, 0),  // Top left
                new Vector3(-0.5f, -0.5f, 0), // Bottom left
                new Vector3(-0.5f, -0.5f, 0),
                new Vector3(-0.5f, 0.5f, 0),
                new Vector3(0.5f, 0.5f, 0),
                new Vector3(0.5f, -0.5f, 0),
            };
            this.tris = new int[] {
                0, 1, 2,
                0, 2, 3,
                4, 5, 6,
                4, 6, 7
            };
        }

        public Mesh renderItem(Item item, byte meta) {
            float x = TexturePos.ITEM_SIZE * item.texturePos.x;
            float y = TexturePos.ITEM_SIZE * item.texturePos.y;

            Vector2[] uvArray = new Vector2[8];
            uvArray[0] = new Vector2(x, y);
            uvArray[1] = new Vector2(x, y + TexturePos.ITEM_SIZE);
            uvArray[2] = new Vector2(x + TexturePos.ITEM_SIZE, y + TexturePos.ITEM_SIZE);
            uvArray[3] = new Vector2(x + TexturePos.ITEM_SIZE, y);
            uvArray[4] = new Vector2(x, y);
            uvArray[5] = new Vector2(x, y + TexturePos.ITEM_SIZE);
            uvArray[6] = new Vector2(x + TexturePos.ITEM_SIZE, y + TexturePos.ITEM_SIZE);
            uvArray[7] = new Vector2(x + TexturePos.ITEM_SIZE, y);

            Mesh m = new Mesh();
            m.vertices = this.verts;
            m.triangles = this.tris;
            m.uv = uvArray;

            return m;
        }

        public Matrix4x4 getMatrix(Vector3 pos) {
            return Matrix4x4.TRS(pos, Quaternion.identity, this.scale);
        }
    }
}
