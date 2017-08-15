using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererChest : BlockRendererPrimitive {

        public BlockRendererChest() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            float f = MathHelper.pixelToWorld(14);
            meshData.addBox(
                new Vector3(x, y - MathHelper.pixelToWorld(2), z),
                new Vector3(f, f, f),
                Block.chest,
                meta,
                RenderManager.TRUE_ARRAY);
        }
    }
}
