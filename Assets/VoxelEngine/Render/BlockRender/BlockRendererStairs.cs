using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererStairs : BlockRendererPrimitive {

        public BlockRendererStairs() {
            this.lookupAdjacentLight = true;
        }

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, int renderFace, Block[] surroundingBlocks) {
            // Bottom.
            meshBuilder.addBox(new Vector3(x, y - 0.25f, z), new Vector3(0.5f, 0.25f, 0.5f), block, meta, RenderManager.TRUE_ARRAY);

            // Top.
            Direction dir = BlockStairs.getDirectionFromMeta(meta);

            meshBuilder.addBox(
                new Vector3(x, y + 0.25f, z) + dir.vector * 0.25f,
                new Vector3(dir.axis == EnumAxis.X ? 0.25f : 0.5f, 0.25f, dir.axis == EnumAxis.Z ? 0.25f : 0.5f),
                block, meta, RenderManager.TRUE_ARRAY);
        }
    }
}
