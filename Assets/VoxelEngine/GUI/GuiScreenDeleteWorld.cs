using System;
using System.IO;
using UnityEngine.UI;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenDeleteWorld : GuiScreen {

        public WorldData worldData;
        public Text errorText;

        public void OnDisable() {
            this.errorText.enabled = false;
        }

        public void deleteWorldCallback() {
            try {
                Directory.Delete("saves/" + this.worldData.worldName, true);
                this.openGuiScreen(this.escapeFallback);
            }
            catch (Exception e) {
                this.errorText.enabled = true;
            }
        }
    }
}
