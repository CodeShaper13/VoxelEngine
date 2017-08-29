using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererFence : BlockRendererPrimitive {

        public BlockRendererFence() {
            this.lookupAdjacentLight = true;
            this.lookupAdjacentBlocks = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            // Post
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    12, 0, 12,
                    20, 32, 20,
                    0),
                renderFace | RenderFace.Y,
                x, y, z);

            // Cross pieces
            this.addCrossPiece(surroundingBlocks[0], meta, meshBuilder, x, y, z, 0);
            this.addCrossPiece(surroundingBlocks[1], meta, meshBuilder, x, y, z, 90);
            this.addCrossPiece(surroundingBlocks[2], meta, meshBuilder, x, y, z, 180);
            this.addCrossPiece(surroundingBlocks[3], meta, meshBuilder, x, y, z, 270);
        }

        private void addCrossPiece(Block surroundingBlock, int meta, MeshBuilder meshBuilder, int x, int y, int z, int rotation) {
            if (surroundingBlock.isSolid || surroundingBlock == Block.fence) {
                meshBuilder.addCube(
                    this, Block.fence, meta,
                    new CubeComponent(
                        14, 17, 20,
                        18, 27, 32,
                        0, rotation, 0,
                        1),
                    RenderFace.ALL, x, y, z);
            }
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, int cubeIndex) {
            if(cubeIndex == 0) { // Post
                if(faceDirection.axis == EnumAxis.Y) {
                    return new UvPlane(block.texturePos, 0, 0, 8, 8);
                } else {
                    return new UvPlane(block.texturePos, 12, 0, 8, 32);
                }
            } else { // Beam
                if(faceDirection.axis == EnumAxis.Y) {
                    return new UvPlane(block.texturePos, 22, 0, 4, 12);
                } else {
                    return new UvPlane(block.texturePos, 0, 18, 12, 8);
                }
            }
        }
    }
}
