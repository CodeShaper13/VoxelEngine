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
        public GuiScreen waitingScreen;
        public GuiScreen currentGui;

        public float averageChunkBakeTime;

        public void Awake() {
            //Make sure the singleton reference is set
            this.GetComponent<References>().initReferences();

            Main.singleton = this;

            Item.initBlockItems();
        }

        public void Start() {
            //Debug instant world generation
            //this.generateWorld(new WorldData("world" + UnityEngine.Random.Range(int.MinValue, int.MaxValue), (int)DateTime.Now.ToBinary(), 0, true));
        }

        public void Update() {
            //if(this.data != null) {
            //    this.worldObj = GameObject.Instantiate(this.worldPrefab).GetComponent<World>();
            //    this.worldObj.initWorld(data);
            //    this.data = null;
            //}

            if (this.worldObj != null && this.player != null) {
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
                    } else {
                        if (!this.isPaused) {
                            this.isPaused = true;
                            this.openGuiScreen(this.pauseScreen);
                            this.player.fpc.enabled = false;
                        }
                        else {
                            this.currentGui = this.currentGui.onEscape(this);
                        }
                        Main.setMouseLock(!this.isPaused);
                        this.player.fpc.enabled = !this.isPaused;
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
            //this.openGuiScreen(this.waitingScreen);
            //this.data = data;
            this.worldObj = GameObject.Instantiate(References.list.worldPrefab).GetComponent<World>();
            this.worldObj.initWorld(data);

            this.currentGui.setActive(false);
            this.currentGui = null;
            this.player = this.worldObj.spawnPlayer(EntityList.singleton.playerPrefab);
            Main.setMouseLock(true);
        }

        //public void onWorldLoadFinish() {
        //    this.currentGui.setActive(false);
        //    this.currentGui = null;
        //    this.player = this.worldObj.spawnPlayer(EntityList.singleton.playerPrefab);
        //    Main.setMouseLock(true);
        //}

        public static void setMouseLock(bool flag) {
            Cursor.visible = !flag;
            Cursor.lockState = flag ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
