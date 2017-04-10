using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererMesh : BlockRenderer {

        private GameObject prefab;
        private Vector3[] cachedMeshVerts;
        private int[] cachedMeshTris;
        private Vector2[] cachedMeshUVs;
        private Bounds[] colliderArray;

        private Vector3 offsetVector;
        private bool randomMirror;
        private bool useMeshForCollision = true;

        public BlockRendererMesh(GameObject prefab) {
            this.prefab = prefab;
            List<Mesh> meshes = new List<Mesh>();
            List<Vector3> offsets = new List<Vector3>();

            this.extractMesh(this.prefab.transform, meshes, offsets);
            foreach(Transform trans in this.prefab.transform) {
                this.extractMesh(trans, meshes, offsets);
            }

            if(meshes.Count == 0) {
                Debug.Log("ERROR!  No MeshFilter components could be found on the Prefab!");
            } else if (meshes.Count == 1) {
                Mesh m = meshes[0];
                this.cachedMeshVerts = m.vertices;
                this.cachedMeshTris = m.triangles;
                this.cachedMeshUVs = m.uv;
            } else {
                List<Vector3> vertList = new List<Vector3>();
                List<int> triList = new List<int>();
                List<Vector2> uvList = new List<Vector2>();
                Vector3[] cachedVerts;
                for(int i = 0; i < meshes.Count; i++) {
                    Mesh m = meshes[i];
                    cachedVerts = m.vertices;
                    for(int j = 0; j < cachedVerts.Length; j++) {
                        vertList.Add((cachedVerts[j] + offsets[i]));
                    }
                    triList.AddRange(m.triangles);
                    uvList.AddRange(m.uv);
                }
                this.cachedMeshVerts = vertList.ToArray();
                this.cachedMeshTris = triList.ToArray();
                this.cachedMeshUVs = uvList.ToArray();
            }
        }

        public override MeshBuilder renderBlock(Block b, byte meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            int i;
            Vector3 v;
            // Broken, triangles normals are messed up when we scale model by -1
            //Vector3 sv = (this.flag ? this.pseudoRandomScale(new BlockPos(x, y, z).GetHashCode()) : Vector3.one);
            //Vector3 sv = Vector3.one;

            // Add the colliders
            if(meshData.useRenderDataForCol && !this.useMeshForCollision) { // Check useRenderDataForCol because it is false if we are rendering an item
                meshData.useRenderDataForCol = false;
                for(i = 0; i < this.colliderArray.Length; i++) {
                    meshData.addColliderBox(this.colliderArray[i], x + this.offsetVector.x, y + this.offsetVector.y, z + this.offsetVector.z);
                }
            }

            // Add vertices
            int vertStart = meshData.getVerticeCount();
            for(i = 0; i < this.cachedMeshVerts.Length; i++) {
                v = this.cachedMeshVerts[i];
                //meshData.addVertex(new Vector3((v.x * sv.x) + x + this.shiftVec.x, (v.y * sv.y) + y + this.shiftVec.y, (v.z * sv.z) + z + this.shiftVec.z));
                meshData.addVertex(new Vector3(v.x + x + this.offsetVector.x, v.y + y + this.offsetVector.y, v.z + z + this.offsetVector.z));
            }

            // Add triangles
            for (i = 0; i < this.cachedMeshTris.Length; i++) {
                meshData.addTriangle(vertStart + this.cachedMeshTris[i]);
            }

            // Add UVs
            for(i = 0; i < this.cachedMeshUVs.Length; i++) {
                meshData.addUv(this.cachedMeshUVs[i]);
            }
        
            return meshData;
        }

        /// <summary>
        /// Returns a random scale to flip the block based on its position hash.
        /// </summary>
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

        private void extractMesh(Transform t, List<Mesh> meshes, List<Vector3> offsets) {
            MeshFilter filter = t.GetComponent<MeshFilter>();
            if (filter != null) {
                meshes.Add(filter.sharedMesh);
                offsets.Add(t.localPosition);
            }
        }

        public BlockRendererMesh useRandomMirror() {
            this.randomMirror = true;
            return this;
        }

        public BlockRendererMesh setOffsetVector(Vector3 vec) {
            this.offsetVector = vec;
            return this;
        }

        public BlockRendererMesh useColliderComponent() {
            this.useMeshForCollision = false;
            BoxCollider[] bc = this.prefab.GetComponents<BoxCollider>();
            this.colliderArray = new Bounds[bc.Length];
            for (int i = 0; i < bc.Length; i++) {
                BoxCollider b = bc[i];
                this.colliderArray[i] = new Bounds(new Vector3(b.center.x, b.center.y, b.center.z), new Vector3(b.size.x, b.size.y, b.size.z));
            }
            return this;
        }
    }
}
