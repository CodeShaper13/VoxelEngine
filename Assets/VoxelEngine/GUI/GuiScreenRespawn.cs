using UnityEngine.UI;
using VoxelEngine.Entities;

namespace VoxelEngine.GUI {

    public class GuiScreenRespawn : GuiScreen {

        public Text deathMessageText;

        public override void onEscape() {
            this.respawnPlayer();
        }

        public void CALLBACK_respawn() {
            this.respawnPlayer();

            this.playClickSound();
        }

        private void respawnPlayer() {
            this.setVisible(false);
            Main.singleton.resumeGame();
            EntityPlayer player = Main.singleton.player;
            player.setHealth(100);
            player.transform.position = Main.singleton.worldObj.worldData.spawnPos;
            player.damageEffect.clearEffect();
        }

        public override GuiScreen getEscapeCallback() {
            return null;
        }
    }
}
