using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Render {

    public class MeshDataArray : MeshDataBase {

        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] uv;

        public Vector3[] colVertices;
        public int[] colTriangles;

        public bool useRenderDataForCol;

        private int vertIndex;
        private int triIndex;
        private int uvIndex;

        private int colVertIndex;
        private int colTriIndex;

        public MeshDataArray(int vertCount, int triCount) {
            this.vertices = new Vector3[vertCount];
            this.triangles = new int[triCount];
            this.uv = new Vector2[triCount];
            this.colVertices = new Vector3[vertCount];
            this.colTriangles = new int[triCount];
        }

        public override void generateQuad() {
            this.triangles[this.triIndex++] = this.vertIndex - 4;
            this.triangles[this.triIndex++] = this.vertIndex - 3;
            this.triangles[this.triIndex++] = this.vertIndex - 2;

            this.triangles[this.triIndex++] = this.vertIndex - 4;
            this.triangles[this.triIndex++] = this.vertIndex - 2;
            this.triangles[this.triIndex++] = this.vertIndex - 1;

            if (this.useRenderDataForCol) {
                this.colTriangles[this.colTriIndex++] = this.colVertIndex - 4;
                this.colTriangles[this.colTriIndex++] = this.colVertIndex - 3;
                this.colTriangles[this.colTriIndex++] = this.colVertIndex - 2;

                this.colTriangles[this.colTriIndex++] = this.colVertIndex - 4;
                this.colTriangles[this.colTriIndex++] = this.colVertIndex - 2;
                this.colTriangles[this.colTriIndex++] = this.colVertIndex - 1;
            }
        }

        public override void addVertex(Vector3 vertex) {
            this.vertices[this.vertIndex++] = vertex;
            if (this.useRenderDataForCol) {
                this.colVertices[this.colVertIndex] = vertex;
                this.colVertIndex += 1;
            }
        }

        public override void addTriangle(int tri) {
            this.triangles[this.triIndex++] = tri;
            //if (useRenderDataForCol) {
            //    this.colTriangles.Add(tri - (vertices.Count - colVertices.Count));
            //}
        }

        public override Mesh toMesh() {
            Mesh m = new Mesh();
            m.vertices = this.vertices;
            m.triangles = this.triangles;
            m.uv = this.uv;
            m.RecalculateNormals();
            return m;
        }

        public override int getVerticeCount() {
            return this.vertIndex;
        }

        public override void addUv(Vector2 uv) {
            throw new NotImplementedException();
        }

        public override void addUvRange(IEnumerable<Vector2> uv) {
            throw new NotImplementedException();
        }
    }
}
