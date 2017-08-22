using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockSlab : Block {

        private Block baseBlock;

        public BlockSlab(byte id, Block baseBlock) : base(id) {
            this.baseBlock = baseBlock;
            this.setName(this.baseBlock.getName(0) + " Slab");
            this.setRenderer(RenderManager.SLAB);
            this.setMineTime(this.baseBlock.mineTime);
            this.setType(this.baseBlock.blockType);
            this.setStatesUsed(7);
            this.setTransparent();
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            // Note, as this is called by ItemBlock, this method isn't called when a second slab is added to the same spot.
            return BlockSlab.getMetaFromDirection(clickedDirNormal.getOpposite()); 
        }

        public override bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace, Vector3 clickedPos) {
            if(heldStack.item.id == this.id && !BlockSlab.isFull(meta) && BlockSlab.getDirectionFromMeta(meta) == clickedFace.getOpposite()) {
                world.setBlock(pos, this, 6);
                player.reduceHeldStackByOne();
                return true;
            }
            return false;
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return new ItemStack [] { new ItemStack(this.asItem(), 0, BlockSlab.isFull(meta) ? 2 : 1) };
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return this.baseBlock.getTexturePos(direction, 0);
        }

        /*
          // Adjust the uvs for the not full side of a slab.
            if(!BlockSlab.isFull(meta)) {
                if(direction.axis != BlockSlab.getDirectionFromMeta(meta).axis) {
                    if(meta == 0) { // North.
                        uvArray[0].y += TexturePos.BLOCK_SIZE / 2;
                        uvArray[3].y += TexturePos.BLOCK_SIZE / 2;
                    } else if(meta == 1) { // East.
                        uvArray[1].y -= TexturePos.BLOCK_SIZE / 2;
                        uvArray[2].y -= TexturePos.BLOCK_SIZE / 2;
                    } else if(meta == 2) { // South.
                        uvArray[0].y += TexturePos.BLOCK_SIZE / 2;
                        uvArray[3].y += TexturePos.BLOCK_SIZE / 2;
                    } else if(meta == 3) { // West.
                        uvArray[1].y -= TexturePos.BLOCK_SIZE / 2;
                        uvArray[2].y -= TexturePos.BLOCK_SIZE / 2;
                    } else if(meta == 4) { // Down.
                        uvArray[0].y += TexturePos.BLOCK_SIZE / 2;
                        uvArray[3].y += TexturePos.BLOCK_SIZE / 2;
                    } else if(meta == 5) { //Up.
                        uvArray[1].y -= TexturePos.BLOCK_SIZE / 2;
                        uvArray[2].y -= TexturePos.BLOCK_SIZE / 2;
                    }
                }                
            }
            return uvArray;
        */

        public override Vector2[] applyUvAlterations(Vector2[] uvs, int meta, Direction direction, Vector2 faceRadius, Vector2 faceOffset) {
            if(!BlockSlab.isFull(meta)) {
                UvHelper.cropUVs(uvs, faceRadius);
                UvHelper.smartShiftUVs(uvs, faceRadius, faceOffset);
            }
            return uvs;
        }

        public static bool isFull(int meta) {
            return meta >= 6;
        }

        public static Direction getDirectionFromMeta(int meta) {
            return Direction.all[meta];
        }

        public static int getMetaFromDirection(Direction direction) {
            return direction.index - 1;
        }
    }
}
