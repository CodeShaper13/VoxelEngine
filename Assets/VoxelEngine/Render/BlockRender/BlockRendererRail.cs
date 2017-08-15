using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererRail : BlockRendererPrimitive {

        public override void renderBlock(Block block, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            float f = 0.425f;
            meshData.addPlane(
                block, meta,
                new Vector3(x - 0.5f, y - f, z + 0.5f),
                new Vector3(x + 0.5f, y - f, z + 0.5f),
                new Vector3(x + 0.5f, y - f, z - 0.5f),
                new Vector3(x - 0.5f, y - f, z - 0.5f),
                Direction.NONE);
        }
    }
}
