using System.IO;
using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenPaused : GuiScreen {

        public override void onEscape() {
            // Dont call base, as it will try to open another gui.
            this.setVisible(false);
            Main.singleton.resumeGame();
        }

        public void CALLBACK_save(bool exitWorld) {
            this.playClickSound();

            World world = Main.singleton.worldObj;
            world.saveEntireWorld(exitWorld);

            if (exitWorld) {
                this.deleteWorldObjects();
                GuiManager.title.open();
            }
        }

        public void CALLBACK_resetMap() {
            this.playClickSound();
            //this.openGuiScreen(GuiManager.title);
            //return;

            this.deleteWorldObjects();
            Directory.Delete("saves/World_1", true);
            Main.singleton.createNewWorld();
        }

        public override GuiScreen getEscapeCallback() {
            return null;
        }

        /// <summary>
        /// Destroys all the GameObjects that make up the world.
        /// </summary>
        private void deleteWorldObjects() {
            Main main = Main.singleton;
            main.player.cleanupPlayerObj();
            GameObject.Destroy(main.player.gameObject);
            GameObject.Destroy(main.worldObj.gameObject);
            main.worldObj = null;
            main.player = null;
            main.isPaused = false;
            Time.timeScale = 1;
            main.showDebugText = false;
            Main.isDeveloperMode = false;
        }
    }
}
