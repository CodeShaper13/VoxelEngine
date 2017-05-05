using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRenderer {

        /// <summary> False will make blocks not be baked into the world. </summary>
        public bool bakeIntoChunks = true;
        /// <summary> If true, adjacent light level will be looked up and passes into the MeshBuilder. </summary>
        public bool lookupAdjacentLight = false;
        public bool lookupAdjacentBlocks = true;

        public abstract void renderBlock(Block b, int meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks);

        public BlockRenderer setRenderInWorld(bool flag) {
            this.bakeIntoChunks = flag;
            return this;
        }
    }
}
