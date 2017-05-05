using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockWood : Block {

        public BlockWood(int id) : base(id) {
            this.setStatesUsed(3);
            this.setType(EnumBlockType.WOOD);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            TexturePos pos = new TexturePos(1, 1);
            if(meta == 0) { // X
                if (direction == Direction.EAST || direction == Direction.WEST) {
                    pos.x = 2;
                }
            } else if(meta == 1) { // Y
                if (direction == Direction.UP || direction == Direction.DOWN) {
                    pos.x = 2;
                }
            } else if(meta == 2) { // Z
                if (direction == Direction.NORTH || direction == Direction.SOUTH) {
                    pos.x = 2;
                }
            } else if(meta == 3) {
                pos = new TexturePos(2, 1);
            }
            return pos;
        }

        public override Vector2[] getUVs(int meta, Direction direction, Vector2[] uvArray) {
            uvArray = base.getUVs(meta, direction, uvArray);

            // Correct the texture rotation.
            if (meta == 0 && direction.axis == EnumAxis.Z) {
                return UvHelper.rotateUVs(uvArray, 90);
            } else if (meta == 2) {
                if(direction.axis == EnumAxis.X) {
                    return UvHelper.rotateUVs(uvArray, 90);
                } else if(direction.axis == EnumAxis.Y) {
                    return UvHelper.rotateUVs(uvArray, 90);
                }
            }
                       
            return uvArray;
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return (int)clickedDirNormal.axis;
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 1, brokenWith);
        }
    }
}
