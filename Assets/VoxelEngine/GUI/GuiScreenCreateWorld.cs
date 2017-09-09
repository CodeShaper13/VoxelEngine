using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Generation;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenCreateWorld : GuiScreen {

        public static string regexWorldName = @"[^a-zA-Z0-9_!]";

        public InputField fieldName;
        public InputField fieldSeed;
        public Text text;
        public Button buttonCreate;
        public Text buttonToggleTypeText;
        private int typeIndex;
        private List<WorldData> cachedWorlds;

        public override void onGuiOpen() {
            this.buttonCreate.interactable = false;
            this.setWorldTypeBtnText();
            this.cachedWorlds = GuiScreenWorldSelect.getSavedWorldData();
        }

        public override void onGuiClose() {
            this.fieldName.text = string.Empty;
            this.fieldSeed.text = string.Empty;
        }

        public void CALLBACK_createWorld() {
            string s = this.fieldSeed.text;
            Main.singleton.generateWorld(new WorldData(fieldName.text, s.Length > 0 ? int.Parse(s) : (int)DateTime.Now.ToBinary(), this.typeIndex, true));

            this.playClickSound();
        }

        public void CALLBACK_toggleWorldType() {
            do {
                this.typeIndex += 1;
                if (this.typeIndex >= WorldType.typeList.Count) {
                    this.typeIndex = 0;
                }
            } while (WorldType.typeList[this.typeIndex].isHidden == true);

            this.setWorldTypeBtnText();

            this.playClickSound();
        }

        public void CALLBACK_characterChange() {
            this.fieldName.text = Regex.Replace(this.fieldName.text, GuiScreenCreateWorld.regexWorldName, "");
            bool validName = this.fieldName.text != string.Empty;
            foreach (WorldData d in this.cachedWorlds) {
                if (d.worldName == this.fieldName.text) {
                    validName = false;
                    break;
                }
            }
            this.buttonCreate.interactable = validName;
            this.text.text = validName ? string.Empty : "Pick a unique world name";

            this.playClickSound();
        }

        private void setWorldTypeBtnText() {
            this.buttonToggleTypeText.text = "Type: " + WorldType.typeList[this.typeIndex].name;
        }

        public override GuiScreen getEscapeCallback() {
            return GuiManager.title;
        }
    }
}
