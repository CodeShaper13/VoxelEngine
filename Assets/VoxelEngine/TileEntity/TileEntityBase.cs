using fNbt;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.TileEntity {

    public abstract class TileEntityBase {
        public World world;
        public int posX;
        public int posY;
        public int posZ;

        public TileEntityBase(World world, int x, int y, int z) {
            this.world = world;
            this.posX = x;
            this.posY = y;
            this.posZ = z;
        }

        public virtual void onDestruction(World world, BlockPos pos, byte meta) {

        }

        public abstract int getId();

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtInt("id", this.getId()));
            tag.Add(new NbtInt("x", this.posX));
            tag.Add(new NbtInt("y", this.posY));
            tag.Add(new NbtInt("z", this.posZ));
            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {
            // It's not needed to read the position, as the constructor will always set them.
            // This methods is still called and is marked as virtual in case another TileEntity needs to read data
            //this.posX = tag.Get<NbtInt>("x").IntValue;
            //this.posY = tag.Get<NbtInt>("y").IntValue;
            //this.posZ = tag.Get<NbtInt>("z").IntValue;
        }

        public static TileEntityBase getTileEntityFromId(World world, BlockPos pos, NbtCompound tag) {
            switch(tag.Get<NbtInt>("id").IntValue) {
                case 1:
                    return new TileEntityChest(world, pos.x, pos.y, pos.z);
                case 2:
                    return new TileEntityGlorb(world, pos.x, pos.y, pos.z);
                case 3:
                    return new TileEntityLantern(world, pos.x, pos.y, pos.z);
                case 4:
                    return new TileEntityTorch(world, pos.x, pos.y, pos.z);
            }
            return null;
        }
    }
}
