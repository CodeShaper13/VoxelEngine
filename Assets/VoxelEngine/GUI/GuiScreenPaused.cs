using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenPaused : GuiScreen {

        public override GuiScreen onEscape(Main voxelEngine) {
            this.setActive(false);
            voxelEngine.isPaused = false;
            return null;
        }

        public void saveCallback(bool exitWorld) {
            World world = Main.singleton.worldObj;
            world.saveEntireWorld(exitWorld);
            if (exitWorld) {
                Main ve = Main.singleton;
                ve.player.cleanupObject();
                GameObject.Destroy(world.gameObject);
                this.openGuiScreen(this.escapeFallback); //Saves us needing a new field since we dont use base.escapeFallback
                ve.worldObj = null;
                ve.player = null;
                ve.isPaused = false;
                ve.showDebugText = false;
                ve.isDeveloperMode = false;
            }
        }
    }
}
