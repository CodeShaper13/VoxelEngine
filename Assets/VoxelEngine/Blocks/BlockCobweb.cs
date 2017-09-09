using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockCobweb : Block {

        // Bits 0 and 1 are the direction the web is attached to, 2 is up/down.
        public BlockCobweb(int id) : base(id) {
            this.setTexture(8, 2);
            this.setTransparent();
            this.setReplaceable();
            this.setRenderer(RenderManager.COBWEB);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return null;
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            //TODO
            base.onNeighborChange(world, pos, meta, neighborDir);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            int mask = BlockCobweb.isHangingFromAbove(meta) ? TexturePos.MIRROR_Y : 0;

            if (direction == Direction.EAST) {
                return new TexturePos(8, 2, 0, mask);
            } else {
                return new TexturePos(8, 2, 0, mask | TexturePos.MIRROR_X);
            }
        }

        public static int getMetaForState(Direction dir, bool hangingFromAbove) {
            return BitHelper.setBit((dir.index - 1), 2, hangingFromAbove);
        }

        public static bool isHangingFromAbove(int meta) {
            return BitHelper.getBit(meta, 2);
        }

        public static Direction getYPlaneDirection(int meta) {
            return Direction.horizontal[meta & ~(1 << 2)];
        }
    }
}
