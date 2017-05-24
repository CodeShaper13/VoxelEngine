using fNbt;
using System;
using VoxelEngine.Blocks;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Containers {

    [Serializable]
    public class ItemStack {

        /// <summary> The maximum size of a stack.  Used by both EntityItem and containers to know how large a stack can be. </summary>
        public const int MAX_SIZE = 32;

        public Item item;
        public int meta;
        public int count;

        public ItemStack(Item item, int meta = 0, int count = 1) {
            if (item == null) {
                throw new Exception("Type ItemStack can not be constructed with a null reference for item paremater!");
            }
            this.item = item;
            this.meta = meta;
            this.count = MathHelper.clamp(count, 0, item.maxStackSize);
        }

        public ItemStack(Block block, int meta = 0, int count = 1) : this(block.asItem(), meta, count) { }

        /// <summary>
        /// Creates a copy of the passed stack.
        /// </summary>
        public ItemStack(ItemStack stack) : this(stack.item, stack.meta, stack.count) { }

        /// <summary>
        /// Creates a stack from a saved NbtCompound.
        /// </summary>
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

            if (combinedTotal <= this.item.maxStackSize) {
                this.count = combinedTotal;
                return null; //there is nothing left in the old stack
            } else {
                //there will be some leftovers, find out how many
                int freeSpace = this.item.maxStackSize - this.count;
                this.count = this.item.maxStackSize;
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

        public override string ToString() {
            return "(Item: " + this.item.getName(this.meta) + ":" + this.meta + " x " + this.count + ")";
        }
    }
}
