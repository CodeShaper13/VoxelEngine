using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Render.Blocks;

namespace VoxelEngine.Render.Items {

    public class RenderItemBlock : IRenderItem {

        public Vector3 scale = new Vector3(0.125f, 0.125f, 0.125f);

        public MeshData renderItem(ItemStack stack) {
            MeshData meshData = new MeshData();

            Block b = Block.BLOCK_LIST[stack.item.id];
            BlockModel model = b.getModel(stack.meta);
            model.preRender(b, stack.meta, meshData);
            model.renderBlock(0, 0, 0, new bool[] { true, true, true, true, true, true });
            return model.meshData;
        }

        public Matrix4x4 getMatrix(Vector3 pos) {
            return Matrix4x4.TRS(pos, Quaternion.Euler(-20, 48, -20), this.scale);
            //return Matrix4x4.TRS(t.position + -t.forward, Quaternion.Euler(-20 + t.eulerAngles.x, 48 + t.eulerAngles.y, -20 + t.eulerAngles.z), this.scale);
        }
    }
}
