using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    // TODO cull faces for wire up and down.
    public class BlockRendererWire : BlockRendererPrimitive {

        public BlockRendererWire() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            bool connectNorth = BitHelper.getBit(meta, 0);
            bool connectEast =  BitHelper.getBit(meta, 2);
            bool connectSouth = BitHelper.getBit(meta, 4);
            bool connectWest =  BitHelper.getBit(meta, 6);

            meshBuilder.addCube(this, block, meta,
                new CubeComponent(
                    connectWest ? 0 : 14, 1, connectSouth ? 0 : 14,
                    connectEast ? 32 : 18, 1, connectNorth ? 32 : 18),
                RenderFace.U, x, y, z);

            // Up Sides
            meshBuilder.useRenderDataForCol = false;

            for (int i = 0; i < 4; i++) {
                if(BitHelper.getBit(meta, (i * 2) + 1)) {
                    meshBuilder.addCube(
                        this, block, meta,
                        new CubeComponent(
                            14, 0, 31,
                            18, 32, 31,
                            0, i * 90, 0),
                        RenderFace.ALL, x, y, z);
                }
            }

            meshBuilder.useRenderDataForCol = true;            
        }

        public override UvPlane getUvPlane(Block block, int meta, Direction faceDirection, int cubeIndex) {
            TexturePos pos = block.getTexturePos(faceDirection, meta);
            if (faceDirection.axis == EnumAxis.X || faceDirection.axis == EnumAxis.Z) {
                return new UvPlane(pos, 14, 0, 4, 32);
            } else {
                // This must be the top face.
                return new UvPlane(
                    pos,
                    new Vector2(
                        BitHelper.getBit(meta, 6) ? 0 : 14,
                        BitHelper.getBit(meta, 4) ? 0 : 14),
                    new Vector2(
                        BitHelper.getBit(meta, 2) ? 31 : 17,
                        BitHelper.getBit(meta, 0) ? 31 : 17));
            }
        }
    }
}
