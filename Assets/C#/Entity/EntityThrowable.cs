using UnityEngine;

public class EntityThrowable : Entity {

    public new void Awake() {
        base.Awake();

        IRenderItem r = Item.pebble.itemRenderer;
        MeshData meshData = r.renderItem(new ItemStack(Item.pebble));
        MeshFilter filter = this.GetComponent<MeshFilter>();
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();
        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();
        this.GetComponent<MeshRenderer>().material = VoxelEngine.singleton.itemMaterial;
    }

    public override void onEntityCollision(Entity otherEntity) {
        base.onEntityCollision(otherEntity);
        if(otherEntity != null) {
            otherEntity.damage(1);
        }
        this.world.killEntity(this);
    }
}
