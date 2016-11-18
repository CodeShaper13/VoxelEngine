using UnityEngine;

public class RenderDataBillboard : RenderData {

    public override MeshData renderItem(ItemStack item) {
        MeshData meshData = new MeshData();

        // Faces front, +z
        meshData.AddVertex(new Vector3(0.5f, -0.5f, 0));  //bottom right
        meshData.AddVertex(new Vector3(0.5f, 0.5f, 0));   //top right
        meshData.AddVertex(new Vector3(-0.5f, 0.5f, 0));  //top left
        meshData.AddVertex(new Vector3(-0.5f, -0.5f, 0)); //bottom left
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(this.setUVs(item.item.texturePos));

        // Faces back, -z
        meshData.AddVertex(new Vector3(-0.5f, -0.5f, 0));
        meshData.AddVertex(new Vector3(-0.5f, 0.5f, 0));
        meshData.AddVertex(new Vector3(0.5f, 0.5f, 0));
        meshData.AddVertex(new Vector3(0.5f, -0.5f, 0));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(this.setUVs(item.item.texturePos));

        return meshData;
    }

    public virtual Vector2[] setUVs(TexturePos pos) {
        float x = TexturePos.ITEM_SIZE * pos.x;
        float y = TexturePos.ITEM_SIZE * pos.y;
        Vector2[] UVs = new Vector2[4] {
            new Vector2(x, y),
            new Vector2(x, y + TexturePos.ITEM_SIZE),
            new Vector2(x + TexturePos.ITEM_SIZE, y + TexturePos.ITEM_SIZE),
            new Vector2(x + TexturePos.ITEM_SIZE, y) };
        return UVs;
    }
}
