using System;
using System.Collections.Generic;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Containers.Data;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.TileEntity;
using VoxelEngine.Util;

namespace VoxelEngine.Generation {

    public class RandomChest {

        public static RandomChest MINESHAFT_SHAFT = new RandomChest(4, 8, new RandomStack(1, Block.rail, 2, 7));
        public static RandomChest SPAWN_CHEST = new RandomChest(8, 10,
            new RandomStack(2, Item.coal, 5, 8),
            new RandomStack(2, Block.torch, 6, 12),
            new RandomStack(2, Item.pebble, 5, 8),
            new RandomStack(1, Item.bronzeBar, 1, 2),
            new RandomStack(4, Block.wood, 3, 7)
            );
        public static RandomChest MINESHAFT_BEDRROM_CHEST = new RandomChest(0, 4, new RandomStack(1, Block.torch, 2, 5));
        public static RandomChest MINESHAFT_STOREROOM = new RandomChest(0, 4, new RandomStack(1, Block.torch, 2, 5));
        public static RandomChest MINESHAFT_START_ROOM = new RandomChest(1, 4, new RandomStack(1, Block.torch, 2, 5));


        private RandomStack[] randomStacks;
        private int minStackCount;
        private int maxStackCount;

        private RandomChest(int min, int max, params RandomStack[] randomStacks) {
            this.minStackCount = min;
            this.maxStackCount = max;

            List<RandomStack> list = new List<RandomStack>();
            for(int i = 0; i < randomStacks.Length; i++) {
                RandomStack rndStack = randomStacks[i];
                for(int j = 0; j < rndStack.chance; j++) {
                    list.Add(rndStack);
                }
            }
            this.randomStacks = list.ToArray();
        }

        /// <summary>
        /// Places a random chest in the world, block and contents.
        /// </summary>
        public void makeChest(World world, int x, int y, int z, Direction chestDirection, System.Random rnd) {
            world.setBlock(x, y, z, Block.chest, BlockChest.getMetaFromDirection(chestDirection));
            ContainerData data = ((TileEntityChest)world.getTileEntity(x, y, z)).chestData;

            int stacksToAdd = rnd.Next(this.minStackCount, this.maxStackCount + 1);
            for (int i = 0; i < stacksToAdd; i++) {
                ItemStack stack = this.randomStacks[rnd.Next(this.randomStacks.Length)].getStack(rnd);
                data.items[rnd.Next(data.width * data.height)] = stack;
            }
        }

        private class RandomStack {

            public int chance;
            private Item item;
            private int meta;
            private int minCount;
            private int maxCount;

            public RandomStack(int chance, Item item, int minCount, int maxCount, int meta = 0) {
                this.chance = chance;
                this.item = item;
                this.meta = meta;
                this.minCount = minCount;
                this.maxCount = maxCount;
            }

            public RandomStack(int chance, Block block, int minCount, int maxCount, int meta = 0) :
                this(chance, block.asItem(), minCount, maxCount, meta) { }

            public ItemStack getStack(Random rnd) {
                return new ItemStack(this.item, this.meta, rnd.Next(this.minCount, this.maxCount + 1));
            }
        }
    }
}
