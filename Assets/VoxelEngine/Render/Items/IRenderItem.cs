using UnityEngine;
using VoxelEngine.Containers;

namespace VoxelEngine.Render.Items {

    public interface IRenderItem {

        Mesh renderItem(ItemStack stack);

        Matrix4x4 getMatrix(Vector3 pos);
    }
}
