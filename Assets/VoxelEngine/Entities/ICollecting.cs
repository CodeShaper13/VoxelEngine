using VoxelEngine.Containers;

namespace VoxelEngine.Entities {

    /// <summary>
    /// Used by entities that should pickup items, EntityPlayer and EntityItem (for merging).
    /// </summary>
    public interface ICollecting {

        /// <summary>
        /// Called when the implementing entity tries to pick a stack up, return anything that it can't hold.
        /// </summary>
        ItemStack tryPickupStack(ItemStack stack);

        /// <summary>
        /// Returns the distance that a player must be to an item to pick it up.
        /// </summary>
        float getPickupRadius();
    }
}
