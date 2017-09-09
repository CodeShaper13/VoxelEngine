using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Render {

    public static class RenderHelper {

        /// <summary>
        /// Renders an ItemStack on the screen.  Used by containers.
        /// </summary>
        public static void renderStack(ItemStack stack, Vector3 position, Quaternion rotation) {
            Item item = stack.item;
            MutableTransform mutableTransform = item.getContainerTransfrom();
            mutableTransform.position += position;
            mutableTransform.rotation *= rotation;
            Graphics.DrawMesh(RenderManager.getItemMesh(stack.item, stack.meta, false), mutableTransform.toMatrix4x4(), References.list.blockMaterial, 8, null, 0, null, false, false);
        }
    }
}
