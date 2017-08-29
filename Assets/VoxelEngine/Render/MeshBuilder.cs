using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Blocks;
using VoxelEngine.Util;
using VoxelEngine.Render.BlockRender;

namespace VoxelEngine.Render {

    public class MeshBuilder {

        /// <summary> The maximum number of vertices the Unity allows per Mesh. </summary>
        private const int UNITY_MAX_VERT = 65536;

        public bool useRenderDataForCol;

        private List<Vector3> vertices;
        private List<int> triangles;
        private List<Vector2> uv;
        private List<Color> vertexColors;

        private List<Vector3> colVertices;
        private List<int> colTriangles;
        /// <summary> A 3x3 area of the surrounding light levels. </summary>
        private int[] lightLevels;
        /// <summary> If true, added geometry will be used to generate colliders. </summary>
        private int[] cachedColliderPoints;
        private Vector2[] allocatedUvArray;

        public MeshBuilder() {
            this.vertices = new List<Vector3>(UNITY_MAX_VERT);
            this.triangles = new List<int>(UNITY_MAX_VERT);
            this.uv = new List<Vector2>(UNITY_MAX_VERT);
            this.vertexColors = new List<Color>(UNITY_MAX_VERT);
            this.colVertices = new List<Vector3>(UNITY_MAX_VERT);
            this.colTriangles = new List<int>(UNITY_MAX_VERT);
            this.lightLevels = new int[27];
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

        public void addVertexColor(Color color) {
            this.vertexColors.Add(color);
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
        /// Adds a single texture UV coordinate.
        /// </summary>
        public void addUv(Vector2 uv) {
            this.uv.Add(uv);

            /*
            Old light uv code.
            
            float i = LightHelper.PIXEL_SIZE * this.getLightLevel(0, 0, 0);
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
            */
        }

        /// <summary>
        /// Adds a quad to the mesh using the passed light sample direction.
        /// </summary>
        public void addQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector2[] uvs, int lightSampleDirection, bool flag = true) {
            // Add the 4 corner vertices.
            this.vertices.Add(v1);
            this.vertices.Add(v2);
            this.vertices.Add(v3);
            this.vertices.Add(v4);

            if(flag) {
                Color c = RenderManager.instance.lightHelper.getColorFromBrightness(this.getLightFromLegacySampleDir(lightSampleDirection));
                this.vertexColors.Add(c);
                this.vertexColors.Add(c);
                this.vertexColors.Add(c);
                this.vertexColors.Add(c);
            }

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
            this.triangles.Add(i - 1);
            this.triangles.Add(i - 1);
            this.triangles.Add(i - 3);
            this.triangles.Add(i - 2);

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

            /*
            Old light uv code.
            // Add light mapping.
            i = this.getLightFromLegacySampleDir(lightSampleDirection);
            float f = LightHelper.PIXEL_SIZE * i;
            this.lightUvs.Add(new Vector2(f, f)); // Bottom left
            this.lightUvs.Add(new Vector2(f, f + LightHelper.PIXEL_SIZE)); // Upper left
            this.lightUvs.Add(new Vector2(f + LightHelper.PIXEL_SIZE, f + LightHelper.PIXEL_SIZE)); // Upper right
            this.lightUvs.Add(new Vector2(f + LightHelper.PIXEL_SIZE, f)); // Bottom right
            */
        }

        /// <summary>
        /// Adds a one sided plane.
        /// </summary>
        public void addPlane(BlockRendererPrimitive renderer, Block block, int meta, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Direction direction, bool forceHardLighting = true) {
            this.addQuad(
                v0, v1, v2, v3,
                renderer.getUvPlane(block, meta, direction, 0).getMeshUvs(this.allocatedUvArray),
                direction.index,
                false);


            // Set vertex colors.
            bool useSmooth = !forceHardLighting | RenderManager.instance.useSmoothLighting;

            // Note! sampleAndSetVertexColor is called in the standard lower left clockwise order.  
            if (direction == Direction.NORTH) {
                if(useSmooth) {
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.DOWN);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.DOWN);
                } else {
                    this.sampleAndSetHardVertexColor(LightHelper.NORTH);
                }
            } else if (direction == Direction.EAST) {
                if (useSmooth) {
                    this.sampleAndSetSmoothVertexColor(direction, Direction.SOUTH, Direction.DOWN);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.SOUTH, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.NORTH, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.NORTH, Direction.DOWN);
                } else {
                    this.sampleAndSetHardVertexColor(LightHelper.EAST);
                }
            } else if(direction == Direction.SOUTH) {
                if (useSmooth) {
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.DOWN);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.DOWN);
                } else {
                    this.sampleAndSetHardVertexColor(LightHelper.SOUTH);
                }
            } else if (direction == Direction.WEST) {
                if (useSmooth) {
                    this.sampleAndSetSmoothVertexColor(direction, Direction.NORTH, Direction.DOWN);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.NORTH, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.SOUTH, Direction.UP);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.SOUTH, Direction.DOWN);
                } else {
                    this.sampleAndSetHardVertexColor(LightHelper.WEST);
                }
            } else if(direction == Direction.UP) {
                if (useSmooth) {
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.SOUTH);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.NORTH);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.NORTH);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.SOUTH);
                } else {
                    this.sampleAndSetHardVertexColor(LightHelper.UP);
                }
            } else if(direction == Direction.DOWN) {
                if (useSmooth) {
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.NORTH);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.WEST, Direction.SOUTH);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.SOUTH);
                    this.sampleAndSetSmoothVertexColor(direction, Direction.EAST, Direction.NORTH);
                } else {
                    this.sampleAndSetHardVertexColor(LightHelper.DOWN);
                }
            }
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
            mesh.SetColors(this.vertexColors);

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
            this.vertexColors.Clear();
            this.triangles.Clear();
            this.uv.Clear();
            this.colVertices.Clear();
            this.colTriangles.Clear();
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
            for(int i = 0; i < this.lightLevels.Length; i++) {
                this.lightLevels[i] = 15;
            }
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

        public void addCube(BlockRendererPrimitive renderer, Block block, int meta, CubeComponent cube, int renderFace, int worldX, int worldY, int worldZ) {
            // Lower, uv order
            Vector3 ppp = cube.from.toVector();
            Vector3 ppn = new Vector3(cube.from.x, cube.from.y, cube.to.z);
            Vector3 npn = new Vector3(cube.to.x, cube.from.y, cube.to.z);
            Vector3 npp = new Vector3(cube.to.x, cube.from.y, cube.from.z);

            // Upper
            Vector3 pnp = new Vector3(cube.from.x, cube.to.y, cube.from.z);
            Vector3 pnn = new Vector3(cube.from.x, cube.to.y, cube.to.z);
            Vector3 nnn = cube.to.toVector();
            Vector3 nnp = new Vector3(cube.to.x, cube.to.y, cube.from.z);

            // Rotate the cube.
            if (!cube.rotation.isZero()) {
                Vector3 pivot = new Vector3(16, 16, 16);
                Quaternion angle = cube.rotation.getAngle();
                ppp = MathHelper.rotateVecAround(ppp, pivot, angle);
                ppn = MathHelper.rotateVecAround(ppn, pivot, angle);
                npn = MathHelper.rotateVecAround(npn, pivot, angle);
                npp = MathHelper.rotateVecAround(npp, pivot, angle);
                pnp = MathHelper.rotateVecAround(pnp, pivot, angle);
                pnn = MathHelper.rotateVecAround(pnn, pivot, angle);
                nnn = MathHelper.rotateVecAround(nnn, pivot, angle);
                nnp = MathHelper.rotateVecAround(nnp, pivot, angle);
            }

            // Offset the cube.
            if(cube.offset != Vector3.zero) {
                ppp += cube.offset;
                ppn += cube.offset;
                npn += cube.offset;
                npp += cube.offset;
                pnp += cube.offset;
                pnn += cube.offset;
                nnn += cube.offset;
                nnp += cube.offset;
            }

            // Convert the pixel units to world units.
            ppp = this.pixelToWorld(ppp);
            ppn = this.pixelToWorld(ppn);
            npn = this.pixelToWorld(npn);
            npp = this.pixelToWorld(npp);
            pnp = this.pixelToWorld(pnp);
            pnn = this.pixelToWorld(pnn);
            nnn = this.pixelToWorld(nnn);
            nnp = this.pixelToWorld(nnp);

            Vector3 worldPos = new Vector3(worldX, worldY, worldZ);

            if(block == Block.button && Main.singleton.worldObj != null) {
                int j;
            }

            // Note: The vertices that check if the face is out of the cell, meaning it need adjacent light is arbitrary,
            // the same effect should be given requardless of the vertex.

            // North +X
            if ((renderFace & 1) == 1) {
                this.addQuad(
                    pnp + worldPos,
                    ppp + worldPos,
                    npp + worldPos,
                    nnp + worldPos,
                    renderer.getUvPlane(block, meta, Direction.NORTH, cube.index).getMeshUvs(this.allocatedUvArray),
                    pnp.x >= 0.5f ? LightHelper.NORTH : LightHelper.SELF);
            }
            // East +Z
            if (((renderFace >> 1) & 1) == 1) {
                this.addQuad(
                    pnn + worldPos,
                    ppn + worldPos,
                    ppp + worldPos,
                    pnp + worldPos,
                    renderer.getUvPlane(block, meta, Direction.EAST, cube.index).getMeshUvs(this.allocatedUvArray),
                    pnn.z >= 0.5f ? LightHelper.EAST : LightHelper.SELF);
            }
            // South -Z
            if (((renderFace >> 2) & 1) == 1) {
                this.addQuad(
                    nnn + worldPos,
                    npn + worldPos,
                    ppn + worldPos,
                    pnn + worldPos,
                    renderer.getUvPlane(block, meta, Direction.SOUTH, cube.index).getMeshUvs(this.allocatedUvArray),
                    nnn.z <= -0.5f ? LightHelper.SOUTH : LightHelper.SELF);
            }
            // West -X
            if (((renderFace >> 3) & 1) == 1) {
                this.addQuad(
                    nnp + worldPos,
                    npp + worldPos,
                    npn + worldPos,
                    nnn + worldPos,
                    renderer.getUvPlane(block, meta, Direction.WEST, cube.index).getMeshUvs(this.allocatedUvArray),
                    nnp.x <= -0.5f ? LightHelper.WEST : LightHelper.SELF);
            }
            // Up +Y
            if (((renderFace >> 4) & 1) == 1) {
                this.addQuad(
                    npn + worldPos,
                    npp + worldPos,
                    ppp + worldPos,
                    ppn + worldPos,
                    renderer.getUvPlane(block, meta, Direction.UP, cube.index).getMeshUvs(this.allocatedUvArray),
                    npn.y >= 0.5f ? LightHelper.UP : LightHelper.SELF);
            }
            // Down -Y
            if (((renderFace >> 5) & 1) == 1) {
                this.addQuad(
                    nnn + worldPos,
                    pnn + worldPos,
                    pnp + worldPos,
                    nnp + worldPos,
                    renderer.getUvPlane(block, meta, Direction.DOWN, cube.index).getMeshUvs(this.allocatedUvArray),
                    nnn.y <= -0.5f ? LightHelper.DOWN : LightHelper.SELF);
            }
        }

        public int getLightLevel(BlockPos pos) {
            return this.getLightLevel(pos.x, pos.y, pos.z);
        }

        public int getLightLevel(int xOffset, int yOffset, int zOffset) {
            return this.lightLevels[((yOffset + 1) * 3 * 3) + ((zOffset + 1) * 3) + (xOffset + 1)];
        }

        public void setLightLevel(int xOffset, int yOffset, int zOffset, int level) {
            this.lightLevels[((yOffset + 1) * 3 * 3) + ((zOffset + 1) * 3) + (xOffset + 1)] = level;
        }

        /// <summary>
        /// Returns the light level from the legacy lightSampleDir constants (0-6 inclusive).
        /// </summary>
        private int getLightFromLegacySampleDir(int lightSampleDirection) {
            switch (lightSampleDirection) {
                case 0: return this.getLightLevel(0, 0, 0);
                case 1: return this.getLightLevel(0, 0, 1);
                case 2: return this.getLightLevel(1, 0, 0);
                case 3: return this.getLightLevel(0, 0, -1);
                case 4: return this.getLightLevel(-1, 0, 0);
                case 5: return this.getLightLevel(0, 1, 0);
                case 6: return this.getLightLevel(0, -1, 0);
            }
            return this.getLightLevel(0, 0, 0);
        }

        private void sampleAndSetHardVertexColor(int lightSampleDir) {
            Color c = RenderManager.instance.lightHelper.getColorFromBrightness(this.getLightFromLegacySampleDir(lightSampleDir));
            this.vertexColors.Add(c);
            this.vertexColors.Add(c);
            this.vertexColors.Add(c);
            this.vertexColors.Add(c);
        }

        private void sampleAndSetSmoothVertexColor(Direction facing, Direction d1, Direction d2) {
            BlockPos orgin = facing.blockPos;
            float j = (
                this.getLightLevel(orgin) +
                this.getLightLevel(orgin + d1.blockPos) +
                this.getLightLevel(orgin + d1.blockPos + d2.blockPos) +
                this.getLightLevel(orgin + d2.blockPos)) / 4;
            this.vertexColors.Add(RenderManager.instance.lightHelper.getColorFromBrightness((int)j));
        }

        /// <summary>
        /// Used by the MeshBuilder to convert ComponentCube coords to world coords.
        /// </summary>
        private Vector3 pixelToWorld(Vector3 v) {
            v.x = (v.x / 32) - 0.5f;
            v.y = (v.y / 32) - 0.5f;
            v.z = (v.z / 32) - 0.5f;

            return v;
        }
    }
}
 