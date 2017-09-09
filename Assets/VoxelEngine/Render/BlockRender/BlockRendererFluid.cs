using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererFluid : BlockRendererPrimitive {

        public BlockRendererFluid() {
            this.lookupAdjacentBlocks = true;
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshData, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshData.autoGenerateColliders = false;

            /*
            // Adjust the renderFace array.
            Block b1;
            for(int i= 0; i < 6; i++) {
                b1 = surroundingBlocks[i];
                renderFace[i] = (b1 != block || b1.isSolid);
            }

            // Adjusts top based on if a matching fluid is above.
            float topHeight = (surroundingBlocks[Direction.UP_ID - 1] == block) ? 0.5f : 0.35f;

            // North
            if (renderFace[0]) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    Direction.NORTH);
            }
            // East
            if (renderFace[1]) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    Direction.EAST);
            }
            // South
            if (renderFace[2]) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    Direction.SOUTH);
            }
            // West
            if (renderFace[3]) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    Direction.WEST);
            }
            // Up
            if (renderFace[4]) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x - 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z + 0.5f),
                    new Vector3(x + 0.5f, y + topHeight, z - 0.5f),
                    new Vector3(x - 0.5f, y + topHeight, z - 0.5f),
                    Direction.UP);
            }
            // Down
            if (renderFace[5]) {
                meshData.addPlane(
                    block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    Direction.DOWN);
            }
            */

            meshData.autoGenerateColliders = true;
        }
    }
}
