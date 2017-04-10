using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveLadder : BlockRendererPrimitive {

        private const float f = 0.45f;

        public override MeshBuilder renderBlock(Block b, byte meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {

            if (meta == 2) {
                meshData.addQuad(
                    new Vector3(x + 0.5f, y - 0.5f, z - f),
                    new Vector3(x + 0.5f, y + 0.5f, z - f),
                    new Vector3(x - 0.5f, y + 0.5f, z - f),
                    new Vector3(x - 0.5f, y - 0.5f, z - f),
                    b.getUVs(meta, Direction.NORTH, this.uvArray),
                    0);
            } else if (meta == 3) {
                meshData.addQuad(
                    new Vector3(x - f, y - 0.5f, z - 0.5f),
                    new Vector3(x - f, y + 0.5f, z - 0.5f),
                    new Vector3(x - f, y + 0.5f, z + 0.5f),
                    new Vector3(x - f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.EAST, this.uvArray),
                    0);
            } else if (meta == 0) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z + f),
                    new Vector3(x - 0.5f, y + 0.5f, z + f),
                    new Vector3(x + 0.5f, y + 0.5f, z + f),
                    new Vector3(x + 0.5f, y - 0.5f, z + f),
                    b.getUVs(meta, Direction.SOUTH, this.uvArray),
                    0);
            } else {
                meshData.addQuad(
                    new Vector3(x + f, y - 0.5f, z + 0.5f),
                    new Vector3(x + f, y + 0.5f, z + 0.5f),
                    new Vector3(x + f, y + 0.5f, z - 0.5f),
                    new Vector3(x + f, y - 0.5f, z - 0.5f),
                    b.getUVs(meta, Direction.WEST, this.uvArray),
                    0);
            }
            return meshData;
        }
    }
}
