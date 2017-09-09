using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererCube : BlockRendererPrimitive {

        public BlockRendererCube() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            // North.
            if ((renderFace & 1) == 1) {
                meshBuilder.addOptimized1x1Plane(
                    this, block, meta,
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), //
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), //
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    Direction.NORTH);
            }
            // East.
            if (((renderFace >> 1) & 1) == 1) {
                meshBuilder.addOptimized1x1Plane(
                    this, block, meta,
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    Direction.EAST);
            }
            // South.
            if (((renderFace >> 2) & 1) == 1) {
                meshBuilder.addOptimized1x1Plane(
                    this, block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    Direction.SOUTH);
            }
            // West
            if (((renderFace >> 3) & 1) == 1) {
                meshBuilder.addOptimized1x1Plane(
                    this, block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    Direction.WEST);
            }
            // Up.
            if (((renderFace >> 4) & 1) == 1) {
                meshBuilder.addOptimized1x1Plane(
                    this, block, meta,                    
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    Direction.UP);
            }
            // Down.
            if (((renderFace >> 5) & 1) == 1) {
                meshBuilder.addOptimized1x1Plane(
                    this, block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    Direction.DOWN);
            }
        }
    }
}
