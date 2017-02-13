using UnityEngine;
using VoxelEngine.Items;

namespace VoxelEngine.Render.Items {

    public interface IRenderItem {

        Mesh renderItem(Item item, byte meta);

        Matrix4x4 getMatrix(Vector3 pos);
    }
}
