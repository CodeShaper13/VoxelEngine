using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockMushroom : Block {
        private int textureY;

        public BlockMushroom(byte id, int textureY) : base(id) {
            this.textureY = textureY;
            this.setSolid(false);
            this.setMineTime(0.1f);
            this.setRenderer(new BlockRendererMesh(References.list.mushroomMesh).setUseRandomRot(true).setShiftVec(new Vector3(0, -0.5f, 0)));
        }

        public override void onNeighborChange(World world, BlockPos pos, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override void onRandomTick(World world, int x, int y, int z, byte meta, int tickSeed) {
            base.onRandomTick(world, x, y, z, meta, tickSeed);
            //TODO
        }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            return new TexturePos(5 + meta, textureY);
        }
    }
}
