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
            return this.items[x * this.width + y];
        }

        public void setStack(int x, int y, ItemStack stack) {
            this.items[x * this.width + y] = stack;
        }
    }
}

