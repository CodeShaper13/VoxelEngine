using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render {

    public class MeshBuilder {

        private List<Vector3> vertices;
        private List<int> triangles;
        private List<Vector2> uv;

        private List<Vector3> colVertices;
        private List<int> colTriangles;
        /// <summary> The light levels of the block being rendered and the 6 adjacent.  0 = current, 1-6 = adjacent </summary>
        public int[] lightLevels;
        private List<Vector2> lightUvs;
        private int internalLightUvCount;

        private int[] cachedColliderPoints;

        public bool useRenderDataForCol;

        public MeshBuilder() {
            this.vertices = new List<Vector3>(65536);
            this.triangles = new List<int>(65536);
            this.uv = new List<Vector2>(65536);
            this.colVertices = new List<Vector3>(65536);
            this.colTriangles = new List<int>(65536);
            this.lightUvs = new List<Vector2>(65536);
            this.lightLevels = new int[7];
            this.cachedColliderPoints = new int[36];
            this.useRenderDataForCol = true;
        }

        public void addVertex(Vector3 vertex) {
            this.vertices.Add(vertex);
            if (this.useRenderDataForCol) {
                this.colVertices.Add(vertex);
            }
        }

        public void addTriangle(int triangle) {
            this.triangles.Add(triangle);
            if (this.useRenderDataForCol) {
                this.colTriangles.Add(triangle - (this.vertices.Count - this.colVertices.Count));
            }
        }

        public void addUv(Vector2 uv) {
            this.uv.Add(uv);

            float x = LightHelper.PIXEL_SIZE * this.lightLevels[0];
            float y = LightHelper.PIXEL_SIZE * this.lightLevels[0];
            Vector2 v = Vector2.zero;
            switch(this.internalLightUvCount) {
                case 0: v = new Vector2(x, y); break;
                case 1: v = new Vector2(x, y + LightHelper.PIXEL_SIZE); break;
                case 2: v = new Vector2(x + LightHelper.PIXEL_SIZE, y); break;
            }
            this.lightUvs.Add(v);

            this.internalLightUvCount += 1;
            if(this.internalLightUvCount == 3) {
                this.internalLightUvCount = 0;
            }
        }

        /// <summary>
        /// Adds a quad to the mesh
        /// </summary>
        public void addQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector2[] uvs, int lightSampleDirection) {
            // Add the 4 corner vertices.
            this.vertices.Add(v1);
            this.vertices.Add(v2);
            this.vertices.Add(v3);
            this.vertices.Add(v4);

            if(this.useRenderDataForCol) {
                this.colVertices.Add(v1);
                this.colVertices.Add(v2);
                this.colVertices.Add(v3);
                this.colVertices.Add(v4);
            }

            int i = this.vertices.Count;

            // Add the triangles to the quad.
            this.triangles.Add(i - 4);
            this.triangles.Add(i - 3);
            this.triangles.Add(i - 2);
            this.triangles.Add(i - 4);
            this.triangles.Add(i - 2);
            this.triangles.Add(i - 1);

            if (this.useRenderDataForCol) {
                i = this.colVertices.Count;
                this.colTriangles.Add(i - 4);
                this.colTriangles.Add(i - 3);
                this.colTriangles.Add(i - 2);
                this.colTriangles.Add(i - 4);
                this.colTriangles.Add(i - 2);
                this.colTriangles.Add(i - 1);
            }

            // Add the uvs.
            for(i = 0; i < uvs.Length; i++) {
                this.uv.Add(uvs[i]);
            }

            // Add light mapping.
            float x = LightHelper.PIXEL_SIZE * this.lightLevels[lightSampleDirection];
            float y = LightHelper.PIXEL_SIZE * this.lightLevels[lightSampleDirection];
            this.lightUvs.Add(new Vector2(x, y));
            this.lightUvs.Add(new Vector2(x, y + LightHelper.PIXEL_SIZE));
            this.lightUvs.Add(new Vector2(x + LightHelper.PIXEL_SIZE, y + LightHelper.PIXEL_SIZE));
            this.lightUvs.Add(new Vector2(x + LightHelper.PIXEL_SIZE, y));
        }

        public void addBox(Vector3 pos, Vector3 size, Block block, int meta, Vector2[] allocatedUvArray) {
            this.addBox(pos, size, Quaternion.identity, block, meta, allocatedUvArray);
        }

        /// <summary>
        /// Adds a rotated box of quads.  Note, adjacent lighting is not used on quads.
        /// </summary>
        //TODO make lighting work in the cases of fences and stuff
        public void addBox(Vector3 pos, Vector3 boxRadius, Quaternion rotation, Block block, int meta, Vector2[] allocatedUvArray) {
            // Top points.
            Vector3 ppp = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 ppn = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, boxRadius.y, -boxRadius.z), Vector3.zero, rotation);
            Vector3 npp = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 npn = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, boxRadius.y, -boxRadius.z), Vector3.zero, rotation);
            // Bottom points.
            Vector3 pnp = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, -boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 pnn = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, -boxRadius.y, -boxRadius.z), Vector3.zero, rotation);
            Vector3 nnp = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, -boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 nnn = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, -boxRadius.y, -boxRadius.z), Vector3.zero, rotation);

            // Top face.
            this.addQuad(npp, ppp, ppn, npn, UvHelper.cropUVs(block.getUVs(meta, Direction.UP, allocatedUvArray), new Vector2(boxRadius.z, boxRadius.x)), Direction.UP_ID);
            // Bottom face.
            this.addQuad(nnn, pnn, pnp, nnp, UvHelper.cropUVs(block.getUVs(meta, Direction.DOWN, allocatedUvArray), new Vector2(boxRadius.x, boxRadius.z)), Direction.DOWN_ID);            
            // +X face.
            this.addQuad(pnn, ppn, ppp, pnp, UvHelper.cropUVs(block.getUVs(meta, Direction.EAST, allocatedUvArray), new Vector2(boxRadius.z, boxRadius.y)), Direction.EAST_ID);
            // +Z face.
            this.addQuad(pnp, ppp, npp, nnp, UvHelper.cropUVs(block.getUVs(meta, Direction.NORTH, allocatedUvArray), new Vector2(boxRadius.x, boxRadius.y)), Direction.NORTH_ID);            
            // -X face.
            this.addQuad(nnp, npp, npn, nnn, UvHelper.cropUVs(block.getUVs(meta, Direction.WEST, allocatedUvArray), new Vector2(boxRadius.z, boxRadius.y)), Direction.WEST_ID);
            // -Z face.
            this.addQuad(nnn, npn, ppn, pnn, UvHelper.cropUVs(block.getUVs(meta, Direction.SOUTH, allocatedUvArray), new Vector2(boxRadius.x, boxRadius.y)), Direction.SOUTH_ID);
        }

        /// <summary>
        /// Adds a collider to the mesh in the form of a Bonds.  X, Y and Z are the blocks orgin.
        /// </summary>
        public void addColliderBox(Bounds b, float x, float y, float z) {
            int i = this.colVertices.Count - 1;
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.min.y, z + b.max.z)); // 1
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.min.y, z + b.min.z)); // 2
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.min.y, z + b.min.z)); // 3
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.min.y, z + b.max.z)); // 4

            this.colVertices.Add(new Vector3(x + b.max.x, y + b.max.y, z + b.max.z)); // 5
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.max.y, z + b.min.z)); // 6
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.max.y, z + b.min.z)); // 7
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.max.y, z + b.max.z)); // 8

            // +X
            this.cachedColliderPoints[0] = i + 1;
            this.cachedColliderPoints[1] = i + 6;
            this.cachedColliderPoints[2] = i + 5;
            this.cachedColliderPoints[3] = i + 1;
            this.cachedColliderPoints[4] = i + 2;
            this.cachedColliderPoints[5] = i + 6;
            
            // -Z
            this.cachedColliderPoints[6] = i + 2;
            this.cachedColliderPoints[7] = i + 3;
            this.cachedColliderPoints[8] = i + 7;
            this.cachedColliderPoints[9] = i + 7;
            this.cachedColliderPoints[10] = i + 6;
            this.cachedColliderPoints[11] = i + 2;
            
            // +X
            this.cachedColliderPoints[12] = i + 3;
            this.cachedColliderPoints[13] = i + 4;
            this.cachedColliderPoints[14] = i + 8;
            this.cachedColliderPoints[15] = i + 8;
            this.cachedColliderPoints[16] = i + 7;
            this.cachedColliderPoints[17] = i + 3;
            
            // +Z
            this.cachedColliderPoints[18] = i + 4;
            this.cachedColliderPoints[19] = i + 1;
            this.cachedColliderPoints[20] = i + 5;
            this.cachedColliderPoints[21] = i + 5;
            this.cachedColliderPoints[22] = i + 8;
            this.cachedColliderPoints[23] = i + 4;

            // +Y
            this.cachedColliderPoints[24] = i + 5;
            this.cachedColliderPoints[25] = i + 6;
            this.cachedColliderPoints[26] = i + 7;
            this.cachedColliderPoints[27] = i + 7;
            this.cachedColliderPoints[28] = i + 8;
            this.cachedColliderPoints[29] = i + 5;

            // -Y
            this.cachedColliderPoints[30] = i + 1;
            this.cachedColliderPoints[31] = i + 4;
            this.cachedColliderPoints[32] = i + 3;
            this.cachedColliderPoints[33] = i + 3;
            this.cachedColliderPoints[34] = i + 2;
            this.cachedColliderPoints[35] = i + 1;

            this.colTriangles.AddRange(this.cachedColliderPoints);
        }

        /// <summary>
        /// Converts the MeshData to a Mesh for rendering
        /// </summary>
        public Mesh toMesh() {
            Mesh mesh = new Mesh();
            mesh.SetVertices(this.vertices);
            mesh.SetTriangles(this.triangles, 0);
            mesh.SetUVs(0, this.uv);
            mesh.uv2 = this.lightUvs.ToArray();
            mesh.RecalculateNormals();
            return mesh;
        }

        /// <summary>
        /// Creates a collider mesh for chunk collision
        /// </summary>
        public Mesh getColliderMesh() {
            Mesh colMesh = new Mesh();
            colMesh.SetVertices(this.colVertices);
            colMesh.SetTriangles(this.colTriangles, 0);
            colMesh.RecalculateNormals();
            return colMesh;
        }

        /// <summary>
        /// Cleans up the meshData object, getting it ready to be used again
        /// </summary>
        public void cleanup() {
            this.vertices.Clear();
            this.triangles.Clear();
            this.uv.Clear();
            this.colVertices.Clear();
            this.colTriangles.Clear();

            this.internalLightUvCount = 0;
            this.lightUvs.Clear();
        }

        /// <summary>
        /// Returns the number of vertices in the MeshBuilder
        /// </summary>
        public int getVerticeCount() {
            return this.vertices.Count;
        }
    }
}