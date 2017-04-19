using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererFluid : BlockRendererPrimitive {

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            meshData.useRenderDataForCol = false;

            // Adjust the renderFace array.
            Block b1;
            for(int i= 0; i < 6; i++) {
                b1 = surroundingBlocks[i];
                renderFace[i] = (b1 != b || b1.isSolid);
            }

            // Adjusts top based on if a matching fluid is above.
            float topHeight = (surroundingBlocks[4] == b) ? 0.5f : 0.35f;

            // North
            if (renderFace[0]) {
                meshData.addQuad(
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.NORTH, this.uvArray),
                    NORTH);
            }
            // East
            if (renderFace[1]) {
                meshData.addQuad(
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.EAST, this.uvArray),
                    EAST);
            }
            // South
            if (renderFace[2]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    b.getUVs(meta, Direction.SOUTH, this.uvArray),
                    SOUTH);
            }
            // West
            if (renderFace[3]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    b.getUVs(meta, Direction.WEST, this.uvArray),
                    WEST);
            }
            // Up
            if (renderFace[4]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z - 0.5f),
                    b.getUVs(meta, Direction.UP, this.uvArray),
                    UP);
            }
            // Down
            if (renderFace[5]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.DOWN, this.uvArray),
                    DOWN);
            }

            meshData.useRenderDataForCol = true;
        }
    }
}
