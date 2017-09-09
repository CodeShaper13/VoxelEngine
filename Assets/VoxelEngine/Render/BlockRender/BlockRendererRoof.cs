using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererRoof : BlockRendererPrimitive {

        public BlockRendererRoof() {
            this.lookupAdjacentBlocks = true;
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            // Bottom.
            if(((renderFace >> 5) & 1) == 1) {
                meshBuilder.addOptimized1x1Plane(this, block, meta,
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    Direction.DOWN);
            }

            Direction facing = Direction.all[meta];
            Direction right = facing.getCounterClockwise();
            Direction left = facing.getClockwise();
            Vector3 pos = new Vector3(x, y, z);
            Quaternion angle = Quaternion.Euler(0, (facing.index - 1) * 90, 0);

            // Back.
            if (((renderFace >> (facing.index - 1)) & 1) == 1) {
                meshBuilder.addQuad(
                    pos + MathHelper.rotateVecAround(new Vector3(0.5f, -0.5f, 0.5f), Vector3.zero, angle),
                    pos + MathHelper.rotateVecAround(new Vector3(0.5f, 0.5f, 0.5f), Vector3.zero, angle),
                    pos + MathHelper.rotateVecAround(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.zero, angle),
                    pos + MathHelper.rotateVecAround(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.zero, angle),
                    meshBuilder.generateUVsFromTP(block.getTexturePos(facing, meta)),
                    facing.index);
            }

            // Sides.
            this.addSide(block, meta, pos, meshBuilder, right, true);
            this.addSide(block, meta, pos, meshBuilder, left, false);

            bool extendsRight = !(surroundingBlocks[right.index - 1] is BlockRoof);
            bool extendsLeft = !(surroundingBlocks[left.index - 1] is BlockRoof);
            float rightSize = 0.5f + (extendsRight ? MathHelper.pixelToWorld(3) : 0f);
            float leftSize = 0.5f + (extendsLeft ? MathHelper.pixelToWorld(3) : 0f);

            // Adjust UVs.
            Vector2[] uvs = meshBuilder.generateUVsFromTP(block.getTexturePos(Direction.UP, meta));
            if(extendsRight) {
                float f = TexturePos.PIXEL_SIZE * 3;
                uvs[0] += new Vector2(-f, 0);
                uvs[1] += new Vector2(-f, 0);
            }
            if (extendsLeft) {
                float f = TexturePos.PIXEL_SIZE * 3;
                uvs[2] += new Vector2(f, 0);
                uvs[3] += new Vector2(f, 0);
            }

            // Slope.
            meshBuilder.addQuad(
                pos + MathHelper.rotateVecAround(new Vector3(-rightSize, -0.5f, -0.5f), Vector3.zero, angle),
                pos + MathHelper.rotateVecAround(new Vector3(-rightSize, 0.5f, 0.5f), Vector3.zero, angle),
                pos + MathHelper.rotateVecAround(new Vector3(leftSize,  0.5f, 0.5f), Vector3.zero, angle),
                pos + MathHelper.rotateVecAround(new Vector3(leftSize, -0.5f, -0.5f), Vector3.zero, angle),
                uvs, LightSampleDirection.SELF);
        }

        private void addSide(Block block, int meta, Vector3 pos, MeshBuilder meshBuilder, Direction direction, bool isRight) {
            float x = (direction.vector / 2).x;
            Vector3 lowVert = pos + new Vector3(x, -0.5f, -0.5f);
            Vector3 middleVert = pos + new Vector3(x, -0.5f, 0.5f);
            Vector3 highVert = pos + new Vector3(x, 0.5f, 0.5f);

            // Verts
            meshBuilder.addRawVertex(lowVert);
            meshBuilder.addRawVertex(middleVert);
            meshBuilder.addRawVertex(highVert);

            meshBuilder.addRawVertexColor(Color.black);
            meshBuilder.addRawVertexColor(Color.black);
            meshBuilder.addRawVertexColor(Color.black);

            // Tris
            int i = meshBuilder.getVerticeCount();
            if (isRight) {
                meshBuilder.addRawTriangle(i - 3);
                meshBuilder.addRawTriangle(i - 2);
                meshBuilder.addRawTriangle(i - 1);
            } else {
                meshBuilder.addRawTriangle(i - 1);
                meshBuilder.addRawTriangle(i - 2);
                meshBuilder.addRawTriangle(i - 3);
            }

            // UVs
            TexturePos tilePos = block.getTexturePos(direction, meta);
            float x1 = TexturePos.ATLAS_TILE_SIZE * tilePos.x;
            float y1 = TexturePos.ATLAS_TILE_SIZE * tilePos.y;

            int sampleDir = direction.index;
            meshBuilder.addRawUv(new Vector2(x1 + TexturePos.ATLAS_TILE_SIZE, y1));
            meshBuilder.addRawUv(new Vector2(x1, y1));
            meshBuilder.addRawUv(new Vector2(x1, y1 + TexturePos.ATLAS_TILE_SIZE));
        }
    }
}
