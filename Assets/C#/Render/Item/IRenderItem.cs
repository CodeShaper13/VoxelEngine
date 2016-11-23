using UnityEngine;

public interface IRenderItem {

    MeshData renderItem(ItemStack stack);

    Matrix4x4 getMatrix(Transform t);
}
