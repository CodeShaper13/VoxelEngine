using UnityEngine;

namespace VoxelEngine.GUI {

    public class GuiManager : MonoBehaviour {

        public static GuiScreen title;
        public static GuiScreen options;
        public static GuiScreen changeLog;
        public static GuiScreen version;
        public static GuiScreen paused;
        public static GuiScreen worldSelect;
        public static GuiScreenCreateWorld createWorld;
        public static GuiScreenRenameWorld renameWorld;
        public static GuiScreenDeleteWorld deleteWorld;
        public static GuiScreenRespawn respawn;

        // References to game objects
        public GameObject titleObject;
        public GameObject optionsObject;
        public GameObject changeLogObject;
        public GameObject versionObject;
        public GameObject pausedObject;
        public GameObject worldSelectObject;
        public GameObject newWorldObject;
        public GameObject renameWorldObject;
        public GameObject deleteWorldObject;
        public GameObject respawnObject;

        public void Awake() {
            GuiManager.title = this.titleObject.GetComponent<GuiScreen>();
            GuiManager.options = this.optionsObject.GetComponent<GuiScreen>();
            GuiManager.changeLog = this.changeLogObject.GetComponent<GuiScreen>();
            GuiManager.version = this.versionObject.GetComponent<GuiScreen>();
            GuiManager.paused = this.pausedObject.GetComponent<GuiScreen>();
            GuiManager.worldSelect = this.worldSelectObject.GetComponent<GuiScreen>();
            GuiManager.createWorld = (GuiScreenCreateWorld) this.newWorldObject.GetComponent<GuiScreen>();
            GuiManager.renameWorld = (GuiScreenRenameWorld) this.renameWorldObject.GetComponent<GuiScreen>();
            GuiManager.deleteWorld = (GuiScreenDeleteWorld) this.deleteWorldObject.GetComponent<GuiScreen>();
            GuiManager.respawn = (GuiScreenRespawn) this.respawnObject.GetComponent<GuiScreen>();
        }
    }
}
