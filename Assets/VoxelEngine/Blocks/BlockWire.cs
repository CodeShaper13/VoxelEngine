using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Render.NewSys;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockWire : Block {

        public BlockWire(int id) : base(id) {
            this.setTransparent();
            this.setTexture(4, 0);
            this.setRenderer(RenderManager.WIRE);
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            // Break if the supporting block is gone.
            if (neighborDir == Direction.DOWN && !world.getBlock(pos.move(neighborDir)).isSolid) {
                world.breakBlock(pos, null);
            }

            this.updateWire(world, pos, meta);
        }

        public override void onPlace(World world, BlockPos pos, int meta) {
            this.updateWire(world, pos, meta);
        }

        // TODO when placing higher connect to lower.
        private void updateWire(World world, BlockPos pos, int meta) {
            int newMeta = meta;

            BlockPos posNeighbor;
            BlockPos posUp = new BlockPos();
            BlockPos posDown = new BlockPos();
            int j, k, flag; // 0 = do nothing, 1 = adjacent,  =2 up.
            Direction direction;
            for(int i = 0; i < 4; i++) {
                flag = 0;
                direction = Direction.all[i];

                // Check for adjacent wires.
                posNeighbor = pos + direction.blockPos;
                if (world.getBlock(posNeighbor).acceptsWire(direction, world.getMeta(posNeighbor))) {
                    newMeta = BitHelper.setBit(newMeta, i * 2);
                    flag = 1;
                }

                // Check for above wires.
                if(world.getBlock(pos.move(Direction.UP)) == Block.air) { // Only connect if nothing is above
                    posUp = posNeighbor.move(Direction.UP);
                    if(world.getBlock(posUp).acceptsWire(direction, world.getMeta(posUp))) {
                        newMeta = BitHelper.setBit(newMeta, i * 2);
                        newMeta = BitHelper.setBit(newMeta, (i * 2) + 1);
                        flag = 2;
                    }
                }

                // Check for below wires.
                if (world.getBlock(posNeighbor) == Block.air) { // Only connect if nothing is above
                    posDown = posNeighbor.move(Direction.DOWN);
                    if (world.getBlock(posDown).acceptsWire(direction, world.getMeta(posDown))) {
                        newMeta = BitHelper.setBit(newMeta, i * 2);
                        // Set down neighbor meta.
                        k = direction.getOpposite().index - 1;
                        j = BitHelper.setBit(world.getMeta(posDown), k * 2);
                        j = BitHelper.setBit(j, (k * 2) + 1);
                        world.setBlock(posDown, null, j, false, false);
                    }
                }

                // Apply the changes to this wire.
                if (flag != 0) {
                    BlockPos p1 = flag == 1 ? posNeighbor : posUp;
                    world.setBlock(p1, null, BitHelper.setBit(world.getMeta(p1), ((direction.getOpposite().index - 1) * 2)), false, false);
                }
            }

            if (newMeta != meta) {
                world.setBlock(pos, null, newMeta, false, false);
            }
        }

        /*
        public override Vector2[] applyUvAlterations(Vector2[] uvs, int meta, Direction direction, Vector2 faceRadius, Vector2 faceOffset) {
            if (meta == 0) {
                UvHelper.cropUVs(uvs, faceRadius);
                UvHelper.smartShiftUVs(uvs, faceRadius, faceOffset);
            } else {
                UvHelper.cropUVs(uvs, faceRadius);
            }
            return uvs;
        }
        */

        public override bool isValidPlaceLocation(World world, BlockPos pos, int meta, Direction clickedDirNormal) {
            return world.getBlock(pos.move(Direction.DOWN)).isSolid;
        }

        public override UvPlane getUvPlane(int meta, Direction direction) {
            return base.getUvPlane(meta, direction);
        }

        public override string getAsDebugText(int meta) {
            return this.name + ":" + meta + "\n" +
                "  N: " + (BitHelper.getBit(meta, 0) == 1 ? "true" : "false") + "\n" +
                "  N UP: " + (BitHelper.getBit(meta, 1) == 1 ? "true" : "false") + "\n" +
                "  E: " + (BitHelper.getBit(meta, 2) == 1 ? "true" : "false") + "\n" +
                "  E UP: " + (BitHelper.getBit(meta, 3) == 1 ? "true" : "false") + "\n" +
                "  S: " + (BitHelper.getBit(meta, 4) == 1 ? "true" : "false") + "\n" +
                "  S UP: " + (BitHelper.getBit(meta, 5) == 1 ? "true" : "false") + "\n" +
                "  W: " + (BitHelper.getBit(meta, 6) == 1 ? "true" : "false") + "\n" +
                "  W UP: " + (BitHelper.getBit(meta, 7) == 1 ? "true" : "false") + "\n" +
                "  POWER: " + BlockWire.getPowerLevel(meta);
        }

        public override bool acceptsWire(Direction directionOfWire, int meta) {
            return true;
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        /*
        public static bool isConnected(Direction direction, int meta) {
            if(direction.axis == EnumAxis.X || direction.axis == EnumAxis.Z) {
                return BitHelper.getBit(meta, direction.index - 1) == 1;
            }
            return false;
        }
        */

        public static int getPowerLevel(int meta) {
            return meta >> 8;
        }
    }
}
