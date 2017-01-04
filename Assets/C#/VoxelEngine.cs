using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class VoxelEngine : MonoBehaviour {
    public static VoxelEngine singleton;

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

    public void Awake() {
        VoxelEngine.singleton = this;

        Item.initBlockItems();

        //this.generateWorld();
    }

    public void Update() {
        if(this.worldObj != null) {
            if (Input.GetKeyDown(KeyCode.F1)) {
                this.showDebugText = !this.showDebugText;
            }
            if(Input.GetKeyDown(KeyCode.F2)) {
                ScreenshotHelper.captureScreenshot();
            }
            if (Input.GetKeyDown(KeyCode.F3)) {
                this.showDebugText = !this.showDebugText;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if(this.player.containerElement != null) {
                    this.player.closeContainer();
                } else {
                    if (!this.isPaused) {
                        this.isPaused = true;
                        this.openGuiScreen(this.pauseScreen);
                    }
                    else {
                        this.currentGui = this.currentGui.onEscape(this);
                    }
                    VoxelEngine.setMouseLock(!this.isPaused);
                }
            }

            if(!this.isPaused) {
                this.worldObj.runWorldUpdate();
                this.player.handleInput();
            }

            //Draw the open container and hud
            if (this.player.containerElement != null) {
                this.player.containerElement.drawnContents();
            }
            this.player.containerHotbar.drawnContents();

            //Update debug text
            if(this.showDebugText) {
                this.textDebug.text = this.getDebugText();
            } else {
                this.textDebug.text = string.Empty;
            }
        } else {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                this.currentGui = this.currentGui.onEscape(this);
            }
        }
    }

    //Button callbacks
    public void buttonCallback(GuiScreen newScreen) {
        this.openGuiScreen(newScreen);
    }

    public void openGuiScreen(GuiScreen screen) {
        if(this.currentGui != null) {
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
        this.player = (EntityPlayer)this.worldObj.spawnEntity(EntityManager.singleton.playerPrefab, this.worldObj.generator.getSpawnPoint(), Quaternion.identity);

        this.currentGui.setActive(false);
        this.currentGui = null;

        VoxelEngine.setMouseLock(true);
    }

    public static Material getMaterial(int id) {
        return id < 256 ? VoxelEngine.singleton.blockMaterial : VoxelEngine.singleton.itemMaterial;
    }

    public static void setMouseLock(bool flag) {
        Cursor.visible = !flag;
        Cursor.lockState = flag ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
