using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenRenameWorld : GuiScreen {

        public InputField field;
        public Button button;
        public Text textErrorMsg;
        private WorldData targetWorldData;
        private List<WorldData> cachedWorlds;

        public void set(WorldData data, List<WorldData> cached) {
            this.targetWorldData = data;
            this.cachedWorlds = cached;
            this.field.text = this.targetWorldData.worldName;
        }

        public void CALLBACK_renameWorld() {
            string newName = this.field.text;
            string oldName = this.targetWorldData.worldName;
            if (newName != oldName) {
                Directory.Move("saves/" + oldName, "saves/" + newName);
                this.targetWorldData.worldName = newName;
            }

            GuiManager.worldSelect.open();
            this.playClickSound();
        }

        public void CALLBACK_characterChange() {
            this.field.text = Regex.Replace(this.field.text, GuiScreenCreateWorld.regexWorldName, "");
            bool validName = true;
            foreach(WorldData d in this.cachedWorlds) {
                if(d.worldName == this.field.text) {
                    validName = false;
                    break;
                }
            }
            bool flag = validName || this.field.text == this.targetWorldData.worldName;
            this.button.interactable = flag;
            this.textErrorMsg.text = flag ? string.Empty : "You may not have duplicate world names, pick a different one";
        }

        public override GuiScreen getEscapeCallback() {
            return GuiManager.worldSelect;
        }
    }
}
