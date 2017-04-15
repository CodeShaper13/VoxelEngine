using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererPrimitiveCross : BlockRendererPrimitive {

        private MeshBuilder meshData;
        private int meta;

        public override MeshBuilder renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            this.meshData = meshData;
            this.meta = meta;

            this.addFace(b, new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), Direction.UP);
            this.addFace(b, new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), Direction.UP);
            this.addFace(b, new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), Direction.UP);
            this.addFace(b, new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), Direction.UP);
            return meshData;
        }

        // Adds a face to the model
        private void addFace(Block b, Vector3 p1, Vector3 p2, Direction dir) {
            meshData.addQuad(
                p1,
                new Vector3(p1.x, p2.y, p1.z),
                p2,
                new Vector3(p2.x, p1.y, p2.z),
                b.getUVs(this.meta, dir, this.uvArray),
                Direction.NONE_ID);
        }
    }
}
