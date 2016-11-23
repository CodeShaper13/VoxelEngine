public class ItemStack {
    public const int MAX_SIZE = 16;

    public Item item;
    public byte meta;
    public int count;

    public ItemStack(Item i, byte meta = 0, int count = 1) {
        this.item = i;
        this.meta = meta;
        this.count = count;
    }

    public bool equals(ItemStack stack) {
        return this.item.id == stack.item.id && this.meta == stack.meta;
    }

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
            return otherStack;
        }
    }
}
