using UnityEngine.UI;
using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenRespawn : GuiScreen {

        public Text deathMessageText;

        public override GuiScreen onEscape(Main voxelEngine) {
            this.setActive(false);
            this.respawnPlayer(voxelEngine.worldObj, voxelEngine.player);
            return null;
        }

        public void respawnButtonCallback() {
            this.setActive(false);
            this.respawnPlayer(Main.singleton.worldObj, Main.singleton.player);
        }

        private void respawnPlayer(World world, EntityPlayer player) {
            Main.singleton.resumeGame();
            player.health = 10;
            player.transform.position = world.worldData.spawnPos;
            player.damageEffect.clearEffect();
        }
    }
}
