using UnityEngine;

public class BlockModelCross : BlockModel {

    public override MeshData renderBlock(int x, int y, int z, bool[] renderFace) {
        this.addFace(new Vector3(x - 0.5f, y, z - 0.5f), new Vector3(x + 0.5f, y + 1, z + 0.5f), Direction.UP);
        this.addFace(new Vector3(x + 0.5f, y, z + 0.5f), new Vector3(x - 0.5f, y + 1, z - 0.5f), Direction.UP);
        this.addFace(new Vector3(x + 0.5f, y, z - 0.5f), new Vector3(x - 0.5f, y + 1, z + 0.5f), Direction.UP);
        this.addFace(new Vector3(x - 0.5f, y, z + 0.5f), new Vector3(x + 0.5f, y + 1, z - 0.5f), Direction.UP);
        return meshData;
    }
}
