using UnityEngine;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {

        protected Vector2[] uvArray;

        public BlockRendererPrimitive() {
            this.uvArray = new Vector2[4];
            this.setRenderInWorld(true);
        }
    }
}
