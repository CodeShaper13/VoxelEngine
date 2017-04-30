using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class ItemThrowable : Item {

        public ItemThrowable(int id) : base(id) { }

        public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            stack.count -= 1;
            if (stack.count <= 0) {
                stack = null;
            }

            Entity entity = world.spawnEntity(EntityRegistry.throwable.prefab, player.mainCamera.position + player.mainCamera.forward, player.mainCamera.rotation);
            entity.gameObject.GetComponent<Rigidbody>().AddForce(player.mainCamera.forward * 20, ForceMode.Impulse);

            return stack;
        }
    }
}
