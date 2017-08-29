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
            TexturePos core = new TexturePos(2, 1);

            if(meta == 0) { // X
                if (direction.axis == EnumAxis.X) {
                    return core;
                } else {
                    return new TexturePos(1, 1, 90);
                }
            } else if(meta == 1) { // Y
                if (direction.axis == EnumAxis.Y) {
                    return core;
                }
            } else if(meta == 2) { // Z
                if (direction.axis == EnumAxis.Z) {
                    return core;
                } else if(direction.axis == EnumAxis.X) {
                    return new TexturePos(1, 1, 90);
                }
            } else if(meta == 3) { // All core.
                return core;
            }
            return new TexturePos(1, 1);
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return (int)clickedDirNormal.axis;
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 1, brokenWith);
        }
    }
}
