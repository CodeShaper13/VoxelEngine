using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererChest : BlockRendererPrimitive {

        public BlockRendererChest() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshData, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            float f = MathHelper.pixelToWorld(14);

            meshData.addCube(
                this, block, meta,
                new CubeComponent(
                    2, 0, 2,
                    30, 28, 30),
                renderFace | RenderFace.Y | RenderFace.U,
                x, y, z);

            /*
            meshData.addBox(
                new Vector3(x, y - MathHelper.pixelToWorld(2), z),
                new Vector3(f, f, f),
                Block.chest,
                meta,
                RenderManager.TRUE_ARRAY);
                */
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            return new UvPlane(block.getTexturePos(faceDirection, meta), 3, 3, 28, 28);
        }
    }
}
