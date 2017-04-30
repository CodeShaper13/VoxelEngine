using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockCrop : Block {

        /// <summary> The meta that this crop is done growwing at. </summary>
        private int finishedMeta;
        private Item produce;
        private Item seed;
        private int minDrop;
        private int maxDrop;

        public BlockCrop(int id, Item produce, Item seed, int minDrop, int maxDrop, int finishedMeta) : base(id) {
            this.produce = produce;
            this.seed = seed;
            this.minDrop = minDrop;
            this.maxDrop = maxDrop;
            this.finishedMeta = finishedMeta;

            this.setTransparent();
            this.setStatesUsed(this.finishedMeta + 1);
        }

        public BlockCrop(int id, Item item, int minDrop, int maxDrop, int finishedMeta) : this(id, item, item, minDrop, maxDrop, finishedMeta) { }

        public override void onRandomTick(World world, int x, int y, int z, int meta, int tickSeed) {
            this.tryGrow(world, x, y, z, meta);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            bool finished = meta < this.finishedMeta;
            int i = finished ? Random.Range(this.minDrop, this.maxDrop) : 1;
            Item item = finished ? this.produce : this.seed;
            ItemStack[] drops = new ItemStack[i];
            for(int j = 0; j < i; j++) {
                drops[j] = new ItemStack(item, 0, 1);
            }
            return drops;
        }

        public override bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace) {
            if(heldStack != null && heldStack.item == Item.rawFish && meta < finishedMeta) {
                this.tryGrow(world, pos.x, pos.y, pos.z, meta);
                player.reduceHeldStackByOne();
                return true;
            }
            return false;
        }


        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            if (neighborDir == Direction.DOWN && world.getBlock(pos.move(neighborDir)) != Block.farmland) {
                world.breakBlock(pos, null);
            }
        }

        /// <summary>
        /// Called when the plant should try to grow.
        /// </summary>
        private void tryGrow(World world, int x, int y, int z, int meta) {
            if(meta < this.finishedMeta && Random.Range(0, 2) == 0) { // 1 in 2 chance.
                world.setBlock(x, y, z, this, meta++);
            }
        }
    }
}
