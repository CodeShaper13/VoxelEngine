using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Render.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class ItemBlock : Item {

        public static IRenderItem RENDER_BLOCK = new RenderItemBlock();

        public Block block;

        public ItemBlock(Block block) : base(block.id) {
            this.block = block;
            this.id = block.id;
            if(this.block.renderAsItem) {
                this.setRenderer(Item.RENDER_BILLBOARD);
                this.setTexture(this.block.itemAtlasPos.x, this.block.itemAtlasPos.y);
            } else {
                this.setRenderer(ItemBlock.RENDER_BLOCK);
            }
        }

        public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            if (hit != null && hit.unityRaycastHit.distance <= player.reach) {
                BlockPos pos = BlockPos.fromRaycastHit(hit.unityRaycastHit);

                Vector3 angle = new Vector3(player.transform.position.x, 0, player.transform.position.z)- new Vector3(hit.unityRaycastHit.point.x, 0, hit.unityRaycastHit.point.z);

                Direction clickedDirNormal = hit.getClickedBlockFace();
                BlockPos newPos = pos.move(clickedDirNormal);
                int meta = stack.meta;
                if (world.getBlock(newPos).replaceable && this.block.isValidPlaceLocation(world, newPos, meta, clickedDirNormal)) {
                    world.setBlock(newPos, this.block, this.block.adjustMetaOnPlace(world, newPos, meta, clickedDirNormal, angle));
                    stack = stack.safeDeduction();
                }
            }
            return stack;
        }

        public override int getStatesUsed() {
            return this.block.statesUsed;
        }

        public override string getName(int meta) {
            return this.block.getName(meta);
        }
    }
}
