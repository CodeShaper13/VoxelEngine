using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Render {

    /// <summary>
    /// Represents the uvs for a single 4 sided plane.
    /// Used for working with pixel coords (0-31 inclusive) and then generating uvs.
    /// </summary>
    public struct UvPlane {

        public TexturePos texturePos;

        // Use 0-32 for uv position

        /// <summary> Uv on the bottom left side. </summary>
        public Vector2 uv0;
        /// <summary> Uv on the top left side. </summary>
        public Vector2 uv1;
        /// <summary> Uv on the top right side. </summary>
        public Vector2 uv2;
        /// <summary> Uv on the bottom right side. </summary>
        public Vector2 uv3;

        public Vector2[] getMeshUvs(Vector2[] uvs) {
            TexturePos tilePos = this.texturePos;
            float x = TexturePos.BLOCK_SIZE * tilePos.x;
            float y = TexturePos.BLOCK_SIZE * tilePos.y;
            uvs[0] = new Vector2(x, y) + (this.uv0 * TexturePos.PIXEL_SIZE);
            uvs[1] = new Vector2(x, y + TexturePos.PIXEL_SIZE) + (this.uv1 * TexturePos.PIXEL_SIZE);
            uvs[2] = new Vector2(x + TexturePos.PIXEL_SIZE, y + TexturePos.PIXEL_SIZE) + (this.uv2 * TexturePos.PIXEL_SIZE);
            uvs[3] = new Vector2(x + TexturePos.PIXEL_SIZE, y) + (this.uv3 * TexturePos.PIXEL_SIZE);

            return uvs;
        }
    }
}
