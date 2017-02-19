using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveRail : BlockRendererPrimitive {

        public override MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.45f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.45f, z + 0.5f));
            meshData.addVertex(new Vector3(x + 0.5f, y - 0.45f, z - 0.5f));
            meshData.addVertex(new Vector3(x - 0.5f, y - 0.45f, z - 0.5f));
            meshData.generateQuad(b.getUVs(meta, Direction.UP, this.uvArray));

            return meshData;
        }
    }
}
