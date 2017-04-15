using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveCube : BlockRendererPrimitive {

        public BlockRendererPrimitiveCube() {
            this.lookupAdjacentLight = EnumLightLookup.ADJACENT;
        }

        public override MeshBuilder renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            // North
            if (renderFace[0]) {
                meshData.addQuad(
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.NORTH, this.uvArray),
                    Direction.NORTH_ID);
            }
            // East
            if (renderFace[1]) {
                meshData.addQuad(
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.EAST, this.uvArray),
                    Direction.EAST_ID);
            }
            // South
            if (renderFace[2]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    b.getUVs(meta, Direction.SOUTH, this.uvArray),
                    Direction.SOUTH_ID);
            }
            // West
            if (renderFace[3]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    b.getUVs(meta, Direction.WEST, this.uvArray),
                    Direction.WEST_ID);
            }
            // Up
            if (renderFace[4]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),
                    new Vector3(x + 0.5f, y + 0.5f, z - 0.5f),
                    new Vector3(x - 0.5f, y + 0.5f, z - 0.5f),
                    b.getUVs(meta, Direction.UP, this.uvArray),
                    Direction.UP_ID);
            }
            // Down
            if (renderFace[5]) {
                meshData.addQuad(
                    new Vector3(x - 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z - 0.5f),
                    new Vector3(x + 0.5f, y - 0.5f, z + 0.5f),
                    new Vector3(x - 0.5f, y - 0.5f, z + 0.5f),
                    b.getUVs(meta, Direction.DOWN, this.uvArray),
                    Direction.DOWN_ID);
            }

            return meshData;
        }
    }
}
