using VoxelEngine.Render;

namespace VoxelEngine.Blocks {

    public class BlockFluid : Block {

        public BlockFluid(byte id) : base(id) {
            this.setReplaceable();
            this.setTransparent();
            this.setRenderer(RenderManager.FLUID);
        }
    }
}
