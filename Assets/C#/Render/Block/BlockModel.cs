using System;
using UnityEngine;

public abstract class BlockModel {

    public Block block;
    public byte meta;
    public MeshData meshData;

    public abstract MeshData renderBlock(int x, int y, int z, bool[] renderFace);

    //Call before begining the rendering of a block, to set up the model
    public void preRender(Block block, byte meta, MeshData meshData) {
        this.block = block;
        this.meta = meta;
        this.meshData = meshData;
    }

    //Adds a face to the model
    public void addFace(Vector3 p1, Vector3 p2, Direction dir = null) {
        this.meshData.addVertex(p1);
        this.meshData.addVertex(new Vector3(p1.x, p2.y, p1.z));
        this.meshData.addVertex(p2);
        this.meshData.addVertex(new Vector3(p2.x, p1.y, p2.z));
        this.meshData.addQuadTriangles();
        this.meshData.uv.AddRange(this.getUVs(this.block, this.meta, dir));
    }

    //Adds a cube to the model
    public void addCube(Vector3 p1, Vector3 p2) {
        this.addFace(new Vector3(p1.x, p1.y, p1.z), new Vector3(p2.x, p1.y, p2.z), Direction.DOWN);
        this.addFace(new Vector3(p1.x, p2.y, p1.z), new Vector3(p2.x, p2.y, p2.x), Direction.UP);
        //this.addFace(new Vector3(), new Vector3(), Direction.UP);
        //this.addFace(new Vector3(), new Vector3(), Direction.UP);
        //this.addFace(new Vector3(), new Vector3(), Direction.UP);
        //this.addFace(new Vector3(), new Vector3(), Direction.UP);
    }

    //Returns the UV's to use for the passed direction
    public virtual Vector2[] getUVs(Block block, byte meta, Direction direction) {
        TexturePos tilePos = block.getTexturePos(direction, meta);
        float x = TexturePos.BLOCK_SIZE * tilePos.x;
        float y = TexturePos.BLOCK_SIZE * tilePos.y;
        Vector2[] UVs = new Vector2[4] {
            new Vector2(x, y),
            new Vector2(x, y + TexturePos.BLOCK_SIZE),
            new Vector2(x + TexturePos.BLOCK_SIZE, y + TexturePos.BLOCK_SIZE),
            new Vector2(x + TexturePos.BLOCK_SIZE, y)};
        return UVs;
    }
}
