using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class ItemFood : Item {

        public float restoredHunger;
        public int restoredHealth;

        public ItemFood(int id, float restoredHunger, int restoredHealth) : base(id) {
            this.restoredHunger = restoredHunger;
            this.restoredHealth = restoredHealth;
        }

        public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            if(player.health < 100) { // || player.hunger < 99f) {
                player.setHealth(player.health + this.restoredHealth);
                player.setHunger(player.hunger + this.restoredHunger);
                return stack.safeDeduction();
            }
            return stack;
        }
    }
}
