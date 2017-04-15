using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveRail : BlockRendererPrimitive {

        public override MeshBuilder renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            meshData.addQuad(
                new Vector3(x - 0.5f, y - 0.45f, z + 0.5f),
                new Vector3(x + 0.5f, y - 0.45f, z + 0.5f),
                new Vector3(x + 0.5f, y - 0.45f, z - 0.5f),
                new Vector3(x - 0.5f, y - 0.45f, z - 0.5f),
                b.getUVs(meta, Direction.UP, this.uvArray),
                0);
            return meshData;
        }
    }
}
