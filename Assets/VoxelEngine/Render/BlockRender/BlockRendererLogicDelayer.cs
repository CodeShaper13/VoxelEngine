using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLogicDelayer : BlockRendererLogicPlate {

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            base.renderBlock(block, meta, meshBuilder, x, y, z, renderFace, surroundingBlocks);

            // Buttons

            bool btn1 = BitHelper.getBit(meta, 2);
            bool btn2 = BitHelper.getBit(meta, 2);
            bool btn3 = BitHelper.getBit(meta, 2);
            bool btn4 = BitHelper.getBit(meta, 2);

            this.addButton(meta, meshBuilder, x, y, z, 2, 6, 6);
            this.addButton(meta, meshBuilder, x, y, z, 3, 6, -6);
            this.addButton(meta, meshBuilder, x, y, z, 4, -6, -6);
            this.addButton(meta, meshBuilder, x, y, z, 5, -6, 6);
        }

        private void addButton(int meta, MeshBuilder meshBuilder, int x, int y, int z, int index, int xShift, int zShift) {
            int i = BitHelper.getBit(meta, index) ? -2 : 0;
            meshBuilder.addCube(
                Block.delayer, meta,
                new CubeComponent(
                    14 + xShift, 3 + i, 14 + zShift,
                    18 + xShift, 7 + i, 18 + zShift),
                x, y, z, RenderFace.YU);
        }
    }
}
