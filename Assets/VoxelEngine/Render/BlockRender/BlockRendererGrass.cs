using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererGrass : BlockRendererCube {

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            // Render the main grass cube.
            base.renderBlock(block, meta, meshBuilder, x, y, z, renderFace, surroundingBlocks);

            // Render top fluff
            if(RenderManager.instance.useFancyMeshes && ((renderFace >> 4) & 1) == 1) {
                int rndRot = (x << 16 + z) * 127;

                meshBuilder.autoGenerateColliders = false;
                this.forcedLightMode = 2;

                meshBuilder.addCube(
                    this, block, meta,
                    new CubeComponent(
                        -4, 0, 16,
                        36, 8, 16,
                        0, rndRot, 0,
                        1),
                    RenderFace.N | RenderFace.S, x, y + 1, z);

                meshBuilder.addCube(
                    this, block, meta,
                    new CubeComponent(
                        -4, 0, 16,
                        36, 8, 16,
                        0, rndRot + 90, 0,
                        1),
                    RenderFace.N | RenderFace.S, x, y + 1, z);

                this.forcedLightMode = 0;
                meshBuilder.autoGenerateColliders = true;
            }
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            if(cubeComponent.index == 0) {
                // Main block.
                return base.getUvPlane(block, meta, faceDirection, cubeComponent);
            } else {
                // Grass fluff.
                return new UvPlane(new TexturePos(3, 1), 1, 1, 32, 8);
            }
        }
    }
}
