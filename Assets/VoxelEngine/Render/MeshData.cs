using UnityEngine;
using System.Collections.Generic;

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
        
        public override void generateQuad() {
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

        public void generateQuad(Vector2[] uvs) {
            this.generateQuad();
            this.uv.AddRange(uvs);
        }

        public void addQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector2[] uvs) {
            this.addVertex(v1);
            this.addVertex(v2);
            this.addVertex(v3);
            this.addVertex(v4);
            this.generateQuad(uvs);
        }

        public override void addVertex(Vector3 vertex) {
            this.vertices.Add(vertex);
            if (this.useRenderDataForCol) {
                this.colVertices.Add(vertex);
            }
        }

        public void addColliderBox(Bounds b, float x, float y, float z) {
            int i = this.colVertices.Count - 1;
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.min.y, z + b.max.z));
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.min.y, z + b.min.z));
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.min.y, z + b.min.z));
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.min.y, z + b.max.z));

            this.colVertices.Add(new Vector3(x + b.max.x, y + b.max.y, z + b.max.z));
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.max.y, z + b.min.z));
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.max.y, z + b.min.z));
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.max.y, z + b.max.z));

            this.colTriangles.AddRange(new int[36] {
                i + 1, i + 6, i + 5, // + X
                i + 1, i + 2, i + 6, // + X
                i + 2, i + 3, i + 7,
                i + 7, i + 6, i + 2,
                i + 3, i + 4, i + 8,
                i + 8, i + 7, i + 3,
                i + 4, i + 1, i + 5,
                i + 5, i + 8, i + 4,
                i + 5, i + 6, i + 7, // Top
                i + 7, i + 8, i + 5, // Top
                i + 1, i + 4, i + 3, // Bottom
                i + 3, i + 2, i + 1  // Bottom
            });
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

        // Unused
        public override void addUv(Vector2 uv) {
            this.uv.Add(uv);
        }

        public override void addUvRange(IEnumerable<Vector2> uv) {
            this.uv.AddRange(uv);
        }
    }
}