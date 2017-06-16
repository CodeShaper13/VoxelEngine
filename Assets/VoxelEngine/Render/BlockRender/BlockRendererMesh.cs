using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererMesh : BlockRenderer {

        private GameObject prefab;
        private Vector3[] cachedMeshVerts;
        private int[] cachedMeshTris;
        private Vector2[] cachedMeshUVs;
        private Bounds[] colliderArray;

        private Vector3 offsetVector;
        private Quaternion modelRotation;
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
                this.correctVerticeRotations(m.vertices);
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
                this.correctVerticeRotations(vertList.ToArray());
                this.cachedMeshTris = triList.ToArray();
                this.cachedMeshUVs = uvList.ToArray();
            }
        }

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            int i;
            Vector3 vertice;

            // Add the colliders
            if(meshData.useRenderDataForCol && !this.useMeshForCollision) { // Check useRenderDataForCol because it is false if we are rendering an item
                for(i = 0; i < this.colliderArray.Length; i++) {
                    meshData.useRenderDataForCol = false;
                    meshData.addColliderBox(this.colliderArray[i], x + this.offsetVector.x, y + this.offsetVector.y, z + this.offsetVector.z);
                }
            }

            // Add vertices
            int vertStart = meshData.getVerticeCount();
            for(i = 0; i < this.cachedMeshVerts.Length; i++) {
                vertice = this.cachedMeshVerts[i];
                meshData.addVertex(new Vector3(vertice.x + x + this.offsetVector.x, vertice.y + y + this.offsetVector.y, vertice.z + z + this.offsetVector.z));
            }

            // Add triangles
            for (i = 0; i < this.cachedMeshTris.Length; i++) {
                meshData.addTriangle(vertStart + this.cachedMeshTris[i]);
            }

            // Add UVs
            for(i = 0; i < this.cachedMeshUVs.Length; i++) {
                meshData.addUv(this.cachedMeshUVs[i]);
            }            

            meshData.useRenderDataForCol = true;
        }

        private void extractMesh(Transform t, List<Mesh> meshes, List<Vector3> offsets) {
            MeshFilter filter = t.GetComponent<MeshFilter>();
            if (filter != null) {
                meshes.Add(filter.sharedMesh);
                offsets.Add(t.localPosition);
            }
        }

        /// <summary>
        /// Corrects the vertices rotation by rotating them -90 degrees because of Blender.
        /// </summary>
        private void correctVerticeRotations(Vector3[] verts) {
            this.cachedMeshVerts = new Vector3[verts.Length];
            Quaternion q = Quaternion.Euler(-90, 0, 0);
            Vector3 v;
            for (int i = 0; i < verts.Length; i++) {
                v = verts[i];
                this.cachedMeshVerts[i] = MathHelper.rotateVecAround(v, Vector3.zero, q);
            }
        }

        public BlockRendererMesh setRotation(Quaternion rot) {
            this.modelRotation = rot;
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
