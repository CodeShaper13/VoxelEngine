using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererBed : BlockRendererPrimitive {

        public BlockRendererBed() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            //if(!BlockBed.isFoot(meta)) {
            // Body.
            int rotation = meta * 90;

                meshBuilder.addCube(
                    this, block, meta,
                    new CubeComponent(
                        0, 0, 0,
                        32, 16, 64,
                        0, rotation, 0),
                    RenderFace.U | RenderFace.D | RenderFace.E | RenderFace.W,
                    x, y, z);

                int mask = RenderFace.N | RenderFace.S;

                // Front.
                meshBuilder.addCube(
                    this, block, meta,
                    new CubeComponent(
                        0, 0, 63.9f,
                        32, 28, 64,
                        0, rotation, 0,
                        1),
                    mask,
                    x, y, z);

                // Back.
                meshBuilder.addCube(
                    this, block, meta,
                    new CubeComponent(
                        0, 0, 0.1f,
                        32, 20, 0,
                        0, rotation, 0,
                        2),
                    mask,
                    x, y, z);
            //}
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            if(cubeComponent.index == 0) {
                if (faceDirection == Direction.UP) {
                    return new UvPlane(new TexturePos(3, 6), 1, 1, 32, 64);
                } else if (faceDirection == Direction.DOWN) {
                    return new UvPlane(new TexturePos(2, 6), 1, 1, 32, 64);
                } else {
                    return new UvPlane(new TexturePos(6, 7), 1, 1, 64, 16);
                }
            } else if(cubeComponent.index == 1) { // Front.
                return new UvPlane(new TexturePos(5, 7), 1, 1, 32, 28);
            } else if(cubeComponent.index == 2) { // Back.
                return new UvPlane(new TexturePos(4, 7), 1, 1, 32, 20);
            }

            return base.getUvPlane(block, meta, faceDirection, cubeComponent);
        }
    }
}
