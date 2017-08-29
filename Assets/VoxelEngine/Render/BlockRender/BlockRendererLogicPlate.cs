using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererLogicPlate : BlockRendererPrimitive {

        public BlockRendererLogicPlate() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            meshBuilder.addCube(
                this, block, meta,
                new CubeComponent(0, 0, 0, 32, 4, 32),
                renderFace | RenderFace.U,
                x, y, z);
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, int cubeIndex) {
            if (faceDirection.axis == EnumAxis.X || faceDirection.axis == EnumAxis.Z) {
                return new UvPlane(new TexturePos(9, 1), 0, 0, 32, 4);
            }
            else if (faceDirection == Direction.DOWN) {
                return new UvPlane(new TexturePos(9, 0), 0, 0, 32, 32); // Bottom
            }
            else {
                return new UvPlane(((BlockLogicBase)block).getTopTexture(meta * 90), 0, 0, 32, 32); // Top
            }
        }
    }
}
