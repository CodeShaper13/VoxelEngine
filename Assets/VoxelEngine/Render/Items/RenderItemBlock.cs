using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Items;

namespace VoxelEngine.Render.Items {

    public class RenderItemBlock : IRenderItem {

        public Vector3 scale = new Vector3(0.125f, 0.125f, 0.125f);
        public bool[] trueArray = new bool[6] { true, true, true, true, true, true };
        public Block[] airArray = new Block[6] {Block.air, Block.air, Block.air, Block.air, Block.air, Block.air };

        public Mesh renderItem(Item item, byte meta) {
            Block b = Block.BLOCK_LIST[item.id];
            return b.renderer.renderBlock(b, meta, new MeshData(), 0, 0, 0, this.trueArray, this.airArray).toMesh();
        }

        public Matrix4x4 getMatrix(Vector3 pos) {
            return Matrix4x4.TRS(pos, Quaternion.Euler(-9.2246f, 45.7556f, -9.346399f), this.scale);
        }
    }
}
