using UnityEngine;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {

        protected Vector2[] preAllocatedUvArray;

        public BlockRendererPrimitive() {
            this.preAllocatedUvArray = new Vector2[4];
            this.setRenderInWorld(true);
        }
    }
}
