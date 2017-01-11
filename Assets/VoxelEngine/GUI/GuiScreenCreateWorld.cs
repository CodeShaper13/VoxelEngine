using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenCreateWorld : GuiScreen {
        public static string regexWorldName = @"[^a-zA-Z0-9_!]";

        public InputField fieldName;
        public InputField fieldSeed;
        public Text text;
        public Button buttonCreate;
        public List<WorldData> cachedWorlds;

        public void OnEnable() {
            this.buttonCreate.interactable = false;
        }

        public void OnDisable() {
            this.fieldName.text = string.Empty;
            this.fieldSeed.text = string.Empty;
        }

        public void createWorldCallback() {
            string s = this.fieldSeed.text;
            Main.singleton.generateWorld(new WorldData(fieldName.text, s.Length > 0 ? int.Parse(s) : (int)DateTime.Now.ToBinary(), false));
        }

        public void characterChangeCallback() {
            this.fieldName.text = Regex.Replace(this.fieldName.text, GuiScreenCreateWorld.regexWorldName, "");
            bool validName = true;
            foreach (WorldData d in this.cachedWorlds) {
                if (d.worldName == this.fieldName.text) {
                    validName = false;
                    break;
                }
            }
            this.buttonCreate.interactable = validName;
            this.text.text = validName ? string.Empty : "Pick a unique world name";
        }
    }
}
