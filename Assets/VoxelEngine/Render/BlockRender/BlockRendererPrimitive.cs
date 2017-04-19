using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {

        // Constants for light sampling directions.
        public const int NORTH = Direction.NORTH_ID;
        public const int EAST = Direction.EAST_ID;
        public const int SOUTH = Direction.SOUTH_ID;
        public const int WEST = Direction.WEST_ID;
        public const int UP = Direction.UP_ID;
        public const int DOWN = Direction.DOWN_ID;

        protected Vector2[] uvArray;

        public BlockRendererPrimitive() {
            this.uvArray = new Vector2[4];
            this.setRenderInWorld(true);
        }
    }
}
