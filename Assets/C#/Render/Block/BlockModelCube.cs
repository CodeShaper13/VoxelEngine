using UnityEngine;

public class BlockModelCube : BlockModel {

    public override MeshData renderBlock(int x, int y, int z, bool[] renderFace) {
        if (renderFace[0]) {
            this.renderNorth(x, y, z, meshData, this.getUVs(block, this.meta, Direction.all[0]));
        }
        if (renderFace[1]) {
            this.renderEast(x, y, z, meshData, this.getUVs(block, this.meta, Direction.all[1]));
        }
        if (renderFace[2]) {
            this.renderSouth(x, y, z, meshData, this.getUVs(block, this.meta, Direction.all[2]));
        }
        if (renderFace[3]) {
            this.renderWest(x, y, z, meshData, this.getUVs(block, this.meta, Direction.all[3]));
        }
        if (renderFace[4]) {
            this.renderUp(x, y, z, meshData, this.getUVs(block, this.meta, Direction.UP));
        }
        if (renderFace[5]) {
            this.renderDown(x, y, z, meshData, this.getUVs(block, this.meta, Direction.DOWN));
        }

        return meshData;
    }

    //Adds all the faces to the mesh on the up side
    private MeshData renderUp(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        meshData.addQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the down side
    private MeshData renderDown(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.addQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the north side
    private MeshData renderNorth(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.addQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the east side
    private MeshData renderEast(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.addQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the south side
    private MeshData renderSouth(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.addQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Adds all the faces to the mesh on the west side
    private MeshData renderWest(int x, int y, int z, MeshData meshData, Vector2[] uv) {
        meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.addVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.addQuadTriangles();
        meshData.uv.AddRange(uv);
        return meshData;
    }

    //Returns the UV's to use for the passed direction
    public override Vector2[] getUVs(Block block, byte meta, Direction direction) {
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

