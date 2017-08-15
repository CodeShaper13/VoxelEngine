using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererTorch : BlockRendererPrimitive {

        private const float ROT = 15.0f;
        public const float SHIFT = 0.4375f;

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            Vector3 offset;
            Quaternion rotation;
            if (meta == 1) { // North
                offset = new Vector3(x, y, z + SHIFT);
                rotation = Quaternion.Euler(-ROT, 0, 0);
            } else if (meta == 2) { // East
                offset = new Vector3(x + SHIFT, y, z);
                rotation = Quaternion.Euler(0, 0, ROT);
            } else if (meta == 3) { // South
                offset = new Vector3(x, y, z - SHIFT);
                rotation = Quaternion.Euler(ROT, 0, 0);
            } else if (meta == 4) { // West
                offset = new Vector3(x - SHIFT, y, z);
                rotation = Quaternion.Euler(0, 0, -ROT);
            } else { // 0, On floor
                offset = new Vector3(x, y - 0.1f, z);
                rotation = Quaternion.identity;
            }

            float r = MathHelper.pixelToWorld(4);

            meshData.addBox(offset, new Vector3(r, MathHelper.pixelToWorld(14), r), rotation, b, meta, RenderManager.TRUE_ARRAY);
        }
    }
}
