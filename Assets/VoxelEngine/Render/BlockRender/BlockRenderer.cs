using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRenderer {

        /// <summary> False will make blocks not be baked into the world. </summary>
        public bool bakeIntoChunks = true;
        /// <summary> If true, the adjacent light levels will be looked up. </summary>
        public bool lookupAdjacentLight = false;
        /// <summary>
        /// If true, the adjacent blocks will be looked up.
        /// Set if the block is rendered differently based on its neighbors.
        /// </summary>
        public bool lookupAdjacentBlocks = false;

        public abstract void renderBlock(Block block, int meta, MeshBuilder meshBuilder, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks);

        public BlockRenderer setRenderInWorld(bool flag) {
            this.bakeIntoChunks = flag;
            return this;
        }
    }
}
