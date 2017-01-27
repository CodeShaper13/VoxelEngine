using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoxelEngine.Level;
using VoxelEngine.TileEntity;

namespace VoxelEngine.Blocks {

    public class BlockTorch : BlockTileEntity {

        public BlockTorch(byte id) : base(id) {
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta) {
            return new TileEntityTorch(world, x, y, z);
        }
    }
}
