using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLadder : BlockRendererPrimitive {

        private const float f = 0.45f;

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            if (meta == 2) {
                meshData.addQuad(
                    new Vector3(x + 0.5f, y - 0.5f, z - f),
                    new Vector3(x + 0.5f, y + 0.5f, z - f),
                    new Vector3(x - 0.5f, y + 0.5f, z - f),
                    new Vector3(x - 0.5f, y - 0.5f, z - f),
                    b.getUVs(meta, Direction.NORTH, this.preAllocatedUvArray),
                    LightHelper.SELF);
            } else if (meta == 3) {
                meshData.addQuad(
                    new Vector3(x - f, y - 0.5f, z - 0.5f),
                    new Vector3(x - f, y + 0.5f, z - 0.5f),
                    new Vector3(x - f, y + 0.5f, z + 0.5f),
                    new Vector3(x - f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.EAST, this.preAllocatedUvArray),
                    LightHelper.SELF);
            } else if (meta == 0) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z + f),
                    new Vector3(x - 0.5f, y + 0.5f, z + f),
                    new Vector3(x + 0.5f, y + 0.5f, z + f),
                    new Vector3(x + 0.5f, y - 0.5f, z + f),
                    b.getUVs(meta, Direction.SOUTH, this.preAllocatedUvArray),
                    LightHelper.SELF);
            } else {
                meshData.addQuad(
                    new Vector3(x + f, y - 0.5f, z + 0.5f),
                    new Vector3(x + f, y + 0.5f, z + 0.5f),
                    new Vector3(x + f, y + 0.5f, z - 0.5f),
                    new Vector3(x + f, y - 0.5f, z - 0.5f),
                    b.getUVs(meta, Direction.WEST, this.preAllocatedUvArray),
                    LightHelper.SELF);
            }
        }
    }
}
