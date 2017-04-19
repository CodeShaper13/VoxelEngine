using UnityEngine;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Render.Items {

    public class RenderItemBillboard : IRenderItem {

        private static Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);

        public Mesh renderItem(RenderManager rm, Item item, int meta) {
            MeshBuilder meshBuilder = rm.getMeshBuilder();
            meshBuilder.lightLevels[0] = 15;
            TexturePos textPos = item.getItemTexturePos(meta);

            // Add the front and back.
            float zOffset = TexturePos.ITEM_SIZE / 2;
            meshBuilder.addQuad(
                new Vector3(0.5f, -0.5f, zOffset), // Bottom right
                new Vector3(0.5f, 0.5f, zOffset),   // Top right
                new Vector3(-0.5f, 0.5f, zOffset),  // Top left
                new Vector3(-0.5f, -0.5f, zOffset), // Bottom left
                this.getUvs(item, textPos),
                0);
            meshBuilder.addQuad(
                new Vector3(-0.5f, -0.5f, -zOffset),
                new Vector3(-0.5f, 0.5f, -zOffset),
                new Vector3(0.5f, 0.5f, -zOffset),
                new Vector3(0.5f, -0.5f, -zOffset),
                this.getUvs(item, textPos),
                0);

            // Add the side pixels
            int startX = textPos.x * 32;
            int startY = textPos.y * 32;
            Texture2D atlas = References.list.itemAtlas;
            Vector2[] pixelUvs = new Vector2[4];
            for(int x = 1; x < 31; x++) {
                for(int y = 1; y < 31; y++) {
                    // Right.
                    if(this.getSidePixel(atlas, startX + x + 1, startY + y, ref pixelUvs)) {
                        meshBuilder.addQuad(
                            new Vector2(),
                            new Vector2(),
                            new Vector2(),
                            new Vector2(),
                            pixelUvs,
                            0);
                    }
                    // Left.
                    if (this.getSidePixel(atlas, startX + x - 1, startY + y, ref pixelUvs)) {

                    }
                    // Up.
                    if (this.getSidePixel(atlas, startX + x, startY + y + 1, ref pixelUvs)) {

                    }
                    // Down.
                    if (this.getSidePixel(atlas, startX + x, startY + y - 1, ref pixelUvs)) {
                    }
                }
            }
            return meshBuilder.toMesh();
        }

        public Matrix4x4 getMatrix(Vector3 pos) {
            return Matrix4x4.TRS(pos, Quaternion.identity, RenderItemBillboard.scale);
        }

        private bool getSidePixel(Texture2D atlas, int x, int y, ref Vector2[] pixelUvs) {
            Color c = atlas.GetPixel(x, y);
            if(c.a == 0) { // Transparent pixel.
                float px = x * TexturePos.ITEM_PIXEL_SIZE;
                float py = y * TexturePos.ITEM_PIXEL_SIZE;
                pixelUvs[0] = new Vector2(px, py);
                pixelUvs[1] = new Vector2(px, py + TexturePos.ITEM_PIXEL_SIZE);
                pixelUvs[2] = new Vector2(px + TexturePos.ITEM_PIXEL_SIZE, py + TexturePos.ITEM_PIXEL_SIZE);
                pixelUvs[3] = new Vector2(px + TexturePos.ITEM_PIXEL_SIZE, py);
                return true;
            }

            return false;
        }

        private Vector2[] getUvs(Item item, TexturePos textPos) {
            float x = TexturePos.ITEM_SIZE * textPos.x;
            float y = TexturePos.ITEM_SIZE * textPos.y;

            Vector2[] uvArray = new Vector2[4];
            uvArray[0] = new Vector2(x, y);
            uvArray[1] = new Vector2(x, y + TexturePos.ITEM_SIZE);
            uvArray[2] = new Vector2(x + TexturePos.ITEM_SIZE, y + TexturePos.ITEM_SIZE);
            uvArray[3] = new Vector2(x + TexturePos.ITEM_SIZE, y);

            return uvArray;
        }
    }
}
