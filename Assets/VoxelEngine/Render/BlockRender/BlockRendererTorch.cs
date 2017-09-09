using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererTorch : BlockRendererPrimitive {

        private const int OFFSET = 12;

        public BlockRendererTorch() {
            this.forcedLightMode = 1;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            int offsetX = 0;
            int offsetZ = 0;
            int rotX = 0;
            int rotY = 0;
            int rotZ = 0;

            if (meta == 1) { // North
                offsetZ = OFFSET;
                rotX = -15;
            } else if (meta == 2) { // East
                offsetX = OFFSET;
                rotZ = 15;
            } else if (meta == 3) { // South
                offsetZ = -OFFSET;
                rotX = 15;
            } else if (meta == 4) { // West
                offsetX = -OFFSET;
                rotZ = -15;
            }

            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    12, 0, 12,
                    20, 28, 20,
                    rotX, rotY, rotZ,
                    offsetX, meta == 0 ? 0 : 3, offsetZ),
                RenderFace.ALL, x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            bool isOut = block != Block.torch;
            if (faceDirection == Direction.UP) {
                return new UvPlane(block.texturePos, 3, isOut ? 13 : 3, 8, 8);
            } else if (faceDirection == Direction.DOWN) {
                return new UvPlane(block.texturePos, 3, 23, 8, 8);
            } else {
                return new UvPlane(block.texturePos, isOut ? 23 : 13, 3, 8, 28);
            }
        }
    }
}
