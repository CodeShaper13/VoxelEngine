using System;
using System.IO;
using UnityEngine.UI;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenDeleteWorld : GuiScreen {

        public WorldData worldData;
        public Text errorText;

        public override void onGuiClose() {
            this.errorText.gameObject.SetActive(false);
        }

        public void deleteWorldCallback() {
            try {
                Directory.Delete("saves/" + this.worldData.worldName, true);
                this.openGuiScreen(GuiManager.worldSelect);
            } catch (Exception e) {
                this.errorText.gameObject.SetActive(true);
            }
        }

        public void callbackCancel() {
            this.openGuiScreen(GuiManager.worldSelect);
        }

        public override GuiScreen getEscapeCallback() {
            return GuiManager.worldSelect;
        }
    }
}
