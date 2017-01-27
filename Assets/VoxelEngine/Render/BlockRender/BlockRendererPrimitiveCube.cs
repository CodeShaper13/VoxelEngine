using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveCube : BlockRendererPrimitive {

        public override MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace) {
            this.block = b;
            this.meta = meta;
            this.meshData = meshData;

            if (renderFace[0]) {
                this.renderNorthFace(x, y, z, meshData, this.getUVs(block, this.meta, Direction.NORTH));
            }
            if (renderFace[1]) {
                this.renderEastFace(x, y, z, meshData, this.getUVs(block, this.meta, Direction.EAST));
            }
            if (renderFace[2]) {
                this.renderSouthFace(x, y, z, meshData, this.getUVs(block, this.meta, Direction.SOUTH));
            }
            if (renderFace[3]) {
                this.renderWestFace(x, y, z, meshData, this.getUVs(block, this.meta, Direction.WEST));
            }
            if (renderFace[4]) {
                this.renderUpFace(x, y, z, meshData, this.getUVs(block, this.meta, Direction.UP));
            }
            if (renderFace[5]) {
                this.renderDownFace(x, y, z, meshData, this.getUVs(block, this.meta, Direction.DOWN));
            }

            return meshData;
        }

        private MeshData renderUpFace(int x, int y, int z, MeshData meshData, Vector2[] uv) {
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

            meshData.addQuadTriangles();
            meshData.uv.AddRange(uv);
            return meshData;
        }

        private MeshData renderDownFace(int x, int y, int z, MeshData meshData, Vector2[] uv) {
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.addQuadTriangles();
            meshData.uv.AddRange(uv);
            return meshData;
        }

        private MeshData renderNorthFace(int x, int y, int z, MeshData meshData, Vector2[] uv) {
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.addQuadTriangles();
            meshData.uv.AddRange(uv);
            return meshData;
        }

        private MeshData renderEastFace(int x, int y, int z, MeshData meshData, Vector2[] uv) {
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            meshData.addQuadTriangles();
            meshData.uv.AddRange(uv);
            return meshData;
        }

        private MeshData renderSouthFace(int x, int y, int z, MeshData meshData, Vector2[] uv) {
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            meshData.addQuadTriangles();
            meshData.uv.AddRange(uv);
            return meshData;
        }

        private MeshData renderWestFace(int x, int y, int z, MeshData meshData, Vector2[] uv) {
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

            meshData.addQuadTriangles();
            meshData.uv.AddRange(uv);
            return meshData;
        }
    }
}
