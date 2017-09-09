using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererStairs : BlockRendererPrimitive {

        public BlockRendererStairs() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            // Bottom.
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    0, 0, 0,
                    32, 16, 32,
                    0),
                renderFace | RenderFace.U, x, y, z);

            Direction facing = BlockStairs.getDirectionFromMeta(meta);
            BlockPos v = facing.blockPos;
            BlockPos size = new BlockPos(
                v.x == 0 ? 16 : 8,
                8,
                v.z == 0 ? 16 : 8);

            // Top.
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    new BlockPos(16, 24, 16) + v * 8,
                        size.x, size.y, size.z,
                        1),
                renderFace | facing.renderMask,
                x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            return new UvPlane(block.getTexturePos(faceDirection, meta), cubeComponent, faceDirection);
        }
    }
}
