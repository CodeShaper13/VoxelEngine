using UnityEngine;
using System.Collections.Generic;

namespace VoxelEngine.Render {

    public class MeshData {
        public List<Vector3> vertices;
        public List<int> triangles;
        public List<Vector2> uv;

        public List<Vector3> colVertices;
        public List<int> colTriangles;

        public bool useRenderDataForCol;

        public MeshData() {
            this.vertices = new List<Vector3>();
            this.triangles = new List<int>();
            this.uv = new List<Vector2>();
            this.colVertices = new List<Vector3>();
            this.colTriangles = new List<int>();
        }

        public void addQuadTriangles() {
            this.triangles.Add(vertices.Count - 4);
            this.triangles.Add(vertices.Count - 3);
            this.triangles.Add(vertices.Count - 2);

            this.triangles.Add(vertices.Count - 4);
            this.triangles.Add(vertices.Count - 2);
            this.triangles.Add(vertices.Count - 1);

            if (this.useRenderDataForCol) {
                this.colTriangles.Add(colVertices.Count - 4);
                this.colTriangles.Add(colVertices.Count - 3);
                this.colTriangles.Add(colVertices.Count - 2);
                this.colTriangles.Add(colVertices.Count - 4);
                this.colTriangles.Add(colVertices.Count - 2);
                this.colTriangles.Add(colVertices.Count - 1);
            }
        }

        public void addVertex(Vector3 vertex) {
            this.vertices.Add(vertex);
            if (this.useRenderDataForCol) {
                this.colVertices.Add(vertex);
            }
        }

        public void addTriangle(int tri) {
            this.triangles.Add(tri);
            if (useRenderDataForCol) {
                this.colTriangles.Add(tri - (vertices.Count - colVertices.Count));
            }
        }

        public Mesh toMesh() {
            Mesh m = new Mesh();
            m.vertices = this.vertices.ToArray();
            m.triangles = this.triangles.ToArray();
            m.uv = this.uv.ToArray();
            m.RecalculateNormals();
            return m;
        }
    }
}