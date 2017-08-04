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

        public void CALLBACK_deleteWorld() {
            try {
                Directory.Delete("saves/" + this.worldData.worldName, true);
                GuiManager.worldSelect.open();
            } catch (Exception e) {
                this.errorText.gameObject.SetActive(true);
            }

            this.playClickSound();
        }

        public void CALLBACK_cancel() {
            GuiManager.worldSelect.open();

            this.playClickSound();
        }

        public override GuiScreen getEscapeCallback() {
            return GuiManager.worldSelect;
        }
    }
}
