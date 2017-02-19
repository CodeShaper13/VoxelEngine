using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine.Render {

    public abstract class MeshDataBase {

        public abstract void generateQuad();

        public abstract void addVertex(Vector3 vertex);

        public abstract void addTriangle(int tri);

        public abstract Mesh toMesh();

        public abstract int getVerticeCount();

        public abstract void addUv(Vector2 uv);

        public abstract void addUvRange(IEnumerable<Vector2> uv);
    }
}
