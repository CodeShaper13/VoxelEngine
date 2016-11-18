using UnityEngine;
using System.Collections;

public class BlockModel {

	public virtual MeshData renderBlock(int x, int y, int z, Block block, byte blockMeta, MeshData meshData, bool[] renderFace) {
        if(renderFace[0]) {
            this.renderNorth(x, y, z, meshData, this.getUVs(block, blockMeta, Direction.all[0]));
        }
        if (renderFace[1]) {
            this.renderEast(x, y, z, meshData, this.getUVs(block, blockMeta, Direction.all[1]));
        }
        if (renderFace[2]) {
            this.renderSouth(x, y, z, meshData, this.getUVs(block, blockMeta, Direction.all[2]));
        }
        if (renderFace[3]) {
            this.renderWest(x, y, z, meshData, this.getUVs(block, blockMeta, Direction.all[3]));
        }
        if (renderFace[4]) {
            this.renderUp(x, y, z, meshData, this.getUVs(block, blockMeta, Direction.all[4]));
        }
        if (renderFace[5]) {
            this.renderDown(x, y, z, meshData, this.getUVs(block, blockMeta, Direction.all[5]));
        }

        return meshData;
    }

    //Adds all the faces to the mesh on the up side
    protected virtual MeshData renderUp(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the down side
    protected virtual MeshData renderDown(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the north side
    protected virtual MeshData renderNorth(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the east side
    protected virtual MeshData renderEast(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the south side
    protected virtual MeshData renderSouth(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the west side
    protected virtual MeshData renderWest(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Retunrs the UV's to use for the passed direction
    public virtual Vector2[] getUVs(Block block, byte meta, Direction direction) {
        Vector2[] UVs = new Vector2[4];
        TexturePos tilePos = block.getTexturePos(direction, meta);

        UVs[0] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x + TexturePos.BLOCK_SIZE, TexturePos.BLOCK_SIZE * tilePos.y);
        UVs[1] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x + TexturePos.BLOCK_SIZE, TexturePos.BLOCK_SIZE * tilePos.y + TexturePos.BLOCK_SIZE);
        UVs[2] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x, TexturePos.BLOCK_SIZE * tilePos.y + TexturePos.BLOCK_SIZE);
        UVs[3] = new Vector2(TexturePos.BLOCK_SIZE * tilePos.x, TexturePos.BLOCK_SIZE * tilePos.y);
        return UVs;
    }
}
