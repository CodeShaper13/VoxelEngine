using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Blocks;
using VoxelEngine.Util;
using VoxelEngine.Render.NewSys;

namespace VoxelEngine.Render {

    public class MeshBuilder {

        private const int UNITY_MAX_VERT = 65536;

        private List<Vector3> vertices;
        private List<int> triangles;
        private List<Vector2> uv;

        private List<Vector3> colVertices;
        private List<int> colTriangles;
        /// <summary> The light levels of the block being rendered and the 6 adjacent.  0 = current, 1-6 = adjacent </summary>
        public int[] lightLevels;
        private List<Vector2> lightUvs;
        private int internalLightUvCount;

        private int[] cachedColliderPoints;
        private Vector2[] allocatedUvArray;

        public bool useRenderDataForCol;

        public MeshBuilder() {
            this.vertices = new List<Vector3>(UNITY_MAX_VERT);
            this.triangles = new List<int>(UNITY_MAX_VERT);
            this.uv = new List<Vector2>(UNITY_MAX_VERT);
            this.colVertices = new List<Vector3>(UNITY_MAX_VERT);
            this.colTriangles = new List<int>(UNITY_MAX_VERT);
            this.lightUvs = new List<Vector2>(UNITY_MAX_VERT);
            this.lightLevels = new int[7];
            this.cachedColliderPoints = new int[36];
            this.allocatedUvArray = new Vector2[4];
            this.useRenderDataForCol = true;
        }

        /// <summary>
        /// Adds a single geometry vertex and the collider vertex if useRenderDataForCol is enabled.
        /// </summary>
        public void addVertex(Vector3 vertex) {
            this.vertices.Add(vertex);
            if (this.useRenderDataForCol) {
                this.colVertices.Add(vertex);
            }
        }

        /// <summary>
        /// Adds a single geometry triangle and the collider triangle if useRenderDataForCol is enabled.
        /// </summary>
        public void addTriangle(int triangle) {
            this.triangles.Add(triangle);
            if (this.useRenderDataForCol) {
                this.colTriangles.Add(triangle - (this.vertices.Count - this.colVertices.Count));
            }
        }

        /// <summary>
        /// Adds a single texture UV coordinate and a light map UV coordinate.
        /// </summary>
        public void addUv(Vector2 uv, int lightSampleDirection) {
            this.uv.Add(uv);

            float i = LightHelper.PIXEL_SIZE * this.lightLevels[lightSampleDirection];
            Vector2 v;
            switch (this.internalLightUvCount) {
                case 0: v = new Vector2(i, i); break;
                case 1: v = new Vector2(i, i + LightHelper.PIXEL_SIZE); break;
                default: v = new Vector2(i + LightHelper.PIXEL_SIZE, i); break;
            }
            this.lightUvs.Add(v);

            // Cycle through the internal counter.
            this.internalLightUvCount += 1;
            if (this.internalLightUvCount == 3) {
                this.internalLightUvCount = 0;
            }
        }

        /// <summary>
        /// Adds a quad to the mesh using the passed light sample direction.
        /// </summary>
        public void addQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector2[] uvs, int lightSampleDirection) {
            // Add the 4 corner vertices.
            this.vertices.Add(v1);
            this.vertices.Add(v2);
            this.vertices.Add(v3);
            this.vertices.Add(v4);

            if (this.useRenderDataForCol) {
                this.colVertices.Add(v1);
                this.colVertices.Add(v2);
                this.colVertices.Add(v3);
                this.colVertices.Add(v4);
            }

            int i = this.vertices.Count;

            // Add the triangles to the quad.
            this.triangles.Add(i - 4);
            this.triangles.Add(i - 3);
            this.triangles.Add(i - 2);
            this.triangles.Add(i - 4);
            this.triangles.Add(i - 2);
            this.triangles.Add(i - 1);

            if (this.useRenderDataForCol) {
                i = this.colVertices.Count;
                this.colTriangles.Add(i - 4);
                this.colTriangles.Add(i - 3);
                this.colTriangles.Add(i - 2);
                this.colTriangles.Add(i - 4);
                this.colTriangles.Add(i - 2);
                this.colTriangles.Add(i - 1);
            }

            // Add the uvs.
            for (i = 0; i < uvs.Length; i++) {
                this.uv.Add(uvs[i]);
            }

            // Add light mapping.
            float f = LightHelper.PIXEL_SIZE * this.lightLevels[lightSampleDirection];
            this.lightUvs.Add(new Vector2(f, f));
            this.lightUvs.Add(new Vector2(f, f + LightHelper.PIXEL_SIZE));
            this.lightUvs.Add(new Vector2(f + LightHelper.PIXEL_SIZE, f + LightHelper.PIXEL_SIZE));
            this.lightUvs.Add(new Vector2(f + LightHelper.PIXEL_SIZE, f));
        }

        /// <summary>
        /// Adds a one sided plane.  Warning, block.applyUvAlterations are called with Vector3.zero for faceRadius and faceOffset.
        /// </summary>
        public void addPlane(Block block, int meta, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Direction direction) {
            this.addQuad(
                v1, v2, v3, v4,
                block.getUvPlane(meta, direction).getMeshUvs(this.allocatedUvArray),
                direction.index);
        }
        
        public void addBox(Vector3 pos, Vector3 boxRadius, Block block, int meta, bool[] renderFace) {
            this.addBox(pos, boxRadius, Quaternion.identity, block, meta, renderFace);
        }

        /// <summary>
        /// Adds a rotated box of quads.  Warning, rotated boxes that extend to the edge of their voxel or past may have lighting errors!
        /// </summary>
        public void addBox(Vector3 pos, Vector3 boxRadius, Quaternion rotation, Block block, int meta, bool[] renderFace) {
            // Top points.
            Vector3 ppp = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 ppn = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, boxRadius.y, -boxRadius.z), Vector3.zero, rotation);
            Vector3 npp = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 npn = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, boxRadius.y, -boxRadius.z), Vector3.zero, rotation);
            // Bottom points.
            Vector3 pnp = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, -boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 pnn = pos + MathHelper.rotateVecAround(new Vector3(boxRadius.x, -boxRadius.y, -boxRadius.z), Vector3.zero, rotation);
            Vector3 nnp = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, -boxRadius.y, boxRadius.z), Vector3.zero, rotation);
            Vector3 nnn = pos + MathHelper.rotateVecAround(new Vector3(-boxRadius.x, -boxRadius.y, -boxRadius.z), Vector3.zero, rotation);

            Vector3 boxOffset = pos - MathHelper.roundVector3(pos);

            // +Z/North face.
            if (renderFace[0]) {
                this.addQuad(pnp, ppp, npp, nnp,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.NORTH, meta)),
                        meta,
                        Direction.NORTH,
                        new Vector2(boxRadius.x, boxRadius.y),
                        new Vector2(boxOffset.x, boxOffset.y)),
                    boxRadius.z >= 0.5f ? LightHelper.NORTH : LightHelper.SELF);
            }
            // +X/East face.
            if (renderFace[1]) {
                this.addQuad(pnn, ppn, ppp, pnp,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.EAST, meta)),
                        meta,
                        Direction.EAST,
                        new Vector2(boxRadius.z, boxRadius.y),
                        new Vector2(boxOffset.z, boxOffset.y)),
                    boxRadius.x >= 0.5f ? LightHelper.EAST : LightHelper.SELF);
            }
            // -Z/South face.
            if (renderFace[2]) {
                this.addQuad(nnn, npn, ppn, pnn,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.SOUTH, meta)),
                        meta,
                        Direction.SOUTH,
                        new Vector2(boxRadius.x, boxRadius.y),
                        new Vector2(boxOffset.x, boxOffset.y)),
                    boxRadius.z <= -0.5f ? LightHelper.SOUTH : LightHelper.SELF);
            }
            // -X/West face.
            if (renderFace[3]) {
                this.addQuad(nnp, npp, npn, nnn,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.WEST, meta)),
                        meta,
                        Direction.WEST,
                        new Vector2(boxRadius.z, boxRadius.y),
                        new Vector2(boxOffset.z, boxOffset.y)),
                    boxRadius.x <= -0.5f ? LightHelper.WEST : LightHelper.SELF);
            }
            // +Y/Up face.
            if (renderFace[4]) {
                this.addQuad(npn, npp, ppp, ppn,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.UP, meta)),
                        meta,
                        Direction.UP,
                        new Vector2(boxRadius.z, boxRadius.x),
                        new Vector2(boxOffset.z, boxOffset.x)),
                    boxRadius.y >= 0.5f ? LightHelper.UP : LightHelper.SELF);
            }
            // -Y/Down face.
            if (renderFace[5]) {
                this.addQuad(nnn, pnn, pnp, nnp,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.DOWN, meta)),
                        meta,
                        Direction.DOWN,
                        new Vector2(boxRadius.x, boxRadius.z),
                        new Vector2(boxOffset.x, boxOffset.z)),
                    boxRadius.y <= -0.5f ? LightHelper.DOWN : LightHelper.SELF);
            }
        }

        /// <summary>
        /// Adds a collider to the mesh in the form of a Bounds.  X, Y and Z are the block's orgin.
        /// </summary>
        public void addColliderBox(Bounds b, float x, float y, float z) {
            int i = this.colVertices.Count - 1;
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.min.y, z + b.max.z)); // 1
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.min.y, z + b.min.z)); // 2
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.min.y, z + b.min.z)); // 3
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.min.y, z + b.max.z)); // 4

            this.colVertices.Add(new Vector3(x + b.max.x, y + b.max.y, z + b.max.z)); // 5
            this.colVertices.Add(new Vector3(x + b.max.x, y + b.max.y, z + b.min.z)); // 6
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.max.y, z + b.min.z)); // 7
            this.colVertices.Add(new Vector3(x + b.min.x, y + b.max.y, z + b.max.z)); // 8

            // +X
            this.cachedColliderPoints[0] = i + 1;
            this.cachedColliderPoints[1] = i + 6;
            this.cachedColliderPoints[2] = i + 5;
            this.cachedColliderPoints[3] = i + 1;
            this.cachedColliderPoints[4] = i + 2;
            this.cachedColliderPoints[5] = i + 6;

            // -Z
            this.cachedColliderPoints[6] = i + 2;
            this.cachedColliderPoints[7] = i + 3;
            this.cachedColliderPoints[8] = i + 7;
            this.cachedColliderPoints[9] = i + 7;
            this.cachedColliderPoints[10] = i + 6;
            this.cachedColliderPoints[11] = i + 2;

            // +X
            this.cachedColliderPoints[12] = i + 3;
            this.cachedColliderPoints[13] = i + 4;
            this.cachedColliderPoints[14] = i + 8;
            this.cachedColliderPoints[15] = i + 8;
            this.cachedColliderPoints[16] = i + 7;
            this.cachedColliderPoints[17] = i + 3;

            // +Z
            this.cachedColliderPoints[18] = i + 4;
            this.cachedColliderPoints[19] = i + 1;
            this.cachedColliderPoints[20] = i + 5;
            this.cachedColliderPoints[21] = i + 5;
            this.cachedColliderPoints[22] = i + 8;
            this.cachedColliderPoints[23] = i + 4;

            // +Y
            this.cachedColliderPoints[24] = i + 5;
            this.cachedColliderPoints[25] = i + 6;
            this.cachedColliderPoints[26] = i + 7;
            this.cachedColliderPoints[27] = i + 7;
            this.cachedColliderPoints[28] = i + 8;
            this.cachedColliderPoints[29] = i + 5;

            // -Y
            this.cachedColliderPoints[30] = i + 1;
            this.cachedColliderPoints[31] = i + 4;
            this.cachedColliderPoints[32] = i + 3;
            this.cachedColliderPoints[33] = i + 3;
            this.cachedColliderPoints[34] = i + 2;
            this.cachedColliderPoints[35] = i + 1;

            this.colTriangles.AddRange(this.cachedColliderPoints);
        }

        /// <summary>
        /// Converts the MeshData to a Mesh for rendering
        /// </summary>
        public Mesh getGraphicMesh() {
            Mesh mesh = new Mesh();
            mesh.SetVertices(this.vertices);
            mesh.SetTriangles(this.triangles, 0);
            mesh.SetUVs(0, this.uv);
            mesh.uv2 = this.lightUvs.ToArray();
            return mesh;
        }

        /// <summary>
        /// Creates a collider mesh for chunk collision
        /// </summary>
        public Mesh getColliderMesh() {
            Mesh colMesh = new Mesh();
            colMesh.SetVertices(this.colVertices);
            colMesh.SetTriangles(this.colTriangles, 0);
            colMesh.RecalculateNormals();
            return colMesh;
        }

        /// <summary>
        /// Cleans up the meshData object, getting it ready to be used again
        /// </summary>
        public void cleanup() {
            this.vertices.Clear();
            this.triangles.Clear();
            this.uv.Clear();
            this.colVertices.Clear();
            this.colTriangles.Clear();

            this.internalLightUvCount = 0;
            this.lightUvs.Clear();
        }

        /// <summary>
        /// Returns the number of vertices in the MeshBuilder.
        /// </summary>
        public int getVerticeCount() {
            return this.vertices.Count;
        }

        /// <summary>
        /// Fills the light lookup table with max light of 15.  Used by blocks and items in the hud.
        /// </summary>
        public void setMaxLight() {
            this.lightLevels[0] = 15;
            this.lightLevels[1] = 15;
            this.lightLevels[2] = 15;
            this.lightLevels[3] = 15;
            this.lightLevels[4] = 15;
            this.lightLevels[5] = 15;
            this.lightLevels[6] = 15;
        }

        public Vector2[] generateUVsFromTP(TexturePos tilePos) {
            float x = TexturePos.BLOCK_SIZE * tilePos.x;
            float y = TexturePos.BLOCK_SIZE * tilePos.y;
            this.allocatedUvArray[0] = new Vector2(x, y);
            this.allocatedUvArray[1] = new Vector2(x, y + TexturePos.BLOCK_SIZE);
            this.allocatedUvArray[2] = new Vector2(x + TexturePos.BLOCK_SIZE, y + TexturePos.BLOCK_SIZE);
            this.allocatedUvArray[3] = new Vector2(x + TexturePos.BLOCK_SIZE, y);
            if (tilePos.rotation != 0) {
                UvHelper.rotateUVs(this.allocatedUvArray, tilePos.rotation);
            }
            return this.allocatedUvArray;
        }

        public void addCube(Block block, int meta, CubeComponent cube, int renderFace, int worldX, int worldY, int worldZ) {
            // Lower, uv order
            BlockPos ppp = cube.from;
            BlockPos ppn = new BlockPos(cube.from.x, cube.from.y, cube.to.z);
            BlockPos npn = new BlockPos(cube.to.x, cube.from.y, cube.to.z);
            BlockPos npp = new BlockPos(cube.to.x, cube.from.y, cube.from.z);

            // Upper
            BlockPos pnp = new BlockPos(cube.from.x, cube.to.y, cube.from.z);
            BlockPos pnn = new BlockPos(cube.from.x, cube.to.y, cube.to.z);
            BlockPos nnn = cube.to;
            BlockPos nnp = new BlockPos(cube.to.x, cube.to.y, cube.from.z);

            if(!cube.rotation.isZero()) {
                BlockPos pivot = new BlockPos(16);
                Quaternion angle = cube.rotation.getAngle();
                ppp = ppp.rotateAround(pivot, angle);
                ppn = ppn.rotateAround(pivot, angle);
                npn = npn.rotateAround(pivot, angle);
                npp = npp.rotateAround(pivot, angle);
                pnp = pnp.rotateAround(pivot, angle);
                pnn = pnn.rotateAround(pivot, angle);
                nnn = nnn.rotateAround(pivot, angle);
                nnp = nnp.rotateAround(pivot, angle);
            }

            if(!cube.offset.isZero()) {
                ppp += cube.offset;
                ppn += cube.offset;
                npn += cube.offset;
                npp += cube.offset;
                pnp += cube.offset;
                pnn += cube.offset;
                nnn += cube.offset;
                nnp += cube.offset;
            }

            Vector3 worldPos = new Vector3(worldX, worldY, worldZ);

            // North +X
            if ((renderFace & 1) == 1) {
                this.addQuad(
                    pnp.func() + worldPos,
                    ppp.func() + worldPos,
                    npp.func() + worldPos,
                    nnp.func() + worldPos,
                    block.getUvPlane(meta, Direction.NORTH).getMeshUvs(this.allocatedUvArray),
                    cube.from.x > 32 ? LightHelper.NORTH : LightHelper.SELF);
            }
            // East +Z
            if (((renderFace >> 1) & 1) == 1) {
                this.addQuad(
                    pnn.func() + worldPos,
                    ppn.func() + worldPos,
                    ppp.func() + worldPos,
                    pnp.func() + worldPos,
                    block.getUvPlane(meta, Direction.EAST).getMeshUvs(this.allocatedUvArray),
                    cube.from.z > 32 ? LightHelper.EAST : LightHelper.SELF);
            }
            // South -Z
            if (((renderFace >> 2) & 1) == 1) {
                this.addQuad(
                    nnn.func() + worldPos,
                    npn.func() + worldPos,
                    ppn.func() + worldPos,
                    pnn.func() + worldPos,
                    block.getUvPlane(meta, Direction.SOUTH).getMeshUvs(this.allocatedUvArray),
                    cube.to.x < 0 ? LightHelper.SOUTH : LightHelper.SELF);
            }
            // West -X
            if (((renderFace >> 3) & 1) == 1) {
                this.addQuad(
                    nnp.func() + worldPos,
                    npp.func() + worldPos,
                    npn.func() + worldPos,
                    nnn.func() + worldPos,
                    block.getUvPlane(meta, Direction.WEST).getMeshUvs(this.allocatedUvArray),
                    cube.to.x < 0 ? LightHelper.WEST : LightHelper.SELF);
            }
            // Up +Y
            if (((renderFace >> 4) & 1) == 1) {
                this.addQuad(
                    npn.func() + worldPos,
                    npp.func() + worldPos,
                    ppp.func() + worldPos,
                    ppn.func() + worldPos,
                    block.getUvPlane(meta, Direction.UP).getMeshUvs(this.allocatedUvArray),
                    cube.from.z > 32 ? LightHelper.UP : LightHelper.SELF);
            }
            // Down -Y //TODO fix UVs!
            if (((renderFace >> 5) & 1) == 1) {
                this.addQuad(
                    nnn.func() + worldPos,
                    pnn.func() + worldPos,
                    pnp.func() + worldPos,
                    nnp.func() + worldPos,
                    block.getUvPlane(meta, Direction.DOWN).getMeshUvs(this.allocatedUvArray),
                    cube.to.x < 0 ? LightHelper.DOWN : LightHelper.SELF);
            }
        }
    }
}