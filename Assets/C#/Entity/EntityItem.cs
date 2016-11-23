using UnityEngine;

public class EntityItem : MonoBehaviour {
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
