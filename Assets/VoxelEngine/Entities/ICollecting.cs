using VoxelEngine.Containers;

namespace VoxelEngine.Entities {

    // Used by entities that should pickup items, EntityPlayer and EntityItem (for merging)
    public interface ICollecting {

        ItemStack tryPickupStack(ItemStack stack);
    }
}
