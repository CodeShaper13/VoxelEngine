using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Render.NewSys {

    /// <summary>
    /// Represents the uvs for a single 4 sided plane.
    /// Used for working with pixel coords (0-31 inclusive) and then generating uvs.
    /// </summary>
    public struct UvPlane {

        private TexturePos texturePos;

        /// <summary> Uv on the bottom left side. </summary>
        private Vector2 uv0;
        /// <summary> Uv on the top left side. </summary>
        private Vector2 uv1;
        /// <summary> Uv on the top right side. </summary>
        private Vector2 uv2;
        /// <summary> Uv on the bottom right side. </summary>
        private Vector2 uv3;

        public UvPlane(TexturePos texturePos, int xStart, int yStart, int xSize, int ySize) {
            this.texturePos = texturePos;
            xSize -= 1;
            ySize -= 1;
            this.uv0 = new Vector2(xStart, yStart);
            this.uv1 = new Vector2(xStart, yStart + ySize);
            this.uv2 = new Vector2(xStart + xSize, yStart + ySize);
            this.uv3 = new Vector2(xStart + xSize, yStart);
        }

        public UvPlane(TexturePos texturePos, Vector2 pos1, Vector2 pos2) {
            this.texturePos = texturePos;
            this.uv0 = pos1;
            this.uv1 = new Vector2(pos1.x, pos2.y);
            this.uv2 = pos2;
            this.uv3 = new Vector2(pos2.x, pos1.y);
        }

        public Vector2[] getMeshUvs(Vector2[] uvs) {
            TexturePos tilePos = this.texturePos;
            float x = TexturePos.BLOCK_SIZE * tilePos.x;
            float y = TexturePos.BLOCK_SIZE * tilePos.y;
            uvs[0] = new Vector2(x,                         y) +                         (this.uv0 * TexturePos.PIXEL_SIZE);
            uvs[1] = new Vector2(x,                         y + TexturePos.PIXEL_SIZE) + (this.uv1 * TexturePos.PIXEL_SIZE);
            uvs[2] = new Vector2(x + TexturePos.PIXEL_SIZE, y + TexturePos.PIXEL_SIZE) + (this.uv2 * TexturePos.PIXEL_SIZE);
            uvs[3] = new Vector2(x + TexturePos.PIXEL_SIZE, y) +                         (this.uv3 * TexturePos.PIXEL_SIZE);
            if(tilePos.rotation != 0) {
                UvHelper.rotateUVs(uvs, tilePos.rotation);
            }
            return uvs;
        }
    }
}
