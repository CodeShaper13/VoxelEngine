using UnityEngine;
using UnityEngine.UI;

namespace VoxelEngine.GUI {

    public class GuiScreenVersion : GuiScreen {
        private const string releaseDate = "XX/XX/XX";
        private const string supportEmail = "null";

        public Text versionText;

        public new void OnEnable() {
            this.versionText.text = "Voxel Engine\nVersion: " + Application.version + "\nRelease Data: " + GuiScreenVersion.releaseDate + "\nPlatform: " + Application.platform.ToString() + "\nUnity Version: " + Application.unityVersion + "\nModified Source: " + !Application.genuine + "\nSupport: " + GuiScreenVersion.supportEmail;
        }
    }
}
