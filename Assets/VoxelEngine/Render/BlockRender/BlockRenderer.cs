using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRenderer {

        public bool renderInWorld;

        public BlockRenderer(bool renderInWorld) {
            this.renderInWorld = renderInWorld;
        }

        public abstract MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace);
    }
}
