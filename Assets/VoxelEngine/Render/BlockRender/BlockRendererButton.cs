using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererButton : BlockRendererPrimitive {

        public BlockRendererButton() {
            //this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            int i = Block.button.isPushed(meta) ? 2 : 0;
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    12, 12, 28 + i,
                    20, 20, 36 + i,
                    0, meta * 90, 0),
                RenderFace.ALL,
                x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            return new UvPlane(block.texturePos, 3, 23, 8, 8);
        }
    }
}
