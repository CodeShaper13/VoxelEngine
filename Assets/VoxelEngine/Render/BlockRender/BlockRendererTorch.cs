using UnityEngine;
using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererTorch : BlockRendererPrimitive {

        private const float ROT = 15.0f;
        public const float SHIFT = 0.35f;
        private const float TORCH_HEIGHT = 0.4f;

        public override MeshBuilder renderBlock(Block b, byte meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {

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

            meshData.addBox(offset, new Vector3(0.15f, TORCH_HEIGHT, 0.15f), rotation, b, meta, this.uvArray);

            return meshData;
        }
    }
}
