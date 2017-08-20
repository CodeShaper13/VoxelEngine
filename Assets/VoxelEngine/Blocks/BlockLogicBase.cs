using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Render.NewSys;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public abstract class BlockLogicBase : Block {

        public BlockLogicBase(int id) : base(id) {
            this.setTransparent();
            this.setRenderer(RenderManager.LOGIC_PLATE);
        }

        public override UvPlane getUvPlane(int meta, Direction direction) {
            if(direction.axis == EnumAxis.X || direction.axis == EnumAxis.Z) {
                return new UvPlane(new TexturePos(9, 1), 0, 0, 32, 4);
            } else if(direction == Direction.DOWN) {
                return new UvPlane(new TexturePos(9, 0), 0, 0, 32, 32); // Bottom
            } else {
                return new UvPlane(this.getTopTexture(), 0, 0, 32, 32); // Bottom
            }
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return base.adjustMetaOnPlace(world, pos, meta, clickedDirNormal, angle);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal) {
            return world.getBlock(pos.move(Direction.DOWN)).isSolid;
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public abstract TexturePos getTopTexture();
    }
}
