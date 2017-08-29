using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {
        
        public BlockRendererPrimitive() {
            this.setRenderInWorld(true);
        }

        public virtual UvPlane getUvPlane(Block block, int meta, Direction faceDirection, int cubeIndex) {
            TexturePos pos = block.getTexturePos(faceDirection, meta);
            return new UvPlane(pos, 0, 0, 32, 32);
        }
    }
}
