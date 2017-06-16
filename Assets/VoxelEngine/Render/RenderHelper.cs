using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Render {

    public static class RenderHelper {

        /// <summary>
        /// Renders an ItemStack on the screen
        /// </summary>
        public static void renderStack(ItemStack stack, Vector3 position, Quaternion rotation) {
            Item item = stack.item;
            MutableTransform mutableTransform = item.getContainerTransfrom();
            mutableTransform.position += position;
            mutableTransform.rotation *= rotation;
            Graphics.DrawMesh(item.getPreRenderedMesh(stack.meta), mutableTransform.toMatrix4x4(), RenderManager.getMaterial(item.id), 8, null, 0, null, false, false);
        }
    }
}
