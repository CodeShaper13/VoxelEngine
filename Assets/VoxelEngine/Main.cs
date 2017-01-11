using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Entities;
using VoxelEngine.GUI;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine {

    public class Main : MonoBehaviour {
        public static Main singleton;

        public bool isDeveloperMode;
        public bool showDebugText;
        public bool isPaused;

        public World worldObj;
        public EntityPlayer player;

        public Text textDebug;

        public GuiScreen pauseScreen;
        public GuiScreen currentGui;

        public Material blockMaterial;
        public Material itemMaterial;
        public GameObject worldPrefab;

        public float averageChunkBakeTime;

        public void Awake() {
            Main.singleton = this;

            Item.initBlockItems();


            //System.Random r = new System.Random();
            //Stopwatch s1 = new Stopwatch();
            //s1.Start();
            //for (int i = 0; i < 100; i++) {
            //    int j = i;
            //    r.Next(0, 100);
            //}
            //print("Random S" + s1.Elapsed);

            //Stopwatch s = new Stopwatch();
            //s.Start();
            //for(int i = 0; i < 100; i++) {
            //    int j = i;
            //    UnityEngine.Random.Range(0, 100);
            //}
            //print("Random U" + s.Elapsed);

            //Debug instant world generation
            this.generateWorld(new WorldData("world" + UnityEngine.Random.Range(int.MinValue, int.MaxValue), (int)DateTime.Now.ToBinary(), true));
        }

        public void Update() {
            if (this.worldObj != null) {
                if (Input.GetKeyDown(KeyCode.F1)) {
                    this.showDebugText = !this.showDebugText;
                }
                if (Input.GetKeyDown(KeyCode.F2)) {
                    ScreenshotHelper.captureScreenshot();
                }
                if (Input.GetKeyDown(KeyCode.F3)) {
                    this.showDebugText = !this.showDebugText;
                }
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    if (this.player.containerElement != null) {
                        this.player.closeContainer();
                    }
                    else {
                        if (!this.isPaused) {
                            this.isPaused = true;
                            this.openGuiScreen(this.pauseScreen);
                            this.player.fpc.enabled = false;
                        }
                        else {
                            this.currentGui = this.currentGui.onEscape(this);
                            this.player.fpc.enabled = true;
                        }
                        Main.setMouseLock(!this.isPaused);
                    }
                }

                if (!this.isPaused) {
                    this.worldObj.runWorldUpdate();
                    this.player.onEntityUpdate();
                    this.player.handleInput();
                }

                //Draw the open container and hud
                if (this.player.containerElement != null) {
                    this.player.containerElement.drawnContents();
                }
                this.player.containerHotbar.drawnContents();

                //Update debug text
                if (this.showDebugText) {
                    this.textDebug.text = this.getDebugText();
                }
                else {
                    this.textDebug.text = string.Empty;
                }
            }
            else {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    this.currentGui = this.currentGui.onEscape(this);
                }
            }

            this.averageChunkBakeTime = Chunk.TOTAL_BAKED / Chunk.MIL;
        }

        [Obsolete("Use GuiScreen.openGuiScreen instead")]
        public void openGuiScreen(GuiScreen screen) {
            if (this.currentGui != null) {
                //Only the pause screen will not trigger this, as there is no screen before it
                this.currentGui.setActive(false);
            }

            this.currentGui = screen;
            this.currentGui.setActive(true);
        }

        public string getDebugText() {
            StringBuilder s = new StringBuilder();
            s.Append("Position: " + this.transform.position.ToString() + "\n");
            s.Append("Rotation: " + this.transform.eulerAngles.ToString() + "\n");
            BlockPos p = this.player.posLookingAt;
            s.Append("Looking At: " + this.worldObj.getBlock(p).name + ":" + this.worldObj.getMeta(p) + " " + p.ToString() + "\n");
            s.Append(this.worldObj.worldData.worldName + " " + this.worldObj.worldData.seed);
            return s.ToString();
        }

        public void generateWorld(WorldData data) {
            this.worldObj = GameObject.Instantiate(this.worldPrefab).GetComponent<World>();
            this.worldObj.initWorld(data);

            this.player = this.worldObj.spawnPlayer(EntityList.singleton.playerPrefab);

            this.currentGui.setActive(false);
            this.currentGui = null;

            Main.setMouseLock(true);
        }

        public static Material getMaterial(int id) {
            return id < 256 ? Main.singleton.blockMaterial : Main.singleton.itemMaterial;
        }

        public static void setMouseLock(bool flag) {
            Cursor.visible = !flag;
            Cursor.lockState = flag ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
