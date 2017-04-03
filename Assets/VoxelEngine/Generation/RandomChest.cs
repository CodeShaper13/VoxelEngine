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

        public static RandomChest MINESHAFT_SHAFT = new RandomChest(4, 8, new RandomStack[] {new RandomStack(1, Block.rail, 0, 2, 7)});
        public static RandomChest SPAWN_CHEST = new RandomChest(8, 10, new RandomStack[] {
            new RandomStack(2, Item.coal, 0, 5, 8),
            new RandomStack(2, Block.torch, 0, 6, 12),
            new RandomStack(2, Item.pebble, 0, 5, 8),
            new RandomStack(1, Item.bronzeBar, 0, 1, 2),
            new RandomStack(4, Block.wood, 0, 3, 7)
            });

        private RandomStack[] randomStacks;
        private int minStackCount;
        private int maxStackCount;

        public RandomChest(int min, int max, RandomStack[] rs) {
            this.minStackCount = min;
            this.maxStackCount = max;

            List<RandomStack> list = new List<RandomStack>();
            for(int i = 0; i < rs.Length; i++) {
                RandomStack rndStack = rs[i];
                for(int j = 0; j < rndStack.chance; j++) {
                    list.Add(rndStack);
                }
            }
            this.randomStacks = list.ToArray();
        }

        public void makeChest(World world, int x, int y, int z, Direction chestDirection, System.Random rnd) {
            world.setBlock(x, y, z, Block.chest, BlockChest.getMetaFromDirection(chestDirection));
            this.generateChestContents(((TileEntityChest)world.getTileEntity(x, y, z)).chestData, rnd);
        }

        public void generateChestContents(ContainerData data, Random rnd) {
            int i = rnd.Next(this.minStackCount, this.maxStackCount + 1);
            for (int j = 0; j < i; j++) {
                ItemStack stack = this.randomStacks[rnd.Next(this.randomStacks.Length)].getStack(rnd);
                data.items[rnd.Next(data.width * data.height)] = stack;
            }
        }

        public class RandomStack {

            public int chance;
            private Item item;
            private byte meta;
            private int minCount;
            private int maxCount;

            public RandomStack(int chance, Item item, byte meta, int minCount, int maxCount) {
                this.chance = chance;
                this.item = item;
                this.meta = meta;
                this.minCount = minCount;
                this.maxCount = maxCount;
            }

            public RandomStack(int chance, Block block, byte meta, int minCount, int maxCount) : this(chance, block.asItem(), meta, minCount, maxCount) { }

            public ItemStack getStack(Random rnd) {
                return new ItemStack(this.item, this.meta, rnd.Next(this.minCount, this.maxCount + 1));
            }
        }
    }
}
