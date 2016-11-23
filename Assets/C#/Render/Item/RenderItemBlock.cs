using System;
using UnityEngine;

public class RenderItemBlock : IRenderItem {

    public MeshData renderItem(ItemStack stack) {
        MeshData meshData = new MeshData();

        Block b = Block.BLOCK_LIST[stack.item.id];
        BlockModel model = b.getModel(stack.meta);
        model.preRender(b, stack.meta, meshData);
        model.renderBlock(0, 0, 0, new bool[] {true, true, true, true, true, true });
        return model.meshData;
    }

    public Matrix4x4 getMatrix(Transform t) {
        return Matrix4x4.TRS(t.position + -t.forward, Quaternion.Euler(-45, 30, 0), new Vector3(0.1f, 0.1f, 0.1f));
    }
}
