using UnityEngine;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererRail3d : BlockRendererMesh {

        public BlockRendererRail3d(GameObject prefab) : base(prefab) {
        }

        /*
        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            int i;
            Vector3 v;
            // Broken, triangles normals are messed up when we scale model by -1
            //Vector3 sv = (this.flag ? this.pseudoRandomScale(new BlockPos(x, y, z).GetHashCode()) : Vector3.one);
            //Vector3 sv = Vector3.one;

            // Add the colliders
            if (meshData.useRenderDataForCol && !this.useMeshForCollision) { // Check useRenderDataForCol because it is false if we are rendering an item
                for (i = 0; i < this.colliderArray.Length; i++) {
                    meshData.useRenderDataForCol = false;
                    meshData.addColliderBox(this.colliderArray[i], x + this.offsetVector.x, y + this.offsetVector.y, z + this.offsetVector.z);
                }
            }

            // Add vertices
            int vertStart = meshData.getVerticeCount();
            for (i = 0; i < this.cachedMeshVerts.Length; i++) {
                v = this.cachedMeshVerts[i];
                //meshData.addVertex(new Vector3((v.x * sv.x) + x + this.shiftVec.x, (v.y * sv.y) + y + this.shiftVec.y, (v.z * sv.z) + z + this.shiftVec.z));
                meshData.addVertex(new Vector3(v.x + x + this.offsetVector.x, v.y + y + this.offsetVector.y, v.z + z + this.offsetVector.z));
            }

            // Add triangles
            for (i = 0; i < this.cachedMeshTris.Length; i++) {
                meshData.addTriangle(vertStart + this.cachedMeshTris[i]);
            }

            // Add UVs
            for (i = 0; i < this.cachedMeshUVs.Length; i++) {
                meshData.addUv(this.cachedMeshUVs[i]);
            }

            meshData.useRenderDataForCol = true;
        }
        */
    }
}
