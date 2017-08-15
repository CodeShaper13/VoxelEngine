using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Items;

namespace VoxelEngine.Render.Items {

    // TODO cull faces on flat block
    public class RenderItemBlock : IRenderItem {

        private bool[] cullFlatArray;
        private Block[] airArray;
        private int[] maxLightLevels;

        public RenderItemBlock() {
            this.cullFlatArray = new bool[6] { true, true, true, false, true, false };
            this.airArray = new Block[6] { Block.air, Block.air, Block.air, Block.air, Block.air, Block.air };
        }

        public Mesh renderItemFlat(Item item, int meta) {
            return this.renderBlock(item, meta, this.cullFlatArray);
        }

        public Mesh renderItem3d(Item item, int meta) {
            return this.renderBlock(item, meta, RenderManager.TRUE_ARRAY);
        }

        public Mesh renderBlock(Item item, int meta, bool[] cullArray) {
            Block block = Block.BLOCK_LIST[item.id];
            MeshBuilder meshBuilder = RenderManager.getMeshBuilder();
            meshBuilder.setMaxLight();
            block.renderer.renderBlock(block, meta, meshBuilder, 0, 0, 0, cullArray, this.airArray);
            return meshBuilder.getGraphicMesh();
        }
    }
}
