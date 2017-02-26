using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenPaused : GuiScreen {

        public override GuiScreen onEscape(Main voxelEngine) {
            this.setActive(false);
            voxelEngine.resumeGame();
            return null;
        }

        public void saveCallback(bool exitWorld) {
            World world = Main.singleton.worldObj;
            world.saveEntireWorld(exitWorld);
            if (exitWorld) {
                Main ve = Main.singleton;
                ve.player.cleanupPlayerObj();
                GameObject.Destroy(ve.player.gameObject);
                GameObject.Destroy(world.gameObject);
                this.openGuiScreen(this.escapeFallback); //Saves us needing a new field since we dont use base.escapeFallback
                ve.worldObj = null;
                ve.player = null;
                ve.isPaused = false;
                Time.timeScale = 1;
                ve.showDebugText = false;
                ve.isDeveloperMode = false;
            }
        }
    }
}
