using System;
using VoxelEngine.Level;
using VoxelEngine.TileEntity;

namespace VoxelEngine.Blocks {

    public class BlockLantern : BlockTileEntity {

        public BlockLantern(byte id) : base(id) {
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta) {
            return new TileEntityLantern(world, x, y, z);
        }
    }
}
