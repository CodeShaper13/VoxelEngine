using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Render;

namespace VoxelEngine.Util {

    public static class RenderHelper {

        /// <summary>
        /// Renders an item stack on the screen
        /// </summary>
        public static void renderStack(ItemStack stack, Vector3 pos) {
            Item item = stack.item;
            Graphics.DrawMesh(item.getPreRenderedMesh(stack.meta), item.itemRenderer.getMatrix(pos), RenderManager.getMaterial(item.id), 8, null, 0, null, false, false);
        }
    }
}
