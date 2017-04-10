using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockTorch : BlockTileEntity {

        public BlockTorch(byte id) : base(id) {
            this.setRenderer(BlockRenderer.TORCH);
            this.setEmittedLight(7);
        }

        public override void onNeighborChange(World world, BlockPos pos, byte meta, Direction neighborDir) {
            Direction attached = (meta == 0 ? Direction.DOWN : Direction.yPlane[meta - 1]);
            if (neighborDir == attached && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, byte meta, Direction clickedDirNormal) {
            return (clickedDirNormal != Direction.DOWN) && world.getBlock(pos.move(clickedDirNormal.getOpposite())).isSolid;
        }

        public override byte adjustMetaOnPlace(World world, BlockPos pos, byte meta, Direction clickedDirNormal, Vector3 angle) {
            return BlockTorch.getMetaFromDirection(clickedDirNormal != Direction.DOWN ? clickedDirNormal.getOpposite() : clickedDirNormal);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta) {
            return new TileEntityTorch(world, x, y, z, meta);
        }

        public static byte getMetaFromDirection(Direction dir) {
            if(dir.axis == EnumAxis.X || dir.axis == EnumAxis.Z) {
                return (byte)dir.directionId;
            } else {
                return 0;
            }
        }
    }
}