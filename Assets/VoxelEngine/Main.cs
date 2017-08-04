using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Command;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Entities.Registry;
using VoxelEngine.Generation;
using VoxelEngine.GUI;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine {

    public class Main : MonoBehaviour {

        public static Main singleton;

        [HideInInspector]
        public bool isDeveloperMode;
        [HideInInspector]
        public bool showDebugText = false;
        [HideInInspector]
        public bool isPaused;

        [HideInInspector]
        public World worldObj;
        [HideInInspector]
        public EntityPlayer player;

        public Text textDebug;
        public Transform textWindowRoot;

        public TextWindow textWindow;
        public CommandManager commandManager;
        public ContainerManager containerManager;
        private FpsCounter fpsCounter;

        private void Awake() {
            Main.singleton = this;

            // Make sure the singleton reference is set.
            this.GetComponent<References>().loadResources();

            this.textWindow = new TextWindow(this.textWindowRoot);
            new EntityRegistry().registerEntities();
            Item.initBlockItems();
            new RenderManager();

            this.commandManager = new CommandManager();

            this.fpsCounter = new FpsCounter();
        }

        private void Start() {
            this.containerManager = new ContainerManager();

            if(0 == 1) { // Debug instant load
                this.createNewWorld(false); // When false, the world is not saved.
            } else {
                GuiManager.title.open();
            }
        }

        private void Update() {
            if (this.worldObj != null && this.player != null) {
                // Playing the game.

                if (Input.GetKeyDown(KeyCode.F1)) {
                    this.isDeveloperMode = !this.isDeveloperMode;
                    this.textWindow.logMessage("Developer Mode is now " + (this.isDeveloperMode ? "ON" : "OFF"));
                }
                if (Input.GetKeyDown(KeyCode.F2)) {
                    ScreenshotHelper.captureScreenshot();
                    this.textWindow.logMessage("Captured screenshot");
                }
                if (Input.GetKeyDown(KeyCode.F3)) {
                    this.showDebugText = !this.showDebugText;
                    if(!this.showDebugText) {
                        this.textDebug.text = string.Empty;
                    }
                }
                if(Input.GetKeyDown(KeyCode.Slash)) {
                    if(!this.containerManager.isContainerOpen() && GuiManager.currentGui == null && !this.textWindow.isOpen) {
                        this.textWindow.openWindow();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    if (this.textWindow.isOpen) {
                        this.textWindow.closeWindow();
                    }
                    else if (this.containerManager.isContainerOpen()) {
                        this.containerManager.closeContainer(this.player);
                    }
                    else if(GuiManager.currentGui != null) {
                        GuiManager.currentGui.onEscape();
                    }
                    else if(!this.isPaused) {
                        this.pauseGame();
                    }
                }

                if (!this.isPaused && player.health > 0 && !this.containerManager.isContainerOpen() && !this.textWindow.isOpen) {
                    this.player.handleInput();
                }

                this.containerManager.drawContainerContents();

                this.fpsCounter.updateCounter();

                //Update debug text.
                if (this.showDebugText) {
                    this.textDebug.text = this.getDebugText();
                }

                // Draw held item.
                ItemStack stack = this.player.containerHotbar.getHeldItem();
                if (stack != null) {
                    stack.item.renderAsHeldItem(stack.meta, stack.count, this.player.handTransfrom);
                }
            } else {
                // Menu screens.
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    GuiScreen fallback = GuiManager.currentGui.getEscapeCallback();
                    if(fallback != null) { // Title screen returns null, can't go back any more!
                        fallback.open();
                    }
                }
            }
        }

        public void createNewWorld(bool flag = true) {
            string name = "World_1";
            Main.singleton.generateWorld(new WorldData(name, 2346347 /*new System.Random().Next()*/, WorldType.CAVE.id, flag));
        }

        /// <summary>
        /// Pauses the game and handles the blocking of input.
        /// </summary>
        public void pauseGame() {
            this.isPaused = true;
            Main.hideMouse(false);
            Time.timeScale = 0;
            GuiManager.paused.open();
            SoundManager.setUsePlayer(false);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void resumeGame() {
            GuiManager.currentGui = null;
            this.isPaused = false;
            Main.hideMouse(true);
            Time.timeScale = 1;
            SoundManager.setUsePlayer(true);
        }

        /// <summary>
        /// Returns the debug text to display.
        /// </summary>
        public string getDebugText() {
            StringBuilder s = new StringBuilder();
            s.Append("FPS: " + this.fpsCounter.currentFps);
            s.Append("\nPlayer Position: " + this.transform.position.ToString());
            s.Append("\nPlayer Rotation: " + this.transform.eulerAngles.ToString());
            BlockPos p = this.player.posLookingAt;
            int meta = this.worldObj.getMeta(p);
            s.Append("\nLooking At: " + this.worldObj.getBlock(p).getName(meta) + ":" + meta + " " + p.ToString());
            s.Append("\nLooking at Light: " + this.worldObj.getLight(p.x, p.y, p.z));
            s.Append("\nPress F3 to toggle debug info");
            return s.ToString();
        }

        /// <summary>
        /// Creates a new world and loads it.
        /// </summary>
        public void generateWorld(WorldData data) {
            this.worldObj = GameObject.Instantiate(References.list.worldPrefab).GetComponent<World>();
            this.worldObj.initWorld(data);

            if(GuiManager.currentGui != null) {
                GuiManager.currentGui.setVisible(false);
            }
            GuiManager.currentGui = null;
            this.player = this.worldObj.spawnPlayer(EntityRegistry.player.getPrefab());
            Main.hideMouse(true);
        }

        /// <summary>
        /// Toggles the mouse visibility.  True means it is hidden and can't move.
        /// </summary>
        public static void hideMouse(bool flag) {
            Cursor.visible = !flag;
            Cursor.lockState = flag ? CursorLockMode.Locked : CursorLockMode.None;
        }

        /// <summary>
        /// Used by the text window input field.
        /// </summary>
        public void callbackTextWindowChange(string text) {
            //TODO tab auto complete.
        }

        public void callbackTextWindowEnter(string text) {
            this.textWindow.onEnter(text);
        }
    }
}
