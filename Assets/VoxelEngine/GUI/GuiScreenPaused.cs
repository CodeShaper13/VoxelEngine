using System;
using System.IO;
using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenPaused : GuiScreen {

        public override void onEscape() {
            // Dont call base, as it will try to open another gui

            this.setVisible(false);
            Main.singleton.resumeGame();
        }

        public void saveCallback(bool exitWorld) {
            World world = Main.singleton.worldObj;
            world.saveEntireWorld(exitWorld);
            if (exitWorld) {
                this.deleteWorldObjects();
                this.openGuiScreen(GuiManager.title); //Saves us needing a new field since we dont use base.escapeFallback
            }
        }

        public void callbackOptions() {
            this.openGuiScreen(GuiManager.options);
        }

        public void callbackHelp() {
            throw new NotImplementedException();
        }

        public void callbackResetMap() {
            this.deleteWorldObjects();
            Directory.Delete("saves/World_1", true);
            Main.singleton.createNewWorld();
        }

        public override GuiScreen getEscapeCallback() {
            return null;
        }

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
            main.isDeveloperMode = false;
        }
    }
}
