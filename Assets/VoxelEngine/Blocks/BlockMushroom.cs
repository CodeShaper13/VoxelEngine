using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Render.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockMushroom : Block {
        private int textureY;

        public BlockMushroom(int textureY) : base() {
            this.textureY = textureY;
        }

        public override void onNeighborChange(World world, BlockPos pos, Direction neighborDir) {
            Debug.Log(world.getBlock(pos.move(neighborDir)).name);
            if (!world.getBlock(pos.move(neighborDir)).isSideSolid(neighborDir)) {
                world.breakBlock(pos, null);
            }
        }

        public override void onRandomTick(World world, BlockPos pos, byte meta, int tickSeed) {
            base.onRandomTick(world, pos, meta, tickSeed);
            //TODO
        }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            return new TexturePos(5 + meta, textureY);
        }

        public override BlockModel getModel(byte meta) {
            return Block.MODEL_CROSS;
        }
    }
}
