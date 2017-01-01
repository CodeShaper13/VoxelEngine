using UnityEngine;

public class GuiScreenWorldSelect : GuiScreen {

    public void loadCallback() {
        VoxelEngine v = VoxelEngine.singleton;
        v.worldObj = GameObject.Instantiate(v.worldPrefab).GetComponent<World>();
        v.player = (EntityPlayer)v.worldObj.spawnEntity(EntityManager.singleton.playerPrefab, v.worldObj.generator.getSpawnPoint(), Quaternion.identity);

        v.currentGui.setActive(false);
        v.currentGui = null;

        VoxelEngine.setMouseLock(true);
    }

    public void renameCallback() {

    }
    
    public void deleteCallback() {

    }
}
