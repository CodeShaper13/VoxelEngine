using UnityEngine;
using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererMesh : BlockRenderer {

        public Vector3[] cachedMeshVerts;
        public int[] cachedMeshTris;
        public Vector2[] cachedMeshUVs;

        private Mesh mesh;
        private Vector3 shiftVec;
        private bool flag = true;

        public BlockRendererMesh(Mesh mesh) {
            if(mesh == null) {
                Debug.Log("ERROR!  Model not set in References!");
                this.mesh = new Mesh();
            } else {
                this.mesh = mesh;
                this.cachedMeshVerts = this.mesh.vertices;
                this.cachedMeshTris = this.mesh.triangles;
                this.cachedMeshUVs = this.mesh.uv;
            }
        }

        public override MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            int i;
            Vector3 v;
            // Broken, triangles normals are messed up when we scale model by -1
            //Vector3 sv = (this.flag ? this.pseudoRandomScale(new BlockPos(x, y, z).GetHashCode()) : Vector3.one);
            //Vector3 sv = Vector3.one;

            int vertStart = meshData.getVerticeCount();
            for(i = 0; i < this.cachedMeshVerts.Length; i++) {
                v = this.cachedMeshVerts[i];
                //meshData.addVertex(new Vector3((v.x * sv.x) + x + this.shiftVec.x, (v.y * sv.y) + y + this.shiftVec.y, (v.z * sv.z) + z + this.shiftVec.z));
                meshData.addVertex(new Vector3(v.x + x + this.shiftVec.x, v.y + y + this.shiftVec.y, v.z + z + this.shiftVec.z));
            }

            for (i = 0; i < this.cachedMeshTris.Length; i++) {
                meshData.addTriangle(vertStart + this.cachedMeshTris[i]);
            }

            for(i = 0; i < this.cachedMeshUVs.Length; i++) {
                meshData.uv.Add(this.cachedMeshUVs[i]);
            }
        
            return meshData;
        }

        private Vector3 pseudoRandomScale(int hash) {
            int b = hash & 3; //Only use the first 2 bits
            if(b == 0) {
                return new Vector3(1, 1, 1);
            } else if(b == 1) {
                return new Vector3(-1, 1, 1);
            } else if(b == 2) {
                return new Vector3(1, 1, -1);
            } else {
                return new Vector3(-1, 1, -1);
            }
        }

        public BlockRendererMesh setUseRandomRot(bool flag) {
            this.flag = flag;
            return this;
        }

        public BlockRendererMesh setShiftVec(Vector3 vec) {
            this.shiftVec = vec;
            return this;
        }
    }
}
