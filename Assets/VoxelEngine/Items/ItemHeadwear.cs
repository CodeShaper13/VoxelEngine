using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.Items {

    public abstract class ItemHeadwear : Item {

        public ItemHeadwear(int id) : base(id) {
            this.setMaxStackSize(1);
        }

        public virtual void onEquip(World world, EntityPlayer player, ItemStack stack) { }

        public virtual void onRemove(World world, EntityPlayer player, ItemStack stack) { }

        /// <summary>
        /// Called every frame that the item is being worn to let the feature of it be updated.
        /// </summary>
        public virtual void onUpdate(World world, EntityPlayer player, ItemStack stack) { }
    }
}
