using UnityEngine;

public class ItemThrowable : Item {

    public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, PlayerRayHit hit) {
        stack.count -= 1;
        if(stack.count <= 0) {
            stack = null;
        }

        Entity entity = world.spawnEntity(EntityManager.singleton.throwablePrefab, player.mainCamera.position + player.mainCamera.forward, player.mainCamera.rotation);
        entity.gameObject.GetComponent<Rigidbody>().AddForce(player.mainCamera.forward * 20, ForceMode.Impulse);
        
        return stack;
    }
}
