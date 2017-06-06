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

        /// <summary> The chest in the bedroom part of the center room. </summary>
        public static RandomChest MINESHAFT_BEDRROM_CHEST = new RandomChest(3, 5,
            new RandomStack(2, Item.carrot, 2, 5),
            new RandomStack(2, Item.rawFish, 1, 3),
            new RandomStack(3, Item.bone, 1, 3),
            new RandomStack(1, Item.skull, 1, 3));
        /// <summary> The chest in the storeroom part of the center room. </summary>
        public static RandomChest MINESHAFT_STOREROOM = new RandomChest(2, 5,
            new RandomStack(3, Block.torch, 5, 15),
            new RandomStack(1, Item.pickaxe, 1, 1),
            new RandomStack(1, Item.axe, 1, 1),
            new RandomStack(1, Item.shovel, 1, 1),
            new RandomStack(1, Item.magnifyingGlass, 1, 1),
            new RandomStack(4, Item.bone, 1, 3));
        public static RandomChest MINESHAFT_START_ROOM = new RandomChest(1, 4,
            new RandomStack(1, Block.torch, 12, 16),
            new RandomStack(1, Block.wood, 2, 5, 1));
        public static RandomChest MINESHAFT_SHAFT = new RandomChest(1, 4, new RandomStack(1, Block.torch, 2, 5));

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
            world.setBlock(x, y, z, Block.chest, BlockChest.getMetaFromDirection(chestDirection), false, false);
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
