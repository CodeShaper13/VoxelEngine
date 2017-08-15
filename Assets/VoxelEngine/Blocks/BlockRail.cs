using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockRail : Block {

        public BlockRail(byte id) : base(id) {
            this.setTexture(0, 13);
            this.setTransparent();
            this.setRenderer(RenderManager.RAIL);
            this.setContainerTransfrom(new MutableTransform(Vector3.zero, Quaternion.Euler(-90, 0, 0), new Vector3(0.2f, 0.2f, 0.2f)));
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

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return new TexturePos((meta >= 2 && meta <= 5) ? 1 : 0, 13);
        }

        public override Vector2[] applyUvAlterations(Vector2[] uvs, int meta, Direction direction, Vector2 faceRadius, Vector2 faceOffset) {
            if(meta == 0) {
                return UvHelper.rotateUVs(uvs, 90);
            }
            return uvs;
        }

        private int getMetaForTurn(World world, BlockPos pos, int inMeta) {
            foreach (Direction dir in Direction.yPlane) {
                if (world.getBlock(pos.move(dir)) == Block.rail && world.getBlock(pos.move(dir.getClockwise())) == Block.rail) {
                    Debug.Log(dir.index + 1);
                    return dir.index + 1;
                }
            }
            return inMeta;
        }
    }
}
