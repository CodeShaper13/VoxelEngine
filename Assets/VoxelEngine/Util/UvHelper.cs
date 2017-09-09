using UnityEngine;

namespace VoxelEngine.Util {

    /// <summary>
    /// Helper class for working with uvs.  All passed in uv arrays
    /// must be 4 elemeents long, a single block/item face.
    /// </summary>
    public static class UvHelper {

        /// <summary>
        /// Rotates the uvs by rotation and returns them.  Only use multiples of 90, no negatives and don't exceed 270 degrees.
        /// </summary>
        public static Vector2[] rotateUVs(Vector2[] uvs, int degrees) {
            degrees /= 90;
            int index = degrees;
            Vector2 v0 = uvs[index >= 4 ? index - 4 : index];
            index = 1 + degrees;
            Vector2 v1 = uvs[index >= 4 ? index - 4 : index];
            index = 2 + degrees;
            Vector2 v2 = uvs[index >= 4 ? index - 4 : index];
            index = 3 + degrees;
            Vector2 v3 = uvs[index >= 4 ? index - 4 : index];

            uvs[0] = v0;
            uvs[1] = v1;
            uvs[2] = v2;
            uvs[3] = v3;
            return uvs;
        }

        /// <summary>
        /// Mirrors uvs on the x axis.
        /// </summary>
        public static Vector2[] mirrorUvsX(Vector2[] uvs) {
            // Bottom
            Vector2 temp = uvs[0];
            uvs[0] = uvs[3];
            uvs[3] = temp;

            // Top
            temp = uvs[1];
            uvs[1] = uvs[2];
            uvs[2] = temp;

            return uvs;
        }

        /// <summary>
        /// Mirrors uvs on the y axis.
        /// </summary>
        public static Vector2[] mirrorUvsY(Vector2[] uvs) {
            // Left
            Vector2 temp = uvs[0];
            uvs[0] = uvs[1];
            uvs[1] = temp;

            // Right
            temp = uvs[2];
            uvs[2] = uvs[3];
            uvs[3] = temp;

            return uvs;
        }

        /// <summary>
        /// WARNING!  Some UVs will be mirrored!  Makes the faces uvs align with pixel, cropping to the middle.
        /// </summary>
        public static Vector2[] cropUVs(Vector2[] uvs, Vector2 faceRadius) {
            if(faceRadius.x < 0.5f || faceRadius.y < 0.5f) { // Only crop if the face is less than a full 1x1 plane.
                Vector2 uv0 = uvs[0];
                Vector2 uv1 = uvs[1];
                Vector2 uv2 = uvs[2];
                Vector2 uv3 = uvs[3];

                float clipX = (16 - (faceRadius.x * 32)) * TexturePos.PIXEL_SIZE;
                float clipY = (16 - (faceRadius.y * 32)) * TexturePos.PIXEL_SIZE;
                uvs[0] = new Vector2(
                    uv0.x + clipX,
                    uv0.y + clipY);
                uvs[1] = new Vector2(
                    uv1.x + clipX,
                    uv1.y - clipY);
                uvs[2] = new Vector2(
                    uv2.x - clipX,
                    uv2.y - clipY);
                uvs[3] = new Vector2(
                    uv3.x - clipX,
                    uv3.y + clipY);
            }
            return uvs;
        }

        public static Vector2[] smartShiftUVs(Vector2[] uvs, Vector2 faceRadius, Vector2 faceOffset) {
            if (faceRadius.x < 0.5f || faceRadius.y < 0.5f) {
                Vector2 pixelOffset = faceOffset * 32;

                Vector2 uv0 = uvs[0];
                Vector2 uv1 = uvs[1];
                Vector2 uv2 = uvs[2];
                Vector2 uv3 = uvs[3];
                uvs[0] = new Vector2(
                    uv0.x + pixelOffset.x * TexturePos.PIXEL_SIZE,
                    uv0.y + pixelOffset.y * TexturePos.PIXEL_SIZE);
                uvs[1] = new Vector2(
                    uv1.x + pixelOffset.x * TexturePos.PIXEL_SIZE,
                    uv1.y + pixelOffset.y * TexturePos.PIXEL_SIZE);
                uvs[2] = new Vector2(
                    uv2.x + pixelOffset.x * TexturePos.PIXEL_SIZE,
                    uv2.y + pixelOffset.y * TexturePos.PIXEL_SIZE);
                uvs[3] = new Vector2(
                    uv3.x + pixelOffset.x * TexturePos.PIXEL_SIZE,
                    uv3.y + pixelOffset.y * TexturePos.PIXEL_SIZE);
            }
            return uvs;
        }
    }
}
