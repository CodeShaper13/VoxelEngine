using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    // Meta:
    // First 2 bits are direction
    // 3rd bit is pushed state where 0 = out, 1 = in.
    public class BlockButton : Block {

        public BlockButton(int id) : base(id) {
            this.setTransparent();
            this.setMineTime(0.5f);
            this.setTexture(9, 1);
            this.setRenderer(RenderManager.BUTTON);
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal, BlockState clickedBlock) {
            return (clickedDirNormal.axis == EnumAxis.X || clickedDirNormal.axis == EnumAxis.Z) && clickedBlock.block.isSolid;
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return clickedDirNormal.getOpposite().index - 1;
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.horizontal[meta &= ~(1 << 2)] && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace, Vector3 clickedPos) {
            if(!this.isPushed(meta)) {
                world.setBlock(pos, null, this.setPushed(meta, true));
                world.scheduleFutureTick(pos, 1);
                return true;
            }
            return false;
        }

        public override void applyScheduledTick(World world, BlockPos pos) {
            world.setBlock(pos, null, this.setPushed(world.getMeta(pos), false));
        }

        public override bool acceptsWire(Direction directionOfWire, int meta) {
            return true;
        }

        public bool isPushed(int meta) {
            return BitHelper.getBit(meta, 2);
        }

        public int setPushed(int meta, bool isPushed) {
            return BitHelper.setBit(meta, 2, isPushed);
        }
    }
}
