using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererMushroom : BlockRendererPrimitive {

        public BlockRendererMushroom() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            int rndRot = (x << 16 + z) * 127;

            // Stem.
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    12, 0, 12,
                    20, 6, 20,
                    0, rndRot, 0,
                    0),
                renderFace | RenderFace.YU,
                x, y, z);

            // Body.
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    10, 6, 10,
                    22, 12, 22,
                    0, rndRot, 0,
                    1),
                RenderFace.ALL,
                x, y, z);

            // Top.
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(
                    12, 12, 12,
                    20, 14, 20,
                    0, rndRot, 0,
                    2),
                RenderFace.YU,
                x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            int shift = meta == 0 ? 0 : 16;

            if (cubeComponent.index == 0) { // Stem.
                return new UvPlane(block.getTexturePos(faceDirection, meta), 3, 3, 8, 6);
            }
            else if (cubeComponent.index == 1) { // Body.
                return new UvPlane(block.getTexturePos(faceDirection, meta), 3 + shift, 11, 12, faceDirection.axis == EnumAxis.Y ? 12 : 6);
            }
            else { // Top.
                if(faceDirection.axis == EnumAxis.Y) {
                    return new UvPlane(block.getTexturePos(faceDirection, meta), 5 + shift, 13, 8, 8);
                }
                else {
                    return new UvPlane(block.getTexturePos(faceDirection, meta), 5 + shift, 25, 8, 2);
                }
            }
        }
    }
}
