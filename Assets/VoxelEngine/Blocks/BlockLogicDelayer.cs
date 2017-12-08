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
            float x = clickedPos.x;
            float z = clickedPos.z;

            int newMeta = -1;

            if(x >= 0 && z >= 0) {
                // top right
                newMeta = BitHelper.invertBit(meta, 2);
            }
            else if(x >= 0 && z < 0) {
                // top left
                newMeta = BitHelper.invertBit(meta, 3);
            }
            else if(x < 0 && z >= 0) {
                // bottom left
                newMeta = BitHelper.invertBit(meta, 4);
            }
            else {
                // bottom right
                newMeta = BitHelper.invertBit(meta, 5);
            }

            if (newMeta != -1) {
                world.setBlock(pos, null, newMeta);
            }

            return true;
        }

        public override bool acceptsWire(Direction directionOfWire, int meta) {
            return directionOfWire.axis == Direction.horizontal[meta].axis;
        }

        public override TexturePos getTopTexture(int rotation) {
            return new TexturePos(9, 5);
        }
    }
}
