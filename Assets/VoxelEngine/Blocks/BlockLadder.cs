using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockLadder : Block {

        public BlockLadder(byte id) : base(id) {
            this.setTransparent();
            this.setRenderer(RenderManager.LADDER);
        }

        public override void onNeighborChange(World world, BlockPos pos, byte meta, Direction neighborDir) {
            if (neighborDir == Direction.yPlane[meta] && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, byte meta, Direction clickedDirNormal) {
            return (clickedDirNormal.axis == EnumAxis.X || clickedDirNormal.axis == EnumAxis.Z) && !(world.getBlock(pos.move(clickedDirNormal)).isSolid);
        }

        public override byte adjustMetaOnPlace(World world, BlockPos pos, byte meta, Direction clickedDirNormal, Vector3 angle) {
            return BlockLadder.getMetaFromDirection(clickedDirNormal.getOpposite());
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public static byte getMetaFromDirection(Direction dir) {
            if (dir.axis == EnumAxis.X || dir.axis == EnumAxis.Z) {
                return (byte)(dir.directionId - 1);
            } else {
                return 0;
            }
        }
    }
}
