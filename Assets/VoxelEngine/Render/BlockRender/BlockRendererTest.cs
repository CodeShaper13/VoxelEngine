using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererTest : BlockRendererPrimitive {

        public BlockRendererTest() {
            this.lookupAdjacentBlocks = true;
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(this, block, meta, new CubeComponent(0, 0, 0, 32, 32, 32, 0, 90, 0), renderFace, x, y, z);
        }
    }
}
