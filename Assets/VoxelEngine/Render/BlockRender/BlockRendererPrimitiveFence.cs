using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveFence : BlockRendererPrimitive {

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            // Post
            float postRadius = MathHelper.pixelToWorld(6);
            meshData.addBox(
                new Vector3(x, y, z),
                new Vector3(postRadius, MathHelper.pixelToWorld(16), postRadius),
                Block.fence,
                0,
                this.preAllocatedUvArray);

            // Cross pieces
            this.addCrossPiece(surroundingBlocks[0], meshData, x, y, z, 0,      0.25f);
            this.addCrossPiece(surroundingBlocks[1], meshData, x, y, z, 0.25f,  0);
            this.addCrossPiece(surroundingBlocks[2], meshData, x, y, z, 0,     -0.25f);
            this.addCrossPiece(surroundingBlocks[3], meshData, x, y, z, -0.25f, 0);
        }

        private void addCrossPiece(Block surroundingBlock, MeshBuilder meshData, int x, int y, int z, float xAxis, float zAxis) {
            if (surroundingBlock.isSolid || surroundingBlock == Block.fence) {
                float f = MathHelper.pixelToWorld(2);
                meshData.addBox(
                    new Vector3(x + xAxis, y + MathHelper.pixelToWorld(5), z + zAxis),
                    new Vector3(xAxis != 0 ? 0.25f : f, MathHelper.pixelToWorld(6), zAxis != 0 ? 0.25f : f),
                    Block.fence,
                    1,
                    this.preAllocatedUvArray);
            }
        }
    }
}
