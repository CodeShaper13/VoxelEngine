using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveCube : BlockRendererPrimitive {

        private Vector2[] uvArray;

        public BlockRendererPrimitiveCube() {
            this.uvArray = new Vector2[4];
        }

        public override MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {

            if (renderFace[0]) {
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                meshData.addQuadTriangles();
                meshData.uv.AddRange(b.getUVs(meta, Direction.NORTH, this.uvArray));
            }
            if (renderFace[1]) {
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                meshData.addQuadTriangles();
                meshData.uv.AddRange(b.getUVs(meta, Direction.EAST, this.uvArray));
            }
            if (renderFace[2]) {
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                meshData.addQuadTriangles();
                meshData.uv.AddRange(b.getUVs(meta, Direction.SOUTH, this.uvArray));
            }
            if (renderFace[3]) {
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                meshData.addQuadTriangles();
                meshData.uv.AddRange(b.getUVs(meta, Direction.WEST, this.uvArray));
            }
            if (renderFace[4]) {
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                meshData.addQuadTriangles();
                meshData.uv.AddRange(b.getUVs(meta, Direction.UP, this.uvArray));
            }
            if (renderFace[5]) {
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                meshData.addQuadTriangles();
                meshData.uv.AddRange(b.getUVs(meta, Direction.DOWN, this.uvArray));
            }

            return meshData;
        }
    }
}
