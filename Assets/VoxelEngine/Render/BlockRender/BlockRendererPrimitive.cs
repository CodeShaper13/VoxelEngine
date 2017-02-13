using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {

        public BlockRendererPrimitive() {
            this.setRenderInWorld(true);
        }
    }
}
