using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public class BlockRendererCorn : BlockRendererPrimitive {

        public override void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks) {
            this.addFace(block, meta, meshBuilder, new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            this.addFace(block, meta, meshBuilder, new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            this.addFace(block, meta, meshBuilder, new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            this.addFace(block, meta, meshBuilder, new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        }

        // Adds a face to the model
        private void addFace(Block block, int meta, MeshBuilder meshBuilder, Vector3 p1, Vector3 p2) {
            meshBuilder.addPlane(
                block, meta,
                p1,
                new Vector3(p1.x, p2.y, p1.z),
                p2,
                new Vector3(p2.x, p1.y, p2.z),
                Direction.NONE);
        }
    }
}
