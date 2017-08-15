using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLadder : BlockRendererPrimitive {

        private const float f = 0.45f;

        public override void renderBlock(Block block, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            if (meta == 2) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x + 0.5f, y - 0.5f, z - f),
                    new Vector3(x + 0.5f, y + 0.5f, z - f),
                    new Vector3(x - 0.5f, y + 0.5f, z - f),
                    new Vector3(x - 0.5f, y - 0.5f, z - f),
                    Direction.NONE);
            } else if (meta == 3) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x - f, y - 0.5f, z - 0.5f),
                    new Vector3(x - f, y + 0.5f, z - 0.5f),
                    new Vector3(x - f, y + 0.5f, z + 0.5f),
                    new Vector3(x - f, y - 0.5f, z + 0.5f),
                    Direction.NONE);
            } else if (meta == 0) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z + f),
                    new Vector3(x - 0.5f, y + 0.5f, z + f),
                    new Vector3(x + 0.5f, y + 0.5f, z + f),
                    new Vector3(x + 0.5f, y - 0.5f, z + f),
                    Direction.NONE);
            } else {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x + f, y - 0.5f, z + 0.5f),
                    new Vector3(x + f, y + 0.5f, z + 0.5f),
                    new Vector3(x + f, y + 0.5f, z - 0.5f),
                    new Vector3(x + f, y - 0.5f, z - 0.5f),
                    Direction.NONE);
            }
        }
    }
}
