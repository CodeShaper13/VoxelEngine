using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Command;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Entities.Registry;
using VoxelEngine.GUI;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine {

    public class Main : MonoBehaviour {

        public const string VERSION = "Pre-Alpha";

        public static Main singleton;
        public static bool isDeveloperMode = false;

        [HideInInspector]
        public bool showDebugText = false;
        [HideInInspector]
        public bool isPaused;

        [HideInInspector]
        public World worldObj;
        [HideInInspector]
        public EntityPlayer player;

        [Header("0 = Normal, >0 = instant load world, -1 = Init only")]
        public int launchFlag = 0;

        public Text textDebug;
        public Transform textWindowRoot;

        public TextWindow textWindow;
        public CommandManager commandManager;
        public ContainerManager containerManager;
        private FpsCounter fpsCounter;

        private void Awake() {
            Main.singleton = this;

            References.list = this.GetComponent<References>();

            Options.loadInitialOptions();

            this.textWindow = new TextWindow(this.textWindowRoot);
            new EntityRegistry().registerEntities();
            Item.initBlockItems();
            new RenderManager();
            //new ObjectData();

            this.commandManager = new CommandManager();
            this.fpsCounter = new FpsCounter();
        }

        private void Start() {
            if(this.launchFlag != -1) {
                this.containerManager = new ContainerManager();
            }

            if(this.launchFlag == 0) {
                GuiManager.title.open();
            } else if(this.launchFlag > 0) { // Debug instant load
                this.createNewWorld(false); // When false, the world is not saved.
            } else {
                MeshBuilder.FLAG = true;

                MeshBuilder mb = RenderManager.getMeshBuilder();

                for(int x = -1; x < 2; x++) {
                    for (int y = -1; y < 2; y++) {
                        for (int z = -1; z < 2; z++) {
                            mb.setLightLevel(x, y, z, 5 + 1 + x);
                        }
                    }
                }
                // print(mb.getLightLevel(-1, 0, 0));
                // print(mb.getLightLevel(0, 0, 0));
                // print(mb.getLightLevel(1, 0, 0));
                // Light levels
                // +---------> X
                // | 5, 6, 7

                print(mb.sampleAxis(0, EnumAxis.X));
                print(mb.sampleAxis(0.5f, EnumAxis.X));
                print(mb.sampleAxis(-0.5f, EnumAxis.X));


                float i = mb.sampleAxis(0.5f, EnumAxis.X);
                float j = mb.sampleAxis(0.0f, EnumAxis.Y);
                float k = mb.sampleAxis(0.0f, EnumAxis.Z);
                print(i);
                print(j);
                print(k);
                print("Result: " + (i + j + k) / 3);

                LightColors lc = RenderManager.instance.lightColors;

                /*
                Color c = mb.calculateLightForVertex(new Vector3(0.5f, 0, 0), new BlockPos(0, 0, 0));
                Color l6 = lc.getColorFromBrightness(6);
                Color l7 = lc.getColorFromBrightness(7);
                print("6: " + l6);
                print("/  " + c);
                print("\\  "+ (l6 + l7) / 2);
                print("7: " + l7);
                */
            }
        }

        private void Update() {
            if (this.worldObj != null && this.player != null) {
                // Playing the game.

                // Debug keys.
                if (Input.GetKeyDown(KeyCode.F1)) {
                    Main.isDeveloperMode = !Main.isDeveloperMode;
                    this.textWindow.logMessage("Developer Mode is now " + (Main.isDeveloperMode ? "ON" : "OFF"));
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
                if(Input.GetKeyDown(KeyCode.F4)) {
                    this.worldObj.rebakeWorld();
                }
                if (Input.GetKeyDown(KeyCode.F5)) {
                    RenderManager.instance.lightColors.toggleUseDebugColors();
                    this.worldObj.rebakeWorld();
                }
                if (Input.GetKeyDown(KeyCode.F6)) {
                    RenderManager.instance.useSmoothLighting = !RenderManager.instance.useSmoothLighting;
                    this.worldObj.rebakeWorld();
                }
                if (Input.GetKeyDown(KeyCode.F7)) {
                    RenderManager.instance.preRenderItems();
                }

                // Command key.
                if (Input.GetKeyDown(KeyCode.Slash)) {
                    if(!this.containerManager.isContainerOpen() && GuiManager.currentGui == null && !this.textWindow.isOpen) {
                        this.textWindow.openWindow();
                    }
                }

                // Handle use of escape.
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

                 // 'e'.
                if(Input.GetKeyDown(KeyCode.E)) {
                    if(this.containerManager.isContainerOpen()) {
                        this.containerManager.closeContainer(this.player);
                    } else {
                        this.containerManager.openContainer(this.player, ContainerManager.containerInventory, this.player.dataInventory);
                    }
                }

                // Player input.
                if (!this.isPaused && player.health > 0 && !this.containerManager.isContainerOpen() && !this.textWindow.isOpen) {
                    this.player.handleInput();

#if UNITY_EDITOR
                    Cursor.lockState = CursorLockMode.Locked;
#endif
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
                    stack.item.renderAsHeldItem(this.player, stack.meta, stack.count, this.player.handTransfrom);
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

        public void createNewWorld(bool worldIsSavable) {
            string name = "World_1";
            Main.singleton.generateWorld(new WorldData(name, 2346347, this.launchFlag, worldIsSavable));
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
            s.Append("\nPlayer Position: " + this.player.transform.position.ToString());
            s.Append("\nPlayer Rotation: " + this.player.transform.eulerAngles.ToString());
            if(this.player.posLookingAt != null) {
                BlockPos p = (BlockPos)this.player.posLookingAt;
                int meta = this.worldObj.getMeta(p);
                s.Append("\nLooking At Light: " + this.worldObj.getLight(p.x, p.y, p.z));
                s.Append("\nLooking At Pos:   " + p.ToString());
                s.Append("\nLooking At State: " + this.worldObj.getBlock(p).getAsDebugText(meta));
            }
            s.Append("\nPress F3 to toggle debug info");
            return s.ToString();
        }

        /// <summary>
        /// Creates a new world and loads it.
        /// </summary>
        public void generateWorld(WorldData data) {
            this.worldObj = new GameObject().AddComponent<World>();
            this.worldObj.initWorld(data);

            if(GuiManager.currentGui != null) {
                GuiManager.currentGui.setVisible(false);
            }
            GuiManager.currentGui = null;
            this.player = this.worldObj.spawnPlayer();
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

        // TODO fix bug:
        // This is called whenever the test field loses focus, like when escape is pressed.
        public void callbackTextWindowEnter(string text) {
            if(!string.IsNullOrEmpty(text)) {
                this.textWindow.onEnter(text);
            }
        }
    }
}
