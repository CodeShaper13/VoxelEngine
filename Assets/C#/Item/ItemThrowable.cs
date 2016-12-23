using UnityEngine;

public class ItemThrowable : Item {

    public override ItemStack onRightClick(World world, EntityPlayer player, ItemStack stack, RaycastHit hit) {
        stack.count -= 1;
        if(stack.count <= 0) {
            stack = null;
        }

        Entity entity = world.spawnEntity(EntityManager.singleton.throwablePrefab, player.mainCamera.transform.position + player.mainCamera.transform.forward, player.mainCamera.transform.rotation);
        entity.gameObject.GetComponent<Rigidbody>().AddForce(player.mainCamera.transform.forward * 10, ForceMode.Impulse);
        
        return stack;
    }
}
