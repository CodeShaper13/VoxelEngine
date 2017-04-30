using VoxelEngine.Containers;

namespace VoxelEngine.Entities {

    /// <summary>
    /// Used by entities that should pickup items, EntityPlayer and EntityItem (for merging).
    /// </summary>
    public interface ICollecting {

        ItemStack tryPickupStack(ItemStack stack);

        float getPickupRadius();
    }
}
