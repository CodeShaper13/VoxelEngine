using fNbt;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenWorldSelect : GuiScreen {

        public RectTransform worldTileWrapperObj;

        public Button loadButton;
        public Button renameButton;
        public Button deleteButton;

        public GameObject worldTilePrefab;

        private List<WorldData> cachedWorlds;
        private PlayWorldButton selectedWorld;

        public GameObject test;

        public static List<WorldData> getSavedWorldData() {
            List<WorldData> worlds = new List<WorldData>();

            string[] names = Directory.GetDirectories("saves/");
            foreach (string folderName in names) {
                string dataFile = folderName + "/world.nbt";
                if (File.Exists(dataFile)) {
                    NbtFile file = new NbtFile();
                    file.LoadFromFile(dataFile);
                    WorldData data = new WorldData(folderName.Substring(folderName.LastIndexOf('/') + 1, folderName.Length - 1 - folderName.LastIndexOf('/')));
                    data.readFromNbt(file.RootTag);
                    worlds.Add(data);
                }
            }

            worlds.Sort((i2, i1) => DateTime.Compare(i1.lastLoaded, i2.lastLoaded));

            return worlds;
        }

        public override void onGuiOpen() {
            this.selectedWorld = null;
            this.cachedWorlds = GuiScreenWorldSelect.getSavedWorldData();

            int y = -70;
            int i;
            for (i = 0; i < this.cachedWorlds.Count; i++) {
                GameObject g = GameObject.Instantiate(this.worldTilePrefab);
                g.GetComponent<PlayWorldButton>().init(this.cachedWorlds[i], this, i);

                RectTransform rt = g.GetComponent<RectTransform>();
                rt.transform.SetParent(this.worldTileWrapperObj, true);
                rt.anchoredPosition = new Vector3(0, y, 0);
                rt.transform.localScale = Vector3.one;
                y -= 130;
            }

            this.worldTileWrapperObj.sizeDelta = new Vector2(this.worldTileWrapperObj.sizeDelta.x, (i * 130) + 10);
        }

        public override void onGuiClose() {
            foreach (Transform t in this.worldTileWrapperObj) {
                GameObject.Destroy(t.gameObject);
            }
            this.toggleButtons(false);
        }

        /*
        public void CALLBACK_newWorld() {
            GuiManager.createWorld.open();

            this.playClickSound();
        }
        */

        public void CALLBACK_loadWorld() {
            WorldData data = this.cachedWorlds[this.selectedWorld.index];
            Main.singleton.generateWorld(data);

            this.playClickSound();
        }

        public void CALLBACK_deleteWorld() {
            GuiManager.deleteWorld.open();
            GuiManager.deleteWorld.worldData = this.cachedWorlds[this.selectedWorld.index];

            this.playClickSound();
        }

        public void CALLBACK_renameWorld() {
            GuiManager.renameWorld.open();
            GuiManager.renameWorld.set(this.cachedWorlds[this.selectedWorld.index], this.cachedWorlds);

            this.playClickSound();
        }

        /// <summary>
        /// Called by PlayWorldButton.
        /// </summary>
        public void selectWorldCallback(PlayWorldButton obj) {
            // Reset color of previous selected.
            if (this.selectedWorld != null) {
                this.selectedWorld.background.color = new Color(1, 1, 1, 0.5f);
            }

            this.selectedWorld = obj;
            this.selectedWorld.background.color = new Color(0, 0, 0, 0.5f);
            this.toggleButtons(true);

            this.playClickSound();
        }

        /// <summary>
        /// Toggles if the buttons along the bottom are active.
        /// </summary>
        private void toggleButtons(bool flag) {
            this.loadButton.interactable = flag;
            this.renameButton.interactable = flag;
            this.deleteButton.interactable = flag;
        }

        public override GuiScreen getEscapeCallback() {
            return GuiManager.title;
        }
    }
}