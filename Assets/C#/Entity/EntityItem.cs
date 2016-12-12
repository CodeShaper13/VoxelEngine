using UnityEngine;

public class EntityItem : Entity {
    public ItemStack stack;
    private MeshFilter filter;
    private MeshRenderer meshRenderer;

	void Awake () {
        this.filter = this.GetComponent<MeshFilter>();
        this.meshRenderer = this.GetComponent<MeshRenderer>();
	}
	
	void Update () {
        this.transform.Rotate(0, Time.deltaTime * 25, 0);
	}

    public override void onPlayerTouch(Player player) {
        ItemStack s = player.pInventory.addItemStack(this.stack);
        if (s == null) {
            GameObject.Destroy(this.gameObject);
        }
    }

    //Sets up the item rendering
    public void initRendering() {
        IRenderItem r = this.stack.item.itemRenderer;
        MeshData meshData = r.renderItem(this.stack);
        this.filter.mesh.Clear();
        this.filter.mesh.vertices = meshData.vertices.ToArray();
        this.filter.mesh.triangles = meshData.triangles.ToArray();
        this.filter.mesh.uv = meshData.uv.ToArray();
        this.filter.mesh.RecalculateNormals();
        this.meshRenderer.material = this.stack.item.id < 256 ? Constants.instance.blockMaterial : Constants.instance.itemMaterial;
    }
}
