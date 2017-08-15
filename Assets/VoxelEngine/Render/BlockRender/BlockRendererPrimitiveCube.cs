using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public sealed /* Stops me from screwing up and extending this instead of BlockRendererPrimitive */ class BlockRendererPrimitiveCube : BlockRendererPrimitive {

        public BlockRendererPrimitiveCube() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            // North
            if (renderFace[0]) {
                meshBuilder.addPlane(block, meta,
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    Direction.NORTH);
            }
            // East
            if (renderFace[1]) {
                meshBuilder.addPlane(block, meta,
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    Direction.EAST);
            }
            // South
            if (renderFace[2]) {
                meshBuilder.addPlane(block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    Direction.SOUTH);
            }
            // West
            if (renderFace[3]) {
                meshBuilder.addPlane(block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    Direction.WEST);
            }
            // Up
            if (renderFace[4]) {
                meshBuilder.addPlane(block, meta,
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    Direction.UP);
            }
            // Down
            if (renderFace[5]) {
                meshBuilder.addPlane(block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    Direction.DOWN);
            }
        }
    }
}
