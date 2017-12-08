using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockBed : Block {

        public BlockBed(int id) : base(id) {
            this.setRenderer(RenderManager.BED);
            this.setTransparent();
            this.setType(EnumBlockType.WOOD);
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal, BlockState clickedBlock, Vector3 angle) {
            Direction d = MathHelper.angleToDirection(angle).getOpposite();
            BlockPos pos1 = pos.move(d);

            return world.getBlock(pos.move(Direction.DOWN)).isSolid && world.getBlock(pos1).isReplaceable && world.getBlock(pos1.move(Direction.DOWN)).isSolid;
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            base.onNeighborChange(world, pos, meta, neighborDir);
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            //world.setBlock(pos.move(MathHelper.angleToDirection(angle).getOpposite()), Block.bed, 4);
            return MathHelper.angleToDirection(angle).getOpposite().index - 1;
            //return base.adjustMetaOnPlace(world, pos, meta, clickedDirNormal, angle);
        }

        public static Direction getBedDirection(int meta) {
            return Direction.horizontal[meta & ~(1 << 2)];
        }
    }
}
