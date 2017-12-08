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
            this.setType(EnumBlockType.WOOD);
            this.setContainerTransfrom(new MutableTransform(Vector3.zero, Quaternion.Euler(0, 0, 0), new Vector3(0.2f, 0.2f, 0.2f)));
            this.setTexture(7, 0);
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.horizontal[meta] && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal, BlockState clickedBlock, Vector3 angle) {
            return (clickedDirNormal.axis == EnumAxis.X || clickedDirNormal.axis == EnumAxis.Z) && clickedBlock.block.isSolid;
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return BlockLadder.getMetaFromDirection(clickedDirNormal.getOpposite());
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public static int getMetaFromDirection(Direction dir) {
            return (dir.axis == EnumAxis.X || dir.axis == EnumAxis.Z) ? dir.index - 1 : 0;
        }
    }
}
