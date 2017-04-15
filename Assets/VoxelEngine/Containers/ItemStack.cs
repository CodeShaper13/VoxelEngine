using fNbt;
using System;
using VoxelEngine.Blocks;
using VoxelEngine.Items;

namespace VoxelEngine.Containers {

    [Serializable]
    public class ItemStack {

        public const int MAX_SIZE = 32;

        public Item item;
        public int meta;
        public int count;

        public ItemStack(Item i, int meta = 0, int count = 1) {
            this.item = i;
            this.meta = meta;
            this.count = count;
        }

        public ItemStack(Block block, int meta = 0, int count = 1) : this(block.asItem(), meta, count) { }

        // Copies a stack
        public ItemStack(ItemStack stack) : this(stack.item, stack.meta, stack.count) { }

        public ItemStack(NbtCompound tag) {
            this.item = Item.ITEM_LIST[tag.Get<NbtInt>("id").IntValue];
            this.meta = tag.Get<NbtByte>("meta").ByteValue;
            this.count = tag.Get<NbtInt>("count").IntValue;
        }

        /// <summary>
        /// Returns true if the stacks share id and meta
        /// </summary>
        public bool equals(ItemStack stack) {
            return this.item.id == stack.item.id && this.meta == stack.meta;
        }

        /// <summary>
        /// Merges two stacks together, returning any left over or null if there is none left.
        /// </summary>
        public ItemStack merge(ItemStack otherStack) {
            if (!this.equals(otherStack)) {
                return otherStack;
            }

            int combinedTotal = this.count + otherStack.count;

            if (combinedTotal <= ItemStack.MAX_SIZE) {
                this.count = combinedTotal;
                return null; //there is nothing left in the old stack
            }
            else {
                //there will be some leftovers, find out how many
                int freeSpace = ItemStack.MAX_SIZE - this.count;
                this.count = ItemStack.MAX_SIZE;
                otherStack.count -= freeSpace;
                if(otherStack.count <= 0) {
                    return null;
                }
                return otherStack;
            }
        }

        /// <summary>
        /// Removes items from the stack, returning it or null if the count is less than 0.
        /// </summary>
        public ItemStack safeDeduction(int i = 1) {
            this.count -= i;
            if (count <= 0) {
                return null;
            }
            return this;
        }

        /// <summary>
        /// Saves the stack to NBT.  See constructor for the reading.
        /// </summary>
        public NbtCompound writeToNbt() {
            NbtCompound tag = new NbtCompound("stack");
            tag.Add(new NbtInt("id", this.item.id));         
            tag.Add(new NbtByte("meta", (byte)this.meta));
            tag.Add(new NbtInt("count", this.count));
            return tag;
        }
    }
}
