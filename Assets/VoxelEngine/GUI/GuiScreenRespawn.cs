using UnityEngine.UI;
using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenRespawn : GuiScreen {

        public Text deathMessageText;

        public override void onEscape() {
            this.respawnPlayer();
        }

        public void respawnButtonCallback() {
            this.respawnPlayer();
        }

        private void respawnPlayer() {
            this.setVisible(false);
            this.respawnPlayer(Main.singleton.worldObj, Main.singleton.player);
        }

        private void respawnPlayer(World world, EntityPlayer player) {
            Main.singleton.resumeGame();
            player.health = 10;
            player.transform.position = world.worldData.spawnPos;
            player.damageEffect.clearEffect();
        }

        public override GuiScreen getEscapeCallback() {
            return null;
        }
    }
}
