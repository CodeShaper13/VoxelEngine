namespace VoxelEngine.Containers.Data {

    public class ContainerDataHotbar : ContainerData {

        public int index;

        public ContainerDataHotbar() : base(9, 1) {
        }

        public ItemStack addItemStack(ItemStack stack) {
            for (int i = 0; i < 9; i++) {
                if (this.items[i] == null) {
                    this.items[i] = stack;
                    return null;
                }
                ItemStack leftover = this.items[i].merge(stack);
                if (leftover == null || leftover.count == 0) {
                    return null;
                }
            }
            return stack;
        }

        public ItemStack dropItem(int i, bool wholeStack) {
            if (this.items[i] != null) {
                ItemStack s = new ItemStack(this.items[i].item, this.items[i].meta, wholeStack ? this.items[i].count : 1);
                this.items[i].count -= wholeStack ? this.items[i].count : 1;
                if (this.items[i].count <= 0) {
                    this.items[i] = null;
                }
                return s;
            }
            return null;
        }

        public ItemStack getHeldItem() {
            return this.items[this.index];
        }

        public void setHeldItem(ItemStack itemStack) {
            this.items[this.index] = itemStack;
        }
    }
}
