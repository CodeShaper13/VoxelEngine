using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRenderer {

        /// <summary> False will make blocks not be baked into the world. </summary>
        public bool bakeIntoChunks = true;
        /// <summary> If true, the adjacent light levels will be looked up. </summary>
        public bool lookupAdjacentLight;
        /// <summary> If true, the adjacent blocks will be looked up. </summary>
        public bool lookupAdjacentBlocks;

        public abstract void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks);

        public BlockRenderer setRenderInWorld(bool flag) {
            this.bakeIntoChunks = flag;
            return this;
        }
    }
}
