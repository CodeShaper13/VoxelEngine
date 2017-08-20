using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererTorch : BlockRendererPrimitive {

        private const int ROTATION = 15;
        public const int OFFSET = 12;

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            int offsetX = 0;
            int offsetZ = 0;
            int rotX = 0;
            int rotY = 0;
            int rotZ = 0;

            if (meta == 1) { // North
                offsetZ = OFFSET;
                rotX = -ROTATION;
            } else if (meta == 2) { // East
                offsetX = OFFSET;
                rotZ = ROTATION;
            } else if (meta == 3) { // South
                offsetZ = -OFFSET;
                rotX = ROTATION;
            } else if (meta == 4) { // West
                offsetX = -OFFSET;
                rotZ = -ROTATION;
            }

            meshBuilder.addCube(
                block, meta,
                new CubeComponent(
                    12, 0, 12,
                    20, 28, 20,
                    rotX, rotY, rotZ,
                    offsetX, 0, offsetZ),
                renderFace | RenderFace.YU, x, y, z);
        }
    }
}
