using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLogicPlate : BlockRendererPrimitive {

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(
                block, meta,
                new CubeComponent(0, 0, 0, 32, 4, 32),
                renderFace | RenderFace.U,
                x, y, z);
        }
    }
}
