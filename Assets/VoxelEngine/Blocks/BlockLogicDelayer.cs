using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    // Bit's 0 and 1 are direction, 2 - 5 are pushed button state.
    public class BlockLogicDelayer : BlockLogicBase {

        public BlockLogicDelayer(int id) : base(id) {
            this.setRenderer(RenderManager.LOGIC_DELAYER);
        }

        public override bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace, Vector3 clickedPos) {
            return true;
        }

        public override bool acceptsWire(Direction directionOfWire, int meta) {
            return directionOfWire.axis == Direction.horizontal[meta].axis;
        }

        public override TexturePos getTopTexture(int rotation) {
            return new TexturePos(9, 6);
        }
    }
}
