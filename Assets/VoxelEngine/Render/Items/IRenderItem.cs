using UnityEngine;
using VoxelEngine.Items;

namespace VoxelEngine.Render.Items {

    public interface IRenderItem {

        /// <summary>
        /// Returns a rendered item in the form of a mesh.
        /// </summary>
        Mesh renderItem(RenderManager rm, Item item, int meta);
    }
}
