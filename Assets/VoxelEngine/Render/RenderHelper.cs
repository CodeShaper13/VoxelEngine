using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Render;

namespace VoxelEngine.Util {

    public static class RenderHelper {

        /// <summary>
        /// Renders an item stack on the screen
        /// </summary>
        public static void renderStack(ItemStack stack, Vector3 pos, int layer = 8) {
            Item item = stack.item;
            MutableTransform mutableTransform = item.getContainerTransfrom();
            mutableTransform.position += pos;
            Graphics.DrawMesh(item.getPreRenderedMesh(stack.meta), mutableTransform.toMatrix4x4(), RenderManager.getMaterial(item.id), layer, null, 0, null, false, false);
        }
    }
}
