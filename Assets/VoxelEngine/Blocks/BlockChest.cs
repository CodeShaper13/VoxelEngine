using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockChest : BlockTileEntity {

        public BlockChest(byte id) : base(id) {
            this.setRenderer(RenderManager.CHEST);
            this.setTransparent();
            this.setType(EnumBlockType.WOOD);
        }

        public override bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace) {
            if(!world.getBlock(pos.move(Direction.UP)).isSolid) {
                TileEntityChest chest = ((TileEntityChest)world.getTileEntity(pos));
                player.contManager.openContainer(player, ContainerManager.containerChest, chest.chestData);
                chest.chestOpen.setOpen(true);
                return true;
            }
            return false;
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDir, Vector3 angle) {
            if (Mathf.Abs(angle.x) > Mathf.Abs(angle.z)) { // X aixs
                if(angle.x < 0) {
                    return 1; // East
                } else {
                    return 3; // West
                }
            }
            else { // Z axis
                if (angle.z < 0) {
                    return 0; // North
                } else {
                    return 2; // South
                }
            }
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            ItemStack[] contents = ((TileEntityChest)world.getTileEntity(pos)).chestData.getRawItemArray();

            List<ItemStack> list = new List<ItemStack>();
            list.Add(new ItemStack(Block.chest));
            foreach(ItemStack stack in contents) {
                if(stack != null) {
                    list.Add(stack);
                }
            }
            return list.ToArray();
        }

        public override TileEntityBase getAssociatedTileEntity(World world, int x, int y, int z, int meta) {
            return new TileEntityChest(world, x, y, z, meta);
        }

        public static byte getMetaFromDirection(Direction dir) {
            if (dir.axis == EnumAxis.X || dir.axis == EnumAxis.Z) {
                return (byte)(dir.directionId - 1);
            }
            else {
                return 0;
            }
        }
    }
}
