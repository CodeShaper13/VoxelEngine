using fNbt;

namespace VoxelEngine.Containers.Data {

    public class ContainerData {

        public int width;
        public int height;
        public ItemStack[] items;

        public ContainerData(int width, int height) {
            this.width = width;
            this.height = height;
            this.items = new ItemStack[this.width * this.height];
        }

        /// <summary>
        /// Returns the stack at (x, y).
        /// </summary>
        public ItemStack getStack(int x, int y) {
            return this.items[x + this.width * y];
        }

        /// <summary>
        /// Sets the stack at (x, y).
        /// </summary>
        public void setStack(int x, int y, ItemStack stack) {
            this.items[x + this.width * y] = stack;
        }

        public ItemStack[] getRawItemArray() {
            return this.items;
        }

        /// <summary>
        /// Adds the passed stack to the container, returning any we couldn't add.
        /// </summary>
        public ItemStack addItemStack(ItemStack stack) {
            if (stack == null) {
                return null;
            }

            // First try to fill up any slots that already have items
            for (int i = 0; i < this.items.Length; i++) {
                //slot = this.slots[i];
                ItemStack contents = this.items[i];
                if (contents == null || (!contents.Equals(stack)) || contents.count >= contents.item.maxStackSize) {
                    continue;
                }
                // Stacks are equal and slot is not full
                stack = contents.merge(stack);

                if (stack == null) {
                    return null;
                }
            }

            // If we still have stuff to deposite, add it to an empty slot
            for (int i = 0; i < this.items.Length; i++) {
                if (this.items[i] == null) {
                    this.items[i] = stack;
                    return null;
                }
            }
            return stack;
        }

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            NbtList list = new NbtList("items", NbtTagType.Compound);
            
            for(int i = 0; i < this.items.Length; i++) {
                if(this.items[i] != null) {
                    NbtCompound tagStack = this.items[i].writeToNbt();
                    tagStack.Add(new NbtInt("slotIndex", i));
                    list.Add(tagStack);
                }
            }
            tag.Add(list);
            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {
            foreach(NbtCompound compound in tag.Get<NbtList>("items")) {
                this.items[compound.Get<NbtInt>("slotIndex").IntValue] = new ItemStack(compound);
            }
        }
    }
}

