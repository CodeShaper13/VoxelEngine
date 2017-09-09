using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererSlab : BlockRendererPrimitive {

        public BlockRendererSlab() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            if(BlockSlab.isFull(meta)) {
                RenderManager.CUBE.renderBlock(block, meta, meshBuilder, x, y, z, renderFace, surroundingBlocks);
            } else {
                BlockPos v = BlockSlab.getDirectionFromMeta(meta).blockPos;
                BlockPos size = new BlockPos(
                    v.x == 0 ? 16 : 8,
                    v.y == 0 ? 16 : 8,
                    v.z == 0 ? 16 : 8);

                meshBuilder.addCube(
                    this, block, meta,
                    new CubeComponent(
                        new BlockPos(16, 16, 16) + v * 8,
                        size.x, size.y, size.z,
                        0),
                    RenderFace.ALL, x, y, z);
            }
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            return new UvPlane(block.getTexturePos(faceDirection, meta), cubeComponent, faceDirection);

            /* UNUSED
            Direction slabDir = BlockSlab.getDirectionFromMeta(meta);
            if (faceDirection.axis == slabDir.axis) { // Full face
                return base.getUvPlane(block, meta, faceDirection, cubeComponent);
            } else {
                if(slabDir.axis == EnumAxis.X) {
                    return new UvPlane(block.getTexturePos(faceDirection, meta), slabDir == Direction.EAST ? 1 : 17, 0, 16, 32);
                } else if (slabDir.axis == EnumAxis.Y) {
                    return new UvPlane(block.getTexturePos(faceDirection, meta), 0, slabDir == Direction.DOWN ? 1 : 17, 32, 16); // GOOD!
                } else { // Z
                    return new UvPlane(block.getTexturePos(faceDirection, meta), slabDir == Direction.SOUTH ? 1 : 17, 0, 16, 32);
                }
            }
            */
        }
    }
}
