using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLadder : BlockRendererPrimitive {

        public BlockRendererLadder() {
            this.forcedLightMode = 1;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    2, 0, 29,
                    30, 32, 31,
                    0, meta * 90, 0),
                RenderFace.Y,
                x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            return new UvPlane(block.getTexturePos(faceDirection, meta), cubeComponent, faceDirection);
        }
    }
}
