using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;

namespace VoxelEngine.Render.Items {

    public class RenderItemBlock : IRenderItem {

        public Vector3 scale = new Vector3(0.125f, 0.125f, 0.125f);

        public Mesh renderItem(ItemStack stack) {
            Block b = Block.BLOCK_LIST[stack.item.id];
            return b.renderer.renderBlock(b, stack.meta, new MeshData(), 0, 0, 0, new bool[6] { true, true, true, true, true, true }).toMesh();
        }

        public Matrix4x4 getMatrix(Vector3 pos) {
            return Matrix4x4.TRS(pos, Quaternion.Euler(-20, 48, -20), this.scale);
        }
    }
}
