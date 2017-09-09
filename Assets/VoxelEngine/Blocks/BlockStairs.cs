using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    // Note!  Stairs meta goes with direction in the normal order, with the direction being the higher side.

    public class BlockStairs : Block {

        private Block baseBlock;

        public BlockStairs(int id, Block baseBlock) : base(id) {
            this.baseBlock = baseBlock;
            this.setName(this.baseBlock.getName(0) + " Stairs");
            this.setRenderer(RenderManager.STAIR);
            this.setMineTime(this.baseBlock.mineTime);
            this.setType(this.baseBlock.blockType);
            this.setTransparent();
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return this.baseBlock.getTexturePos(direction, 0);
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            clickedDirNormal = clickedDirNormal.getOpposite();
            if(clickedDirNormal == Direction.DOWN || clickedDirNormal == Direction.UP) {
                if (Mathf.Abs(angle.x) > Mathf.Abs(angle.z)) { // X aixs
                    return (angle.x > 0) ? 3 : 1;
                } else { // Z axis
                    return (angle.z > 0) ? 2 : 0;
                }
            } else {
                return BlockStairs.getMetaFromDirection(clickedDirNormal);
            }
        }

        public static int getMetaFromDirection(Direction direction) {
            if(direction.axis == EnumAxis.X || direction.axis == EnumAxis.Z) {
                return direction.index - 1;
            } else {
                return 0;
            }
        }

        public static Direction getDirectionFromMeta(int meta) {
            return Direction.all[meta];
        }
    }
}
