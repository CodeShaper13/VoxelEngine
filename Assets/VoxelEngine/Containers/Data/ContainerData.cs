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

        public ItemStack getStack(int x, int y) {
            return this.items[x + this.width * y];
        }

        public void setStack(int x, int y, ItemStack stack) {
            this.items[x + this.width * y] = stack;
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

