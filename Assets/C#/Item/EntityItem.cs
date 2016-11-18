using UnityEngine;

public class EntityItem : MonoBehaviour {

    public ItemStack stack;
    private MeshFilter filter;

	void Awake () {
        this.filter = this.GetComponent<MeshFilter>();
	}
	
	void Update () {
        this.transform.Rotate(0, Time.deltaTime * 25, 0);
	}

    //Sets up the item rendering
    public void initRendering() {
        MeshData meshData = this.stack.item.renderData.renderItem(this.stack);
        this.filter.mesh.Clear();
        this.filter.mesh.vertices = meshData.vertices.ToArray();
        this.filter.mesh.triangles = meshData.triangles.ToArray();
        this.filter.mesh.uv = meshData.uv.ToArray();
        this.filter.mesh.RecalculateNormals();
    }
}
