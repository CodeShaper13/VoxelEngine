using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Render {

    /// <summary>
    /// Represents the uvs for a single 4 sided plane.
    /// NOT CORRECT? ->Used for working with pixel coords (0-31 inclusive) and then generating uvs.
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

        /// <summary>
        /// Start is 0 based, with 0,0 being on texture.  1, 1, 32, 32 is an entire tile.
        /// </summary>
        public UvPlane(TexturePos texturePos, int xStart, int yStart, int pixelCountX, int pixelCountY) {
            this.texturePos = texturePos;
            xStart--;
            yStart--;
            pixelCountX--;
            pixelCountY--;
            this.uv0 = new Vector2(xStart, yStart);
            this.uv1 = new Vector2(xStart, yStart + pixelCountY);
            this.uv2 = new Vector2(xStart + pixelCountX, yStart + pixelCountY);
            this.uv3 = new Vector2(xStart + pixelCountX, yStart);
        }

        public UvPlane(TexturePos texturePos, Vector2 pos1, Vector2 pos2) {
            this.texturePos = texturePos;
            this.uv0 = pos1;
            this.uv1 = new Vector2(pos1.x, pos2.y);
            this.uv2 = pos2;
            this.uv3 = new Vector2(pos2.x, pos1.y);
        }

        // Used by slabs and stairs.
        /// <summary>
        /// Attempts to create the uvs based on the CubeComponent's faces.
        /// </summary>
        public UvPlane(TexturePos pos, CubeComponent cubeComponent, Direction faceDirection) {
            this.texturePos = pos;

            Vector2 v0;
            Vector2 v1;

            if(faceDirection == Direction.NORTH) {
                v0 = new Vector2(32 - cubeComponent.pos.x, cubeComponent.neg.y);
                v1 = new Vector2(32 - cubeComponent.neg.x, cubeComponent.pos.y);
            } else if(faceDirection == Direction.EAST) {
                v0 = new Vector2(cubeComponent.neg.z, cubeComponent.neg.y);
                v1 = new Vector2(cubeComponent.pos.z, cubeComponent.pos.y);
            } else if (faceDirection == Direction.SOUTH) {
                v0 = new Vector2(cubeComponent.neg.x, cubeComponent.neg.y);
                v1 = new Vector2(cubeComponent.pos.x, cubeComponent.pos.y);
            } else if(faceDirection == Direction.WEST) {
                v0 = new Vector2(32 - cubeComponent.pos.z, cubeComponent.neg.y);
                v1 = new Vector2(32 - cubeComponent.neg.z, cubeComponent.pos.y);
            } else if(faceDirection == Direction.UP) {
                v0 = new Vector2(cubeComponent.neg.x, cubeComponent.neg.z);
                v1 = new Vector2(cubeComponent.pos.x, cubeComponent.pos.z);
            } else { // d == Direction.DOWN
                v0 = new Vector2(cubeComponent.neg.x, cubeComponent.neg.z);
                v1 = new Vector2(cubeComponent.pos.x, cubeComponent.pos.z);
            }

            v1.x -= 1;
            v1.y -= 1;

            this.uv0 = v0;
            this.uv1 = new Vector2(v0.x, v1.y);
            this.uv2 = v1;
            this.uv3 = new Vector2(v1.x, v0.y);
        }

        public Vector2[] getMeshUvs(Vector2[] uvs) {
            TexturePos tilePos = this.texturePos;
            float x = TexturePos.ATLAS_TILE_SIZE * tilePos.x;
            float y = TexturePos.ATLAS_TILE_SIZE * tilePos.y;
            uvs[0] = new Vector2(x,                         y) +                         (this.uv0 * TexturePos.PIXEL_SIZE);
            uvs[1] = new Vector2(x,                         y + TexturePos.PIXEL_SIZE) + (this.uv1 * TexturePos.PIXEL_SIZE);
            uvs[2] = new Vector2(x + TexturePos.PIXEL_SIZE, y + TexturePos.PIXEL_SIZE) + (this.uv2 * TexturePos.PIXEL_SIZE);
            uvs[3] = new Vector2(x + TexturePos.PIXEL_SIZE, y) +                         (this.uv3 * TexturePos.PIXEL_SIZE);
            if(tilePos.rotation != 0) {
                UvHelper.rotateUVs(uvs, tilePos.rotation);
            }
            if(tilePos.mirrorFlags != 0) {
                if((tilePos.mirrorFlags & 1) == 1) {
                    UvHelper.mirrorUvsX(uvs);
                }
                if ((tilePos.mirrorFlags >> 1) == 1) {
                    UvHelper.mirrorUvsY(uvs);
                }
            }
            return uvs;
        }
    }
}
