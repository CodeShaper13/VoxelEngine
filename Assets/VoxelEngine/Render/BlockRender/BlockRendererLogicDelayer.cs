using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLogicDelayer : BlockRendererLogicPlate {

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            base.renderBlock(block, meta, meshBuilder, x, y, z, renderFace, surroundingBlocks);

            // Buttons
            this.addButton(meta, meshBuilder, x, y, z, 2, 6, 6);
            this.addButton(meta, meshBuilder, x, y, z, 3, 6, -6);
            this.addButton(meta, meshBuilder, x, y, z, 4, -6, 6);
            this.addButton(meta, meshBuilder, x, y, z, 5, -6, -6);
        }

        private void addButton(int meta, MeshBuilder meshBuilder, int x, int y, int z, int index, int xShift, int zShift) {
            int i = BitHelper.getBit(meta, index) ? -2 : 0;

            meshBuilder.addCube(
                this, Block.delayer, meta,
                new CubeComponent(
                    14 + xShift, 3 + i, 14 + zShift,
                    18 + xShift, 7 + i, 18 + zShift,
                    1),
                RenderFace.ALL, x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            if(cubeComponent.index == 0) {
                return base.getUvPlane(block, meta, faceDirection, cubeComponent);
            } else {
                return new UvPlane(new TexturePos(9, 1), 27, 27, 4, 4);
            }
        }
    }
}
