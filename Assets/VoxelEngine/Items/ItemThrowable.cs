using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Entities.Registry;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Items {

    public class ItemThrowable : Item {

        private RegisteredEntity registeredEntity;

        public ItemThrowable(int id, RegisteredEntity entity) : base(id) {
            this.registeredEntity = entity;
        }

        public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
            Transform camera = player.mainCamera;

            Entity entity = world.spawnEntity(this.registeredEntity, camera.position + camera.forward, camera.rotation);
            entity.gameObject.GetComponent<Rigidbody>().AddForce(camera.forward * this.getThrowSpeed(), ForceMode.Impulse);

            return stack.safeDeduction();
        }

        protected virtual float getThrowSpeed() {
            return 20f;
        }
    }
}
