using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.GUI {

    public class GuiScreenWorldSelect : GuiScreen {

        public GameObject worldTilePrefab;
        public RectTransform worldTileWrapperObj;

        public Button loadButton;
        public Button renameButton;
        public Button deleteButton;

        private List<WorldData> cachedWorlds;
        private PlayWorldButton selectedWorld;

        public void Awake() {
            this.cachedWorlds = new List<WorldData>();
        }

        public new void OnEnable() {
            string[] folders = Directory.GetDirectories("saves/");
            foreach (string f in folders) {
                string name = f + "/world.bin";
                if (File.Exists(name)) {
                    WorldData data = (WorldData)SerializationHelper.deserialize(name);
                    data.loadWorldImage();
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
            this.selectedWorld = null;
            this.cachedWorlds.Clear();
            foreach (Transform t in this.worldTileWrapperObj) {
                GameObject.Destroy(t.gameObject);
            }
        }

        public void loadCallback() {
            if (this.selectedWorld != null) {
                Main.singleton.generateWorld(this.cachedWorlds[this.selectedWorld.index]);
            }
        }

        public void renameCallback() {

        }

        public void deleteCallback() {

        }

        public void newWorldCallback() {
            Main.singleton.generateWorld(new WorldData("world" + Random.Range(0, 1000000)));
        }

        //Used by PlayWorldButton
        public void selectWorldCallback(PlayWorldButton obj) {
            //reset color of previous
            if (this.selectedWorld != null) {
                this.selectedWorld.background.color = new Color(1, 1, 1, 0.5f);
            }

            this.selectedWorld = obj;
            this.selectedWorld.background.color = new Color(0, 0, 0, 0.5f);
            this.loadButton.interactable = true;
            this.renameButton.interactable = true;
            this.deleteButton.interactable = true;
        }
    }
}
