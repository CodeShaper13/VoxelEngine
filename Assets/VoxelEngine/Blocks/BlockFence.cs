using UnityEngine;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockFence : Block {

        public BlockFence(int id) : base(id) {
            this.setTransparent();
            this.setRenderer(RenderManager.FENCE);
            this.setType(EnumBlockType.WOOD);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            // Hacky fix where meta 0 = post, 1 = cross piece
            if(meta == 0) {
                if (direction == Direction.UP || direction == Direction.DOWN) {
                    return new TexturePos(7, 3);
                } else {
                    return new TexturePos(6, 3);
                }
            } else {
                return new TexturePos(8, 3);
            }
        }

        public override Vector2[] applyUvAlterations(Vector2[] uvs, int meta, Direction direction, Vector2 faceRadius, Vector2 faceOffset) {
            UvHelper.cropUVs(uvs, faceRadius);
            return uvs;
        }
    }
}
