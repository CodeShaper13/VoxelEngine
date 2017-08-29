using VoxelEngine.Render;

namespace VoxelEngine.Blocks {

    public class BlockFence : Block {

        public BlockFence(int id) : base(id) {
            this.setTransparent();
            this.setRenderer(RenderManager.FENCE);
            this.setType(EnumBlockType.WOOD);
            this.setTexture(6, 3);
        }
    }
}
