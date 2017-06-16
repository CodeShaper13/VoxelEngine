using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererRail : BlockRendererPrimitive {

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            float f = 0.425f;
            meshData.addQuad(
                new Vector3(x - 0.5f, y - f, z + 0.5f),
                new Vector3(x + 0.5f, y - f, z + 0.5f),
                new Vector3(x + 0.5f, y - f, z - 0.5f),
                new Vector3(x - 0.5f, y - f, z - 0.5f),
                b.getUVs(meta, Direction.UP, this.preAllocatedUvArray),
                LightHelper.SELF);
        }
    }
}
