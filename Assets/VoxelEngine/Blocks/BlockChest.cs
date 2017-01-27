using System.Collections.Generic;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockChest : BlockTileEntity {

        public BlockChest(byte id) : base(id) { }

        public override void onRightClick(World world, EntityPlayer player, BlockPos pos, byte meta) {
            player.openContainer(References.list.containerChest, ((TileEntityChest)world.getTileEntity(pos)).chestData);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            ItemStack[] contents = ((TileEntityChest)world.getTileEntity(pos)).chestData.items;

            List<ItemStack> list = new List<ItemStack>();
            list.Add(new ItemStack(Block.chest));
            foreach(ItemStack stack in contents) {
                if(stack != null) {
                    list.Add(stack);
                }
            }
            return list.ToArray();
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, byte meta) {
            return new TileEntityChest(world, x, y, z);
        }
    }
}
