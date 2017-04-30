using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockFarmland : Block {

        public BlockFarmland(int id) : base(id) {
            this.setStatesUsed(3);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            if(meta == 0) {
                return new TexturePos(4, 1);
            } else if(meta == 1) {
                return new TexturePos(4, 2);
            } else { // meta == 2.
                return new TexturePos(4, 3);
            }
        }

        public override bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace) {
            if(heldStack != null) {
                Item item = heldStack.item;
                if(item is ItemTool && ((ItemTool)item).toolType == EnumToolType.SHOVEL) {
                    if (meta < 2) {
                        // TODO Damage shovel
                        world.setBlock(pos, this, meta += 1);
                        return true;
                    }
                } else if(item == Item.corn) {
                    return this.tryPlant(world, player, pos, Block.cornCrop);
                } else if(item == Item.carrot) {
                    return this.tryPlant(world, player, pos, Block.carrotCrop);
                }
            }
            return false;
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.UP && world.getBlock(pos.move(neighborDir)).isSolid) {
                world.setBlock(pos, Block.dirt, 0);
            }
        }

        /// <summary>
        /// Tries to plant an item, returning true if the block was placed.
        /// </summary>
        private bool tryPlant(World world, EntityPlayer player, BlockPos pos, Block block) {
            if(world.getBlock(pos) == Block.farmland) {
                world.setBlock(pos.move(Direction.UP), block, 0);
                player.reduceHeldStackByOne();
                return true;
            }
            return false;
        }
    }
}
