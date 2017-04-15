using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockRail : Block {

        public BlockRail(byte id) : base(id) {
            this.setTexture(0, 13);
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            if(Mathf.Abs(angle.x) > Mathf.Abs(angle.z)) { // X aixs
                return this.getMetaForTurn(world, pos, 0);
                //if (!(world.getBlock(pos.move(Direction.EAST)) == Block.rail || world.getBlock(pos.move(Direction.WEST)) == Block.rail)) {
                //    if(world.getBlock(pos.move(Direction.NORTH)) == Block.rail || world.getBlock(pos.move(Direction.SOUTH)) == Block.rail) {
                //        return 1;
                //    }
                //}
            } else { // Z axis
                return this.getMetaForTurn(world, pos, 1);
                //if (!(world.getBlock(pos.move(Direction.NORTH)) == Block.rail || world.getBlock(pos.move(Direction.SOUTH)) == Block.rail)) {
                //    if (world.getBlock(pos.move(Direction.EAST)) == Block.rail || world.getBlock(pos.move(Direction.WEST)) == Block.rail) {
                //        return 0;
                //    }
                //}
            }
        }

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal) {
            return world.getBlock(pos.move(Direction.DOWN)).isSolid;
        }

        public override Vector2[] getUVs(int meta, Direction direction, Vector2[] uvArray) {
            bool flag = meta >= 2 && meta <= 5;
            float x = TexturePos.BLOCK_SIZE * (flag ? 1f : 0f);
            float y = TexturePos.BLOCK_SIZE * 13;
            if(meta == 1) {
                uvArray[0] = new Vector2(x, y);
                uvArray[1] = new Vector2(x, y + TexturePos.BLOCK_SIZE);
                uvArray[2] = new Vector2(x + TexturePos.BLOCK_SIZE, y + TexturePos.BLOCK_SIZE);
                uvArray[3] = new Vector2(x + TexturePos.BLOCK_SIZE, y);
            } else {
                uvArray[1] = new Vector2(x, y);
                uvArray[2] = new Vector2(x, y + TexturePos.BLOCK_SIZE);
                uvArray[3] = new Vector2(x + TexturePos.BLOCK_SIZE, y + TexturePos.BLOCK_SIZE);
                uvArray[0] = new Vector2(x + TexturePos.BLOCK_SIZE, y);
            }
            return uvArray;
        }

        private int getMetaForTurn(World world, BlockPos pos, int inMeta) {
            foreach (Direction dir in Direction.yPlane) {
                if (world.getBlock(pos.move(dir)) == Block.rail && world.getBlock(pos.move(dir.getClockwise())) == Block.rail) {
                    Debug.Log(dir.directionId + 1);
                    return dir.directionId + 1;
                }
            }
            return inMeta;
        }
    }
}
