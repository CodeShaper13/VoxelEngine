using UnityEngine;
using System.Collections.Generic;
using System;

namespace VoxelEngine.Render {

    public class MeshData : MeshDataBase {

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
        
        public override void addQuadTriangles() {
            this.triangles.Add(this.vertices.Count - 4);
            this.triangles.Add(this.vertices.Count - 3);
            this.triangles.Add(this.vertices.Count - 2);

            this.triangles.Add(this.vertices.Count - 4);
            this.triangles.Add(this.vertices.Count - 2);
            this.triangles.Add(this.vertices.Count - 1);

            if (this.useRenderDataForCol) {
                this.colTriangles.Add(this.colVertices.Count - 4);
                this.colTriangles.Add(this.colVertices.Count - 3);
                this.colTriangles.Add(this.colVertices.Count - 2);

                this.colTriangles.Add(this.colVertices.Count - 4);
                this.colTriangles.Add(this.colVertices.Count - 2);
                this.colTriangles.Add(this.colVertices.Count - 1);
            }
        }

        public void addQuadWithUVs(Vector2[] uvs) {
            this.addQuadTriangles();
            this.uv.AddRange(uvs);
        }

        public override void addVertex(Vector3 vertex) {
            this.vertices.Add(vertex);
            if (this.useRenderDataForCol) {
                this.colVertices.Add(vertex);
            }
        }

        public override void addTriangle(int tri) {
            this.triangles.Add(tri);
            if (useRenderDataForCol) {
                this.colTriangles.Add(tri - (vertices.Count - colVertices.Count));
            }
        }

        public override Mesh toMesh() {
            Mesh m = new Mesh();
            m.SetVertices(this.vertices);
            m.SetTriangles(this.triangles, 0);
            m.SetUVs(0, this.uv);
            m.RecalculateNormals();
            return m;
        }

        public override int getVerticeCount() {
            return this.vertices.Count;
        }

        public override void addUv(Vector2 uv) {
            this.uv.Add(uv);
        }

        public override void addUvRange(IEnumerable<Vector2> uv) {
            this.uv.AddRange(uv);
        }
    }
}