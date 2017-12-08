using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockMushroom : Block {

        public BlockMushroom(int id) : base(id) {
            this.setTransparent();
            this.setMineTime(0.1f);
            this.setRenderer(RenderManager.MUSHROOM);
            this.setStatesUsed(2);
            this.setTexture(4, 5);
        }

        public override string getName(int meta) {
            return meta == 0 ? "Brown Mushroom" : "Purple Mushroom";
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return new ItemStack[] { new ItemStack(Block.mushroom, meta, 1) };
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        /*
        public override void onRandomTick(World world, int x, int y, int z, int meta, int tickSeed) {
            base.onRandomTick(world, x, y, z, meta, tickSeed);
            //TODO
        }
        */

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal, BlockState clickedBlock, Vector3 angle) {
            return world.getBlock(pos.move(Direction.DOWN)).isSolid;
        }
    }
}
