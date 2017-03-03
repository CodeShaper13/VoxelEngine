using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockWood : Block {

        public BlockWood(byte id) : base(id) { }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            TexturePos pos = new TexturePos(1, 1);
            // X
            if(meta == 0) {
                if (direction == Direction.EAST || direction == Direction.WEST) {
                    pos.x = 2;
                }
            // Y
            } else if(meta == 1) {
                if (direction == Direction.UP || direction == Direction.DOWN) {
                    pos.x = 2;
                }
            // Z
            } else if(meta == 2) {
                if (direction == Direction.NORTH || direction == Direction.SOUTH) {
                    pos.x = 2;
                }
            }
            return pos;
        }

        public override byte adjustMetaOnPlace(World world, BlockPos pos, byte meta, Direction clickedDirNormal, Vector3 angle) {
            return (byte)clickedDirNormal.axis;
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 1, brokenWith);
        }
    }
}
