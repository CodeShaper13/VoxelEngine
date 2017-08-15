using UnityEngine;
using VoxelEngine.Entities;
using VoxelEngine.Entities.Registry;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockFalling : Block {

        public BlockFalling(int id) : base(id) {
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if(neighborDir == Direction.DOWN && world.getBlock(pos.move(Direction.DOWN)) == Block.air) {
                this.func(world, pos, meta);
            }
        }

        public override void onPlace(World world, BlockPos pos, int meta) {
            if(world.getBlock(pos.move(Direction.DOWN)) == Block.air) {
                this.func(world, pos, meta);
            }
        }

        private void func(World world, BlockPos pos, int meta) {
            world.setBlock(pos, Block.air, 0);
            EntityDynamicBlock block = (EntityDynamicBlock) world.spawnEntity(EntityRegistry.dynamicBlock, pos.toVector(), Quaternion.identity);
            block.setTile(this, meta);
        }
    }
}
