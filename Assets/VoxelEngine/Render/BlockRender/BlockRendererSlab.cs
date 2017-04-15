using UnityEngine;
using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererSlab : BlockRendererPrimitive {

        public override MeshBuilder renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            if(BlockSlab.isFull(meta)) {
                RenderManager.CUBE.renderBlock(b, meta, meshData, x, y, z, renderFace, surroundingBlocks);
            } else {
                Vector3 v = BlockSlab.getDirectionFromMeta(meta).direction.toVector();
                Vector3 size = new Vector3(
                    v.x == 0 ? 0.5f : 0.25f,
                    v.y == 0 ? 0.5f : 0.25f,
                    v.z == 0 ? 0.5f : 0.25f);
                meshData.addBox(new Vector3(x + (v.x / 4), y + (v.y / 4), z + (v.z / 4)), size, b, meta, this.uvArray);
            }

            return meshData;
        }
    }
}
