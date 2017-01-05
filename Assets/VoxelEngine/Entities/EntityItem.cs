using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Render;
using VoxelEngine.Render.Items;

namespace VoxelEngine.Entities {

    public class EntityItem : Entity {
        public ItemStack stack;
        private MeshFilter filter;

        public new void Awake() {
            base.Awake();
            this.filter = this.GetComponent<MeshFilter>();
        }

        public void initRendering() {
            IRenderItem r = this.stack.item.itemRenderer;
            MeshData meshData = r.renderItem(this.stack);
            this.filter.mesh.Clear();
            this.filter.mesh.vertices = meshData.vertices.ToArray();
            this.filter.mesh.triangles = meshData.triangles.ToArray();
            this.filter.mesh.uv = meshData.uv.ToArray();
            this.filter.mesh.RecalculateNormals();
            this.GetComponent<MeshRenderer>().material = Main.getMaterial(this.stack.item.id);
        }

        public override void onEntityUpdate() {
            base.onEntityUpdate();
            this.transform.Rotate(0, Time.deltaTime * 25, 0);
        }

        public override string getMagnifyingText() {
            return "An item, bumping into it will let you pick it up.";
        }
    }
}
