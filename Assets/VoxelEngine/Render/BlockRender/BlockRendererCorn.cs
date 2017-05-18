using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererCorn : BlockRendererPrimitive {

        public override void renderBlock(Block b, int meta, MeshBuilder meshBuilder, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            this.addFace(b, meta, meshBuilder, new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            this.addFace(b, meta, meshBuilder, new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            this.addFace(b, meta, meshBuilder, new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            this.addFace(b, meta, meshBuilder, new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        }

        // Adds a face to the model
        private void addFace(Block b, int meta, MeshBuilder meshBuilder, Vector3 p1, Vector3 p2) {
            meshBuilder.addQuad(
                p1,
                new Vector3(p1.x, p2.y, p1.z),
                p2,
                new Vector3(p2.x, p1.y, p2.z),
                b.getUVs(meta, Direction.UP, this.preAllocatedUvArray),
                Direction.NONE_ID);
        }
    }
}
