using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {

        /// <summary>
        /// 0 = Normal vertex light lookup
        /// 1 = Use the blocks cell lighting for all verts.
        /// 2 = Use the above blocks lighting for all verts.
        /// </summary>
        public int forcedLightMode = 0;

        public BlockRendererPrimitive() {
            this.setRenderInWorld(true);
        }

        public virtual UvPlane getUvPlane(Block block, int meta, Direction faceDirection, CubeComponent cubeComponent) {
            return new UvPlane(block.getTexturePos(faceDirection, meta), 1, 1, 32, 32);
        }
    }
}
