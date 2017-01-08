using fNbt;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenWorldSelect : GuiScreen {

        public GameObject worldTilePrefab;
        public RectTransform worldTileWrapperObj;

        public Button loadButton;
        public Button renameButton;
        public Button deleteButton;

        private List<WorldData> cachedWorlds;
        private string[] worldFolderNames;
        public PlayWorldButton selectedWorld;

        public void Awake() {
            this.cachedWorlds = new List<WorldData>();
        }

        public void OnEnable() {
            this.selectedWorld = null;
            this.cachedWorlds.Clear();

            this.worldFolderNames = Directory.GetDirectories("saves/");
            foreach (string f in this.worldFolderNames) {
                string name = f + "/world.nbt";
                if (File.Exists(name)) {
                    int index = f.LastIndexOf('/');
                    NbtFile file = new NbtFile();
                    file.LoadFromFile(name);
                    WorldData data = new WorldData(f.Substring(f.LastIndexOf('/') + 1, f.Length - 1 - index));
                    data.readFromNbt(file.RootTag);
                    this.cachedWorlds.Add(data);
                }
            }

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

        public void OnDisable() {
            foreach (Transform t in this.worldTileWrapperObj) {
                GameObject.Destroy(t.gameObject);
            }
            this.toggleButtons(false);
        }

        public void newWorldCallback(GuiScreenCreateWorld screen) {
            this.openGuiScreen(screen);
            screen.cachedWorlds = this.cachedWorlds;
        }

        public void loadCallback() {
            Main.singleton.generateWorld(this.cachedWorlds[this.selectedWorld.index]);
        }

        public void deleteWorldCallback(GuiScreenDeleteWorld screen) {
            this.openGuiScreen(screen);
            screen.worldData = this.cachedWorlds[this.selectedWorld.index];
        }

        public void renameWorldCallback(GuiScreenRenameWorld screen) {
            this.openGuiScreen(screen);
            screen.init(this.cachedWorlds[this.selectedWorld.index], this.cachedWorlds);
        }

        //Used by PlayWorldButton
        public void selectWorldCallback(PlayWorldButton obj) {
            //reset color of previous
            if (this.selectedWorld != null) {
                this.selectedWorld.background.color = new Color(1, 1, 1, 0.5f);
            }

            this.selectedWorld = obj;
            this.selectedWorld.background.color = new Color(0, 0, 0, 0.5f);
            this.toggleButtons(true);
        }

        private void toggleButtons(bool flag) {
            this.loadButton.interactable = flag;
            this.renameButton.interactable = flag;
            this.deleteButton.interactable = flag;
        }
    }
}