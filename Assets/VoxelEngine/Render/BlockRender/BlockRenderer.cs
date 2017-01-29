using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRenderer {

        public bool renderInWorld;

        public BlockRenderer setRenderInWorld(bool flag) {
            this.renderInWorld = flag;
            return this;
        }

        public abstract MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace);
    }
}
