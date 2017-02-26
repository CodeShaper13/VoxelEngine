using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;

namespace VoxelEngine.Util {

    public static class RenderHelper {

        public static void renderStack(ItemStack stack, Vector3 pos) {
            Item item = stack.item;
            Graphics.DrawMesh(item.getPreRenderedMesh(stack.meta), item.itemRenderer.getMatrix(pos), References.getUnlitMaterial(item.id), 8, null, 0, null, false, false);
        }
    }
}
