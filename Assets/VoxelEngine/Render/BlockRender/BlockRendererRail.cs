using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererRail : BlockRendererPrimitive {

        public BlockRendererRail() {
            this.lookupAdjacentBlocks = true;
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    0, 1, 0,
                    32, 1, 32),
                RenderFace.U,
                x, y, z);

            return;
        }
    }
}
