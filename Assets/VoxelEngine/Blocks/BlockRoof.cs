using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockRoof : Block {

        private Block baseBlock;

        public BlockRoof(int id, Block baseBlock) : base(id) {
            this.baseBlock = baseBlock;
            this.setName(this.baseBlock.getName(0) + " Roof");
            this.setRenderer(RenderManager.ROOF);
            this.setMineTime(this.baseBlock.mineTime);
            this.setType(this.baseBlock.blockType);
            this.setStatesUsed(4);
            this.setTransparent();
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            if(direction == Direction.UP || direction == Direction.all[meta].getOpposite()) {
                return new TexturePos(6, 7);
            } else {
                return this.baseBlock.getTexturePos(direction, meta);
            }
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            clickedDirNormal = clickedDirNormal.getOpposite();
            if (clickedDirNormal == Direction.DOWN || clickedDirNormal == Direction.UP) {
                if (Mathf.Abs(angle.x) > Mathf.Abs(angle.z)) { // X aixs
                    return (angle.x > 0) ? 3 : 1;
                }
                else { // Z axis
                    return (angle.z > 0) ? 2 : 0;
                }
            }
            else {
                return BlockStairs.getMetaFromDirection(clickedDirNormal);
            }
        }

        public override Vector2[] applyUvAlterations(Vector2[] uvs, int meta, Direction direction, Vector2 faceRadius, Vector2 faceOffset) {
            //UvHelper.cropUVs(uvs, faceRadius);
            //UvHelper.smartShiftUVs(uvs, faceRadius, faceOffset);
            return uvs;
        }

        public static int getMetaFromDirection(Direction direction) {
            if (direction.axis == EnumAxis.X || direction.axis == EnumAxis.Z) {
                return direction.index - 1;
            } else {
                return 0;
            }
        }
    }
}
