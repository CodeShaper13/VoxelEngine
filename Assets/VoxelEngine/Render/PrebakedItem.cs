using UnityEngine;
using VoxelEngine.Items;

namespace VoxelEngine.Render {

    /// <summary>
    /// Stores data about meshes for the pre-baked item and item blocks in the hud.
    /// </summary>
    public class PreBakedItem {

        /// <summary> Flat version doesn't have 3d edge effect, thus is optimized for container rendering. </summary>
        private Mesh[] meshFlat;
        private Mesh[] mesh3d;

        public PreBakedItem(Item item) {
            int statesUsed = item.getStatesUsed();
            this.meshFlat = new Mesh[statesUsed];
            this.mesh3d = new Mesh[statesUsed];
            for (int i = 0; i < statesUsed; i++) {
                this.meshFlat[i] = item.itemRenderer.renderItemFlat(item, i);
                this.mesh3d[i] = item.itemRenderer.renderItem3d(item, i);
            }
        }

        public Mesh getMesh(int meta, bool is3d) {
            return is3d ? this.mesh3d[meta] : this.meshFlat[meta];
        }
    }
}
