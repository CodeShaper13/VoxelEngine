using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockTorch : BlockTileEntity, ILightSource {

        public BlockTorch(byte id) : base(id) {
            this.setRenderer(BlockRenderer.TORCH);
        }

        public override void onNeighborChange(World world, BlockPos pos, byte meta, Direction neighborDir) {
            Direction attached = (meta == 0 ? Direction.DOWN : Direction.xzPlane[meta - 1]);
            if (neighborDir == attached && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, byte meta, Direction intendedDir) {
            return intendedDir != Direction.UP && world.getBlock(pos.move(intendedDir)).isSolid;
        }

        public override byte adjustMetaOnPlace(World world, BlockPos pos, byte meta, Direction clickedDir, Vector3 angle) {
            return BlockTorch.getMetaFromDirection(clickedDir);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta) {
            return new TileEntityTorch(world, x, y, z, meta);
        }

        public static byte getMetaFromDirection(Direction dir) {
            return dir != Direction.DOWN ? (byte)dir.directionId : (byte)0;
        }

        public GameObject getPrefab() {
            return References.list.torchPrefab;
        }
    }
}