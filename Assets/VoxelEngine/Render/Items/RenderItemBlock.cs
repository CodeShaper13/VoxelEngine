using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Items;

namespace VoxelEngine.Render.Items {

    public class RenderItemBlock : IRenderItem {

        public Mesh renderItemFlat(Item item, int meta) {
            return this.renderBlock(item, meta, RenderFace.N | RenderFace.E | RenderFace.S | RenderFace.U);
        }

        public Mesh renderItem3d(Item item, int meta) {
            return this.renderBlock(item, meta, RenderFace.ALL);
        }

        public Mesh renderBlock(Item item, int meta, int renderFace) {
            Block block = Block.BLOCK_LIST[item.id];
            MeshBuilder meshBuilder = RenderManager.getMeshBuilder();
            meshBuilder.setMaxLight();
            block.renderer.renderBlock(block, meta, meshBuilder, 0, 0, 0, renderFace, BlockAir.AIR_ARRAY);
            return meshBuilder.getGraphicMesh();
        }
    }
}
