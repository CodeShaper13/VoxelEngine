using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveFence : BlockRendererPrimitive {

        private Vector2[] uvArray;

        public BlockRendererPrimitiveFence() {
            this.uvArray = new Vector2[4];
        }

        public override MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            float postRadius = 0.2f;           

            //North
                meshData.addVertex(new Vector3(x + postRadius, y - 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y + 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y + 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y - 0.5f, z + postRadius));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.NORTH, this.uvArray));
            // East
                meshData.addVertex(new Vector3(x + postRadius, y - 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y + 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y + 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y - 0.5f, z + postRadius));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.EAST, this.uvArray));
            // South
                meshData.addVertex(new Vector3(x - postRadius, y - 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y + 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y + 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y - 0.5f, z - postRadius));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.SOUTH, this.uvArray));
            // West
                meshData.addVertex(new Vector3(x - postRadius, y - 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y + 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y + 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y - 0.5f, z - postRadius));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.WEST, this.uvArray));
            if(renderFace[4]) { // Up
                meshData.addVertex(new Vector3(x - postRadius, y + 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y + 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y + 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y + 0.5f, z - postRadius));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.UP, this.uvArray));
            }
            if(renderFace[5]) { // Down
                meshData.addVertex(new Vector3(x - postRadius, y - 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y - 0.5f, z - postRadius));
                meshData.addVertex(new Vector3(x + postRadius, y - 0.5f, z + postRadius));
                meshData.addVertex(new Vector3(x - postRadius, y - 0.5f, z + postRadius));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.DOWN, this.uvArray));
            }

            // Plank pointing north
            if (surroundingBlocks[0].isSolid || surroundingBlocks[0] == Block.fence) {
                meshData.addVertex(new Vector3(x, y + 0.45f, z));
                meshData.addVertex(new Vector3(x, y + 0.45f, z + 1));
                meshData.addVertex(new Vector3(x, y + 0.1f, z + 1));
                meshData.addVertex(new Vector3(x, y + 0.1f,  z));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.NONE, this.uvArray));
                meshData.addVertex(new Vector3(x, y + 0.1f, z));
                meshData.addVertex(new Vector3(x, y + 0.1f, z + 1));
                meshData.addVertex(new Vector3(x, y + 0.45f, z + 1));
                meshData.addVertex(new Vector3(x, y + 0.45f, z));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.NONE, this.uvArray));
            }

            // Plank pointing east
            if (surroundingBlocks[1].isSolid || surroundingBlocks[1] == Block.fence) {
                meshData.addVertex(new Vector3(x,     y + 0.45f, z));
                meshData.addVertex(new Vector3(x + 1, y + 0.45f, z));
                meshData.addVertex(new Vector3(x + 1, y + 0.1f, z));
                meshData.addVertex(new Vector3(x,     y + 0.1f, z));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.NONE, this.uvArray));
                meshData.addVertex(new Vector3(x,     y + 0.1f, z));
                meshData.addVertex(new Vector3(x + 1, y + 0.1f, z));
                meshData.addVertex(new Vector3(x + 1, y + 0.45f, z));
                meshData.addVertex(new Vector3(x,     y + 0.45f, z));
                meshData.addQuadWithUVs(b.getUVs(meta, Direction.NONE, this.uvArray));
            }

            return meshData;
        }
    }
}
