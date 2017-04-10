using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Items;

namespace VoxelEngine.Render.Items {

    public class RenderItemBlock : IRenderItem {

        private static bool[] TRUE_ARRAY = new bool[6] {true, true, true, true, true, true};
        private static Block[] AIR_ARRAY = new Block[6] {Block.air, Block.air, Block.air, Block.air, Block.air, Block.air};
        private static int[] MAX_LIGHT_LEVELS = new int[7] {15, 15, 15, 15, 15, 15, 15};

        public Mesh renderItem(Item item, byte meta) {
            Block block = Block.BLOCK_LIST[item.id];
            MeshBuilder meshBuilder = RenderManager.instance.getMeshBuilder();
            meshBuilder.lightLevels = RenderItemBlock.MAX_LIGHT_LEVELS;
            return block.renderer.renderBlock(block, meta, meshBuilder, 0, 0, 0, RenderItemBlock.TRUE_ARRAY, RenderItemBlock.AIR_ARRAY).toMesh();
        }

        public Matrix4x4 getMatrix(Vector3 pos) {
            return Matrix4x4.TRS(pos, Quaternion.Euler(-9.2246f, 45.7556f, -9.346399f), new Vector3(0.125f, 0.125f, 0.125f));
        }
    }
}
