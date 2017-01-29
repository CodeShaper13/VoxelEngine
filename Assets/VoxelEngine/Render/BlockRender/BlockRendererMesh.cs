using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererMesh : BlockRenderer {

        private Mesh mesh;
        private Vector3 shiftVec;
        private bool flag = true;

        public BlockRendererMesh(Mesh mesh) {
            if(mesh == null) {
                Debug.Log("ERROR!  Model not set in References!");
                this.mesh = new Mesh();
            } else {
                this.mesh = mesh;
            }
        }

        public override MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace) {
            int i;

            // Broken, triangles nramls are messed up when we scale model by -1
            //Vector3 sv = (this.flag ? this.pseudoRandomScale(new BlockPos(x, y, z).GetHashCode()) : Vector3.one);
            Vector3 sv = Vector3.one;

            int vertStart = meshData.vertices.Count;
            for(i = 0; i < this.mesh.vertices.Length; i++) {
                Vector3 v = this.mesh.vertices[i];
                meshData.addVertex(new Vector3((v.x * sv.x) + x + this.shiftVec.x, (v.y * sv.y) + y + this.shiftVec.y, (v.z * sv.z) + z + this.shiftVec.z));
            }

            for (i = 0; i < this.mesh.triangles.Length; i++) {
                meshData.addTriangle(vertStart + this.mesh.triangles[i]);
            }

            // Uses error texture
            //for (i = 0; i < this.mesh.uv.Length / 4; i++) {
            //    meshData.uv.Add(new Vector2(0, 0));
            //    meshData.uv.Add(new Vector2(0, TexturePos.BLOCK_SIZE));
            //    meshData.uv.Add(new Vector2(TexturePos.BLOCK_SIZE, 0));
            //    meshData.uv.Add(new Vector2(TexturePos.BLOCK_SIZE, TexturePos.BLOCK_SIZE));
            //}

            for(i = 0; i < this.mesh.uv.Length; i++) {
                meshData.uv.Add(this.mesh.uv[i]);
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
