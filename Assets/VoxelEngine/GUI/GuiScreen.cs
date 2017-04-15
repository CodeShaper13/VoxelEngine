using UnityEngine;

namespace VoxelEngine.GUI {

    public class GuiScreen : MonoBehaviour {

        /// <summary>
        /// Opens a new GuiScreenand closes the current one.
        /// </summary>
        /// <param name="screen"></param>
        public void openGuiScreen(GuiScreen screen) {
            if (Main.singleton.currentGui != null) {
                // Hide the current gui screen, only if there is one.
                // In the event of pressing pause while playing, there is not current gui
                Main.singleton.currentGui.setVisible(false);
            }
            Main.singleton.currentGui = screen;
            Main.singleton.currentGui.setVisible(true);
        }

        private void OnEnable() {
            this.onGuiOpen();
        }

        private void OnDisable() {
            this.onGuiClose();
        }

        public virtual void onGuiOpen() { }

        public virtual void onGuiClose() { }

        /// <summary>
        /// Called when the escape key is pressed.
        /// </summary>
        public virtual void onEscape() {
            GuiScreen fallback = this.getEscapeCallback();
            if (fallback != null) {
                this.openGuiScreen(fallback);
            }
        }

        /// <summary>
        /// Enables or disables the gui.
        /// </summary>
        public void setVisible(bool flag) {
            //this.gameObject.GetComponent<Canvas>().enabled = flag;
            this.gameObject.SetActive(flag);
        }

        /// <summary>
        /// Getter for escapeCallback.  Some screens may want to vary what they fallback to, depending on where they were opened from.
        /// </summary>
        public virtual GuiScreen getEscapeCallback() {
            return null;
        }
    }
}
