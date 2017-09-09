using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererTnt : BlockRendererPrimitive {

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    2, 0, 2,
                    30, 32, 30),
                renderFace | RenderFace.Y, x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            if(faceDirection.axis == EnumAxis.Y) {
                return new UvPlane(block.getTexturePos(faceDirection, meta), 3, 3, 28, 28);
            } else {
                return new UvPlane(block.getTexturePos(faceDirection, meta), 3, 1, 28, 32);
            }
        }
    }
}
