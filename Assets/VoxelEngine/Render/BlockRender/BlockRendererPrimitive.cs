using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {

        /// <summary>
        /// This makes the block use its own cells light even if it crosses into other cells.
        /// Set this value in the constructor.
        /// </summary>
        public bool demandLocalLight = false;

        public BlockRendererPrimitive() {
            this.setRenderInWorld(true);
        }

        public virtual UvPlane getUvPlane(Block block, int meta, Direction faceDirection, int cubeIndex) {
            TexturePos pos = block.getTexturePos(faceDirection, meta);
            return new UvPlane(pos, 0, 0, 32, 32);
        }
    }
}
