using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class ItemMagnifyingGlass : Item {

        public ItemMagnifyingGlass(int id) : base(id) { }

        public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            string s = null;
            if (hit.unityRaycastHit.distance <= player.reach) {
                if (hit.hitState != null) {
                    s = hit.hitState.block.getMagnifyingText(hit.hitState.meta);
                }
                else if (hit.entity != null) {
                    s = hit.entity.getMagnifyingText();
                }
                if (s != null) {
                    player.setMagnifyingText(s);
                }
            }
            return stack;
        }
    }
}
