using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public abstract class BlockTileEntity : Block {

        public override void onPlace(World world, BlockPos pos, byte meta) {
            world.addTileEntity(pos, this.getAssociatedTileEntity(world, pos.x, pos.y, pos.z, meta));
        }

        public override void onDestroy(World world, BlockPos pos, byte meta) {
            world.getTileEntity(pos).onDestruction(world, pos, meta);
            world.removeTileEntity(pos);
        }

        public abstract TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta);
    }
}
