using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    // TODO make collider only a single tri.
    public class BlockRendererCobweb : BlockRendererPrimitive {

        public BlockRendererCobweb() {
            this.forcedLightMode = 1;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    16, 0, 0,
                    16, 32, 32,
                    0, meta * 90, 0),
                RenderFace.ALL, x, y, z);
        }
    }
}
