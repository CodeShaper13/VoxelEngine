using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Blocks;
using VoxelEngine.Util;
using VoxelEngine.Render.BlockRender;
using System;

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
        public void addTriangle(int triIndex) {
            this.triangles.Add(triIndex);
            if (this.useRenderDataForCol) {
                this.colTriangles.Add(triIndex - (this.vertices.Count - this.colVertices.Count));
            }
        }

        /// <summary>
        /// Adds a single texture UV coordinate.
        /// </summary>
        public void addUv(Vector2 uv) {
            this.uv.Add(uv);
        }

        /// <summary>
        /// Adds a basic quad to the mesh with texture uvs.  If lightSampleDirection != null, light will be looked up from
        /// the legacy light sample direction system (0- 6).  If vertex colors are not added here, the calling method must
        /// add them in another way.
        /// </summary>
        public void addQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector2[] uvs, int lightSampleDirection = -1) {
            // Add the 4 vertices.
            this.vertices.Add(v0);
            this.vertices.Add(v1);
            this.vertices.Add(v2);
            this.vertices.Add(v3);

            // Add vertex colors.
            if (lightSampleDirection != -1) {
                Color c = RenderManager.instance.lightColors.getColorFromBrightness(this.getLightFromLegacySampleDir(lightSampleDirection));
                this.vertexColors.Add(c);
                this.vertexColors.Add(c);
                this.vertexColors.Add(c);
                this.vertexColors.Add(c);
            }

            int i = this.vertices.Count;

            // Add the triangles to the quad.
            this.triangles.Add(i - 4);
            this.triangles.Add(i - 3);
            this.triangles.Add(i - 1);
            this.triangles.Add(i - 1);
            this.triangles.Add(i - 3);
            this.triangles.Add(i - 2);

            // Add collider vericies and triangles.
            if (this.useRenderDataForCol) {
                this.colVertices.Add(v0);
                this.colVertices.Add(v1);
                this.colVertices.Add(v2);
                this.colVertices.Add(v3);

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
        }

        /// <summary>
        /// Adds a plane.  Optimized for use in BlockRendererCube and this shouldn't be used for anything else.
        /// </summary>
        public void addOptimized1x1Plane(BlockRendererPrimitive renderer, Block block, int meta, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Direction direction) {
            this.addQuad(
                v0,v1, v2, v3,
                renderer.getUvPlane(block, meta, direction, 0).getMeshUvs(this.allocatedUvArray));

            // Set vertex colors.
            bool useSmooth = RenderManager.instance.useSmoothLighting;

            // Note! sampleAndSetVertexColor is called in the standard lower left clockwise order.  
            if (direction == Direction.NORTH) {
                if(useSmooth) {
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.DOWN);
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.DOWN);
                } else {
                    this.color4Vertices(LightSampleDirection.NORTH);
                }
            } else if (direction == Direction.EAST) {
                if (useSmooth) {
                    this.sampleFor1x1Plane(direction, Direction.SOUTH, Direction.DOWN);
                    this.sampleFor1x1Plane(direction, Direction.SOUTH, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.NORTH, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.NORTH, Direction.DOWN);
                } else {
                    this.color4Vertices(LightSampleDirection.EAST);
                }
            } else if(direction == Direction.SOUTH) {
                if (useSmooth) {
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.DOWN);
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.DOWN);
                } else {
                    this.color4Vertices(LightSampleDirection.SOUTH);
                }
            } else if (direction == Direction.WEST) {
                if (useSmooth) {
                    this.sampleFor1x1Plane(direction, Direction.NORTH, Direction.DOWN);
                    this.sampleFor1x1Plane(direction, Direction.NORTH, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.SOUTH, Direction.UP);
                    this.sampleFor1x1Plane(direction, Direction.SOUTH, Direction.DOWN);
                } else {
                    this.color4Vertices(LightSampleDirection.WEST);
                }
            } else if(direction == Direction.UP) {
                if (useSmooth) {
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.SOUTH);
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.NORTH);
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.NORTH);
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.SOUTH);
                } else {
                    this.color4Vertices(LightSampleDirection.UP);
                }
            } else if(direction == Direction.DOWN) {
                if (useSmooth) {
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.NORTH);
                    this.sampleFor1x1Plane(direction, Direction.WEST, Direction.SOUTH);
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.SOUTH);
                    this.sampleFor1x1Plane(direction, Direction.EAST, Direction.NORTH);
                } else {
                    this.color4Vertices(LightSampleDirection.DOWN);
                }
            }
        }

        /// <summary>
        /// Adds a rotated box of quads.  Warning, rotated boxes that extend to the edge of their voxel or past may have lighting errors!
        /// </summary>
        [Obsolete("Use MeshBuilder.addCube() instead")]
        public void addBox(Vector3 pos, Vector3 boxRadius, Quaternion rotation, Block block, int meta, int renderFace) {
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
            if ((renderFace & 1) == 1) {
                this.addQuad(pnp, ppp, npp, nnp,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.NORTH, meta)),
                        meta,
                        Direction.NORTH,
                        new Vector2(boxRadius.x, boxRadius.y),
                        new Vector2(boxOffset.x, boxOffset.y)),
                    boxRadius.z >= 0.5f ? LightSampleDirection.NORTH : LightSampleDirection.SELF);
            }
            // +X/East face.
            if (((renderFace >> 1) & 1) == 1) {
                this.addQuad(pnn, ppn, ppp, pnp,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.EAST, meta)),
                        meta,
                        Direction.EAST,
                        new Vector2(boxRadius.z, boxRadius.y),
                        new Vector2(boxOffset.z, boxOffset.y)),
                    boxRadius.x >= 0.5f ? LightSampleDirection.EAST : LightSampleDirection.SELF);
            }
            // -Z/South face.
            if (((renderFace >> 2) & 1) == 1) {
                this.addQuad(nnn, npn, ppn, pnn,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.SOUTH, meta)),
                        meta,
                        Direction.SOUTH,
                        new Vector2(boxRadius.x, boxRadius.y),
                        new Vector2(boxOffset.x, boxOffset.y)),
                    boxRadius.z <= -0.5f ? LightSampleDirection.SOUTH : LightSampleDirection.SELF);
            }
            // -X/West face.
            if (((renderFace >> 3) & 1) == 1) {
                this.addQuad(nnp, npp, npn, nnn,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.WEST, meta)),
                        meta,
                        Direction.WEST,
                        new Vector2(boxRadius.z, boxRadius.y),
                        new Vector2(boxOffset.z, boxOffset.y)),
                    boxRadius.x <= -0.5f ? LightSampleDirection.WEST : LightSampleDirection.SELF);
            }
            // +Y/Up face.
            if (((renderFace >> 4) & 1) == 1) {
                this.addQuad(npn, npp, ppp, ppn,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.UP, meta)),
                        meta,
                        Direction.UP,
                        new Vector2(boxRadius.z, boxRadius.x),
                        new Vector2(boxOffset.z, boxOffset.x)),
                    boxRadius.y >= 0.5f ? LightSampleDirection.UP : LightSampleDirection.SELF);
            }
            // -Y/Down face.
            if (((renderFace >> 5) & 1) == 1) {
                this.addQuad(nnn, pnn, pnp, nnp,
                    block.applyUvAlterations(
                        this.generateUVsFromTP(block.getTexturePos(Direction.DOWN, meta)),
                        meta,
                        Direction.DOWN,
                        new Vector2(boxRadius.x, boxRadius.z),
                        new Vector2(boxOffset.x, boxOffset.z)),
                    boxRadius.y <= -0.5f ? LightSampleDirection.DOWN : LightSampleDirection.SELF);
            }
        }

        /// <summary>
        /// Adds a collider geometry to a mesh based on a Bounds.  X, Y and Z are the block's orgin.
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
        /// Creates a geometry mesh for rendering.
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
        /// Creates a collider mesh for chunk collision.
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
        public void prepareForReuse() {
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
        /// Fills the light lookup table with max light of 15.  Used by blocks and items in the hud when prerendering.
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
            // Define the points.
            Vector3 ppp = cube.from.toVector();
            Vector3 ppn = new Vector3(cube.from.x, cube.from.y, cube.to.z);
            Vector3 npn = new Vector3(cube.to.x, cube.from.y, cube.to.z);
            Vector3 npp = new Vector3(cube.to.x, cube.from.y, cube.from.z);
            Vector3 pnp = new Vector3(cube.from.x, cube.to.y, cube.from.z);
            Vector3 pnn = new Vector3(cube.from.x, cube.to.y, cube.to.z);
            Vector3 nnn = cube.to.toVector();
            Vector3 nnp = new Vector3(cube.to.x, cube.to.y, cube.from.z);

            // Rotate the cube if needed.
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

            // Offset the cube if needed.
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

            // Convert the pixel units to world units before rendering.
            ppp = this.pixelToWorld(ppp);
            ppn = this.pixelToWorld(ppn);
            npn = this.pixelToWorld(npn);
            npp = this.pixelToWorld(npp);
            pnp = this.pixelToWorld(pnp);
            pnn = this.pixelToWorld(pnn);
            nnn = this.pixelToWorld(nnn);
            nnp = this.pixelToWorld(nnp);

            Vector3 worldPos = new Vector3(worldX, worldY, worldZ);

            // Note: The vertices that check if the face is out of the cell, meaning it need adjacent light, is arbitrary,
            // the same effect should be given requardless of the vertex.

            // North +Z
            if ((renderFace & 1) == 1) {
                this.func01(
                    renderer, worldPos,
                    pnp,
                    ppp,
                    npp,
                    nnp,
                    renderer.getUvPlane(block, meta, Direction.NORTH, cube.index).getMeshUvs(this.allocatedUvArray),
                    pnp.z >= 0.5f ? BlockPos.north : BlockPos.zero);
            }
            // East +X
            if (((renderFace >> 1) & 1) == 1) {
                this.func01(
                    renderer, worldPos,
                    pnn,
                    ppn,
                    ppp,
                    pnp,
                    renderer.getUvPlane(block, meta, Direction.EAST, cube.index).getMeshUvs(this.allocatedUvArray),
                    pnn.x >= 0.5f ? BlockPos.east : BlockPos.zero);
            }
            // South -Z
            if (((renderFace >> 2) & 1) == 1) {
                this.func01(
                    renderer, worldPos,
                    nnn,
                    npn,
                    ppn,
                    pnn,
                    renderer.getUvPlane(block, meta, Direction.SOUTH, cube.index).getMeshUvs(this.allocatedUvArray),
                    nnn.z <= -0.5f ? BlockPos.south : BlockPos.zero);
            }
            // West -X
            if (((renderFace >> 3) & 1) == 1) {
                this.func01(
                    renderer, worldPos,
                    nnp,
                    npp,
                    npn,
                    nnn,
                    renderer.getUvPlane(block, meta, Direction.WEST, cube.index).getMeshUvs(this.allocatedUvArray),
                    nnp.x <= -0.5f ? BlockPos.west : BlockPos.zero);
            }
            // Up +Y
            if (((renderFace >> 4) & 1) == 1) {
                this.func01(
                    renderer, worldPos,
                    npn,
                    npp,
                    ppp,
                    ppn,
                    renderer.getUvPlane(block, meta, Direction.UP, cube.index).getMeshUvs(this.allocatedUvArray),
                    npn.y >= 0.5f ? BlockPos.west : BlockPos.zero);
            }
            // Down -Y
            if (((renderFace >> 5) & 1) == 1) {
                this.func01(
                    renderer, worldPos,
                    nnn,
                    pnn,
                    pnp,
                    nnp,
                    renderer.getUvPlane(block, meta, Direction.DOWN, cube.index).getMeshUvs(this.allocatedUvArray),
                    nnn.y <= -0.5f ? BlockPos.down : BlockPos.zero);
            }
        }

        /// <summary>
        /// Calculates the light color for a single vertex.  Vertex should be a Vector with its offset from
        /// it's cell orgin.  Normal can point away from the cell, if the plane borders the edge of the cell,
        /// or be (0, 0, 0) if the face is inside of the cell.
        /// </summary>
        private Color calculateLightForVertex(Vector3 vertex, BlockPos normal) {
            int level;
            if (RenderManager.instance.useSmoothLighting) {
                List<int> sampledLevels = new List<int>();

                int x = normal.x;
                int y = normal.y;
                int z = normal.z;

                // Sample light on X axis.
                if (normal.x == 0) {
                    if (vertex.x >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x + 1, y, z));
                    }
                    else if (vertex.x <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x - 1, y, z));
                    }
                }

                // Sample light in front/in of cell.
                sampledLevels.Add(this.getLightLevel(x, y, z));

                // Sample light on Y axis.
                if (normal.y == 0) {
                    if (vertex.y >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y + 1, z));
                    }
                    else if (vertex.y <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y - 1, z));
                    }
                }

                // Sample light on Z axis.
                if (normal.z == 0) {
                    if (vertex.z >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y, z + 1));
                    }
                    else if (vertex.z <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y, z - 1));
                    }
                }

                // Sample light diagonal of cell.
                if (normal.x != 0) {
                    if (vertex.z >= 0.5f && vertex.y >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y + 1, z + 1));
                    }
                    else if (vertex.z >= 0.5f && vertex.y <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y - 1, z + 1));
                    }
                    else if (vertex.z <= -0.5f && vertex.y >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y + 1, z - 1));
                    }
                    else if (vertex.z <= -0.5f && vertex.y <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x, y - 1, z - 1));
                    }
                }
                else if (normal.y != 0) {
                    if (vertex.x >= 0.5f && vertex.z >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x + 1, y, z + 1));
                    }
                    else if (vertex.x >= 0.5f && vertex.z <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x + 1, y, z - 1));
                    }
                    else if (vertex.x <= -0.5f && vertex.z >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x - 1, y, z + 1));
                    }
                    else if (vertex.x <= -0.5f && vertex.z <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x - 1, y, z - 1));
                    }
                }
                else if (normal.z != 0) {
                    if (vertex.x >= 0.5f && vertex.y >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x + 1, y + 1, z));
                    }
                    else if (vertex.x >= 0.5f && vertex.y <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x + 1, y - 1, z));
                    }
                    else if (vertex.x <= -0.5f && vertex.y >= 0.5f) {
                        sampledLevels.Add(this.getLightLevel(x - 1, y + 1, z));
                    }
                    else if (vertex.x <= -0.5f && vertex.y <= -0.5f) {
                        sampledLevels.Add(this.getLightLevel(x - 1, y - 1, z));
                    }
                }

                // Average levels.
                if (sampledLevels.Count == 0) {
                    level = this.getLightLevel(0, 0, 0);
                } else {
                    float lightTotal = 0;
                    for (int i = 0; i < sampledLevels.Count; i++) {
                        lightTotal += sampledLevels[i];
                    }
                    level = (int)(lightTotal / sampledLevels.Count);
                }
            } else {
                level = this.getLightLevel(0, 0, 0);
            }

            // Return color.
            return RenderManager.instance.lightColors.getColorFromBrightness(level);
        }

        /// <summary>
        /// Adds and computes the lighting for a quad from a ComponentCube.
        /// </summary>
        private void func01(BlockRendererPrimitive renderer, Vector3 worldPos, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector2[] uvs, BlockPos shift) {
            // Arg shift is sort of like a normal, but can be (0, 0, 0).

            if (renderer.demandLocalLight) {
                // No need to lookup fancy lighting, render a quad.
                this.addQuad(
                    v0 + worldPos,
                    v1 + worldPos,
                    v2 + worldPos,
                    v3 + worldPos,
                    uvs,
                    LightSampleDirection.SELF);
            } else {
                // Add vertex colors for lighting.
                this.vertexColors.Add(this.calculateLightForVertex(v0, shift));
                this.vertexColors.Add(this.calculateLightForVertex(v1, shift));
                this.vertexColors.Add(this.calculateLightForVertex(v2, shift));
                this.vertexColors.Add(this.calculateLightForVertex(v3, shift));

                this.addQuad(
                    v0 + worldPos,
                    v1 + worldPos,
                    v2 + worldPos,
                    v3 + worldPos,
                    uvs);
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

        /// <summary>
        /// Adds lighting color to the 4 most recent vertices.  Color is looked up from the legacy light sample constants.
        /// </summary>
        private void color4Vertices(int lightSampleDir) {
            Color c = RenderManager.instance.lightColors.getColorFromBrightness(this.getLightFromLegacySampleDir(lightSampleDir));
            this.vertexColors.Add(c);
            this.vertexColors.Add(c);
            this.vertexColors.Add(c);
            this.vertexColors.Add(c);
        }

        /// <summary>
        /// Make sure to call in the standard clockwise order!
        /// </summary>
        private void sampleFor1x1Plane(Direction facing, Direction d1, Direction d2) {
            BlockPos orgin = facing.blockPos;
            float totaledLightLevel = (
                this.getLightLevel(orgin) +
                this.getLightLevel(orgin + d1.blockPos) +
                this.getLightLevel(orgin + d1.blockPos + d2.blockPos) +
                this.getLightLevel(orgin + d2.blockPos)) / 4;
            this.vertexColors.Add(RenderManager.instance.lightColors.getColorFromBrightness((int)totaledLightLevel));
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
 