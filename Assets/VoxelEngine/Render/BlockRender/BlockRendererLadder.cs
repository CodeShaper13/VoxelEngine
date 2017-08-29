using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLadder : BlockRendererPrimitive {

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    0, 0, 31,
                    32, 32, 31,
                    0, meta * 90, 0),
                RenderFace.ALL,
                x, y, z);
        }
    }
}
