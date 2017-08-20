using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererWire : BlockRendererPrimitive {

        public BlockRendererWire() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            bool connectNorth =   BitHelper.getBit(meta, 0) == 1;
            bool connectUpNorth = BitHelper.getBit(meta, 1) == 1;
            bool connectEast =    BitHelper.getBit(meta, 2) == 1;
            bool connectUpEast =  BitHelper.getBit(meta, 3) == 1;
            bool connectSouth =   BitHelper.getBit(meta, 4) == 1;
            bool connectUpSouth = BitHelper.getBit(meta, 5) == 1;
            bool connectWest =    BitHelper.getBit(meta, 6) == 1;
            bool connectUpWest =  BitHelper.getBit(meta, 7) == 1;

            // Middle
            meshBuilder.addCube(block, meta,
                new CubeComponent(
                    connectWest ? 0 : 14, 1, connectSouth ? 0 : 14,
                    connectEast ? 32 : 18, 1, connectNorth ? 32 : 18),
                RenderFace.U, x, y, z);

            // Sides

            // Up Sides
            meshBuilder.useRenderDataForCol = false;

            BlockPos to = new BlockPos(14, 0, 31);
            BlockPos from = new BlockPos(18, 32, 31);
            // TODO cull faces, we only "draw" one.
            if(connectUpNorth) {
                meshBuilder.addCube(block, meta, new CubeComponent(to, from, new ComponentRotation(0, 0, 0)), RenderFace.ALL, x, y, z);
            }
            if (connectUpEast) {
                meshBuilder.addCube(block, meta, new CubeComponent(to, from, new ComponentRotation(0, 90, 0)), RenderFace.ALL, x, y, z);
            }
            if (connectUpSouth) {
                meshBuilder.addCube(block, meta, new CubeComponent(to, from, new ComponentRotation(0, 180, 0)), RenderFace.ALL, x, y, z);
            }
            if (connectUpWest) {
                meshBuilder.addCube(block, meta, new CubeComponent(to, from, new ComponentRotation(0, 270, 0)), RenderFace.ALL, x, y, z);
            }

            meshBuilder.useRenderDataForCol = true;


            /*
            float f1 = MathHelper.pixelToWorld(1);
            float f2 = f1 * 2;
            float f7 = MathHelper.pixelToWorld(7);


            Vector3 boxOrgin = new Vector3(x + xShift, y - MathHelper.pixelToWorld(15), z + zShift);

            // Top.
            meshBuilder.addBox(
                boxOrgin,
                new Vector3(
                    f2 + (connectEast  ? f7 : 0) + (connectWest  ? f7 : 0),
                    f1,
                    f2 + (connectNorth ? f7 : 0) + (connectSouth ? f7 : 0)),
                block, 0, RenderManager.TRUE_ARRAY);

            // Inside sides.
            meshBuilder.useRenderDataForCol = false;

            float p2 = MathHelper.pixelToWorld(2);

            // X
//            meshBuilder.addBox(boxOrgin, new Vector3(f1 * 2, f1, 0.5f), block, 1, new bool[] { false, true, false, true, false, false });
            // Z
//            meshBuilder.addBox(boxOrgin, new Vector3(0.5f, f1, f1 * 2), block, 1, new bool[] { true, false, true, false, false, false });

            meshBuilder.useRenderDataForCol = true;
            */
        }
    }
}
