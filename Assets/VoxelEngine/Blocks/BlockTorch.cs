using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockTorch : BlockTileEntity {

        public BlockTorch(int id) : base(id) {
            this.setRenderer(RenderManager.TORCH);
            this.setEmittedLight(12);
            this.setTransparent();
            this.setType(EnumBlockType.WOOD);
            this.setTexture(3, 2);
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            Direction attached = (meta == 0 ? Direction.DOWN : Direction.yPlane[meta - 1]);
            if (neighborDir == attached && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal) {
            Block clickedBlock = world.getBlock(pos.move(clickedDirNormal.getOpposite()));
            return ((clickedDirNormal != Direction.DOWN) && clickedBlock.isSolid) ||(clickedDirNormal == Direction.UP && (clickedBlock == Block.fence || clickedBlock == Block.ironFence));
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return BlockTorch.getMetaFromDirection(clickedDirNormal != Direction.DOWN ? clickedDirNormal.getOpposite() : clickedDirNormal);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, int meta) {
            return new TileEntityTorch(world, x, y, z, meta);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            int i;
            if (direction == Direction.UP) {
                i = 1;
            } else if (direction == Direction.DOWN) {
                i = 2;
            } else {
                i = 0;
            }
            return new TexturePos(8, i);
        }

        public static int getMetaFromDirection(Direction dir) {
            if(dir.axis == EnumAxis.X || dir.axis == EnumAxis.Z) {
                return dir.directionId;
            } else {
                return 0;
            }
        }
    }
}