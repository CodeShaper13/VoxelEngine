using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveLadder : BlockRendererPrimitive {

        private const float f = 0.45f;

        public override MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {

            if (meta == 2) {
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - f));
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - f));
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - f));
                meshData.generateQuad(b.getUVs(meta, Direction.NORTH, this.uvArray));
            } else if (meta == 3) {
                meshData.addVertex(new Vector3(x - f, y - 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x - f, y + 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x - f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x - f, y - 0.5f, z + 0.5f));
                meshData.generateQuad(b.getUVs(meta, Direction.EAST, this.uvArray));
            } else if (meta == 0) {
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + f));
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + f));
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + f));
                meshData.generateQuad(b.getUVs(meta, Direction.SOUTH, this.uvArray));
            } else {
                meshData.addVertex(new Vector3(x + f, y - 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x + f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x + f, y + 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x + f, y - 0.5f, z - 0.5f));
                meshData.generateQuad(b.getUVs(meta, Direction.WEST, this.uvArray));
            }
            return meshData;
        }
    }
}
