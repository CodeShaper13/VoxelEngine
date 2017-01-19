using System;
using fNbt;
using VoxelEngine.Containers.Data;
using VoxelEngine.Level;

namespace VoxelEngine.TileEntity {

    public class TileEntityChest : TileEntityBase {

        public ContainerData chestData;

        public TileEntityChest(World world, int x, int y, int z) : base(world, x, y, z) {
            this.chestData = new ContainerData(2, 2);
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(this.chestData.writeToNbt(new NbtCompound("container")));
            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);
            this.chestData.readFromNbt(tag.Get<NbtCompound>("container"));
        }

        public override int getId() {
            return 1;
        }
    }
}
