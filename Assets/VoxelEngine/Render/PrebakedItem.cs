using UnityEngine;
using VoxelEngine.Items;

namespace VoxelEngine.Render {

    /// <summary>
    /// Stores data about the pre-baked item and item block meshes.
    /// </summary>
    public class PreBakedItem {

        private Mesh[] meshFlat;
        private Mesh[] mesh3d;

        public PreBakedItem(Item item) {
            int i = item.getStatesUsed();
            this.meshFlat = new Mesh[i];
            this.mesh3d = new Mesh[i];
            for (int j = 0; j < i; j++) {
                this.meshFlat[j] = item.itemRenderer.renderItemFlat(item, j);
                this.mesh3d[j] = item.itemRenderer.renderItem3d(item, j);
            }
        }

        public Mesh getMesh(int meta, bool is3d) {
            return is3d ? this.mesh3d[meta] : this.meshFlat[meta];
        }
    }
}
