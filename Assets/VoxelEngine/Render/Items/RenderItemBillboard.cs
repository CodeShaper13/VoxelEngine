using UnityEngine;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Render.Items {

    public class RenderItemBillboard : IRenderItem {

        public Mesh renderItem(RenderManager rm, Item item, int meta) {
            MeshBuilder meshBuilder = rm.getMeshBuilder();
            meshBuilder.lightLevels[0] = 15;
            TexturePos textPos = item.getItemTexturePos(meta);
            float halfPixelSize = 0.015625f;

            // Add the front and back.
            float zOffset = halfPixelSize;
            meshBuilder.addQuad(
                new Vector3(0.5f, -0.5f, zOffset),  // Bottom right
                new Vector3(0.5f, 0.5f, zOffset),   // Top right
                new Vector3(-0.5f, 0.5f, zOffset),  // Top left
                new Vector3(-0.5f, -0.5f, zOffset), // Bottom left
                UvHelper.mirrorUvsX(this.getUvs(item, textPos)),
                0);
            meshBuilder.addQuad(
                new Vector3(-0.5f, -0.5f, -zOffset),
                new Vector3(-0.5f, 0.5f, -zOffset),
                new Vector3(0.5f, 0.5f, -zOffset),
                new Vector3(0.5f, -0.5f, -zOffset),
                this.getUvs(item, textPos),
                0);

            // Add the side pixels
            int pixelStartX = textPos.x * 32;
            int pixelStartY = textPos.y * 32;
            Texture2D atlas = References.list.itemAtlas;
            Vector2[] pixelUvs = new Vector2[4];
            float pixelOrginX, pixelOrginY;

            for(int x = 1; x < 32; x++) {
                for(int y = 1; y < 32; y++) {
                    if(!(atlas.GetPixel(pixelStartX + x, pixelStartY + y).a == 0)) { // Solid pixel.
                        pixelOrginX = (x - 15) * (halfPixelSize * 2) - halfPixelSize;
                        pixelOrginY = (y - 15) * (halfPixelSize * 2) - halfPixelSize;

                        // Right/+X
                        if (this.func(atlas, pixelStartX + x, pixelStartY + y, 1, 0, ref pixelUvs)) {
                            meshBuilder.addQuad(
                                new Vector3(pixelOrginX + halfPixelSize, pixelOrginY - halfPixelSize, 0 - halfPixelSize),
                                new Vector3(pixelOrginX + halfPixelSize, pixelOrginY + halfPixelSize, 0 - halfPixelSize),
                                new Vector3(pixelOrginX + halfPixelSize, pixelOrginY + halfPixelSize, 0 + halfPixelSize),
                                new Vector3(pixelOrginX + halfPixelSize, pixelOrginY - halfPixelSize, 0 + halfPixelSize),
                                pixelUvs,
                                0);
                        }
                        // Left/-X
                        if (this.func(atlas, pixelStartX + x, pixelStartY + y, -1, 0, ref pixelUvs)) {
                            meshBuilder.addQuad(
                                new Vector3(pixelOrginX - halfPixelSize, pixelOrginY - halfPixelSize, 0 + halfPixelSize),
                                new Vector3(pixelOrginX - halfPixelSize, pixelOrginY + halfPixelSize, 0 + halfPixelSize),
                                new Vector3(pixelOrginX - halfPixelSize, pixelOrginY + halfPixelSize, 0 - halfPixelSize),
                                new Vector3(pixelOrginX - halfPixelSize, pixelOrginY - halfPixelSize, 0 - halfPixelSize),
                                pixelUvs,
                                0);
                        }
                        // Up/+Y
                        if (this.func(atlas, pixelStartX + x, pixelStartY + y, 0, 1, ref pixelUvs)) {
                            meshBuilder.addQuad(
                                new Vector3(pixelOrginX - halfPixelSize, pixelOrginY + halfPixelSize, 0 - halfPixelSize),
                                new Vector3(pixelOrginX - halfPixelSize, pixelOrginY + halfPixelSize, 0 + halfPixelSize),
                                new Vector3(pixelOrginX + halfPixelSize, pixelOrginY + halfPixelSize, 0 + halfPixelSize),
                                new Vector3(pixelOrginX + halfPixelSize, pixelOrginY + halfPixelSize, 0 - halfPixelSize),
                                pixelUvs,
                                0);
                        }
                        // Down/-Y
                        if (this.func(atlas, pixelStartX + x, pixelStartY + y, 0, -1, ref pixelUvs)) {
                            meshBuilder.addQuad(
                               new Vector3(pixelOrginX - halfPixelSize, pixelOrginY - halfPixelSize, 0 + halfPixelSize),
                               new Vector3(pixelOrginX - halfPixelSize, pixelOrginY - halfPixelSize, 0 - halfPixelSize),
                               new Vector3(pixelOrginX + halfPixelSize, pixelOrginY - halfPixelSize, 0 - halfPixelSize),
                               new Vector3(pixelOrginX + halfPixelSize, pixelOrginY - halfPixelSize, 0 + halfPixelSize),
                               pixelUvs,
                               0);
                        }
                    }
                }
            }

            return meshBuilder.toMesh();
        }

        /// <summary>
        /// Checks if an adjacent pixel is transparent, and if so returns true and populates pixelUvs with uvs for the orgin pixel.
        /// </summary>
        private bool func(Texture2D textureAtlas, int x, int y, int shiftX, int shiftY, ref Vector2[] pixelUvs) {
            Color c = textureAtlas.GetPixel(x + shiftX, y + shiftY);
            if(c.a == 0) { // Transparent pixel.
                float px = x * TexturePos.PIXEL_SIZE;
                float py = y * TexturePos.PIXEL_SIZE;
                
                pixelUvs[0] = new Vector2(px, py);
                pixelUvs[1] = new Vector2(px, py + TexturePos.PIXEL_SIZE);
                pixelUvs[2] = new Vector2(px + TexturePos.PIXEL_SIZE, py + TexturePos.PIXEL_SIZE);
                pixelUvs[3] = new Vector2(px + TexturePos.PIXEL_SIZE, py);
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the uvs for an item's front and back.
        /// </summary>
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