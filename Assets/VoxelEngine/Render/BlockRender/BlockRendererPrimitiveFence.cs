using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveFence : BlockRendererPrimitive {

        private const float postRadius = 0.2f;

        private Bounds postBounds;
        private Bounds northBounds;
        private Bounds eastBounds;
        private Bounds southBounds;
        private Bounds westBounds;

        public BlockRendererPrimitiveFence() : base() {
            this.postBounds = new Bounds(new Vector3(0, 0, 0), new Vector3(postRadius * 2, 1, postRadius * 2));
            this.northBounds = new Bounds(new Vector3(0, 0, 0.35f), new Vector3(0.1f, 1f, 0.3f));
            this.eastBounds = new Bounds(new Vector3(0.35f, 0, 0), new Vector3(0.3f, 1f, 0.1f));
            this.southBounds = new Bounds(new Vector3(0, 0, -0.35f), new Vector3(0.1f, 1f, 0.3f));
            this.westBounds = new Bounds(new Vector3(-0.35f, 0, 0), new Vector3(0.3f, 1f, 0.1f));
        }

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            meshData.useRenderDataForCol = false;
            meshData.addColliderBox(this.postBounds, x, y, z);

            // North
            meshData.addQuad(
                new Vector3(x + postRadius, y - 0.5f, z + postRadius),
                new Vector3(x + postRadius, y + 0.5f, z + postRadius),
                new Vector3(x - postRadius, y + 0.5f, z + postRadius),
                new Vector3(x - postRadius, y - 0.5f, z + postRadius),
                b.getUVs(meta, Direction.NORTH, this.uvArray),
                0);

            // East
            meshData.addQuad(
                new Vector3(x + postRadius, y - 0.5f, z - postRadius),
                new Vector3(x + postRadius, y + 0.5f, z - postRadius),
                new Vector3(x + postRadius, y + 0.5f, z + postRadius),
                new Vector3(x + postRadius, y - 0.5f, z + postRadius),
                b.getUVs(meta, Direction.EAST, this.uvArray),
                0);

            // South
            meshData.addQuad(
                new Vector3(x - postRadius, y - 0.5f, z - postRadius),
                new Vector3(x - postRadius, y + 0.5f, z - postRadius),
                new Vector3(x + postRadius, y + 0.5f, z - postRadius),
                new Vector3(x + postRadius, y - 0.5f, z - postRadius),
                b.getUVs(meta, Direction.SOUTH, this.uvArray),
                0);

            // West
            meshData.addQuad(
                new Vector3(x - postRadius, y - 0.5f, z + postRadius),
                new Vector3(x - postRadius, y + 0.5f, z + postRadius),
                new Vector3(x - postRadius, y + 0.5f, z - postRadius),
                new Vector3(x - postRadius, y - 0.5f, z - postRadius),
                b.getUVs(meta, Direction.WEST, this.uvArray),
                0);

            if(renderFace[4]) {
                // Up
                meshData.addQuad(
                    new Vector3(x - postRadius, y + 0.5f, z + postRadius),
                    new Vector3(x + postRadius, y + 0.5f, z + postRadius),
                    new Vector3(x + postRadius, y + 0.5f, z - postRadius),
                    new Vector3(x - postRadius, y + 0.5f, z - postRadius),
                    b.getUVs(meta, Direction.UP, this.uvArray),
                    Direction.UP_ID);
            }
            if(renderFace[5]) {
                // Down
                meshData.addQuad(
                    new Vector3(x - postRadius, y - 0.5f, z - postRadius),
                    new Vector3(x + postRadius, y - 0.5f, z - postRadius),
                    new Vector3(x + postRadius, y - 0.5f, z + postRadius),
                    new Vector3(x - postRadius, y - 0.5f, z + postRadius),
                    b.getUVs(meta, Direction.DOWN, this.uvArray),
                    Direction.DOWN_ID);
            }

            this.addCrossPiece(surroundingBlocks[0], meshData, x, y, z, 0,     0.5f,  this.northBounds);
            this.addCrossPiece(surroundingBlocks[1], meshData, x, y, z, 0.5f,  0,     this.eastBounds);
            this.addCrossPiece(surroundingBlocks[2], meshData, x, y, z, 0,     -0.5f, this.southBounds);
            this.addCrossPiece(surroundingBlocks[3], meshData, x, y, z, -0.5f, 0,     this.westBounds);
        }

        private void addCrossPiece(Block surroundingBlock, MeshBuilder meshData, int x, int y, int z, float xAxis, float zAxis, Bounds bounds) {
            if (surroundingBlock.isSolid || surroundingBlock == Block.fence) {
                meshData.addQuad(
                    new Vector3(x,         y + 0.45f, z),
                    new Vector3(x + xAxis, y + 0.45f, z + zAxis),
                    new Vector3(x + xAxis, y + 0.1f,  z + zAxis),
                    new Vector3(x,         y + 0.1f,  z),
                    Block.fence.getUVs(0, Direction.NONE, this.uvArray),
                    0);
                meshData.addQuad(
                    new Vector3(x,         y + 0.1f,  z),
                    new Vector3(x + xAxis, y + 0.1f,  z + zAxis),
                    new Vector3(x + xAxis, y + 0.45f, z + zAxis),
                    new Vector3(x,         y + 0.45f, z),
                    Block.fence.getUVs(0, Direction.NONE, this.uvArray),
                    0);
                meshData.addColliderBox(bounds, x, y, z);
            }
        }
    }
}
