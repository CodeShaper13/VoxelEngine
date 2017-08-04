using UnityEngine;

namespace VoxelEngine.GUI {

    public class GuiScreen : MonoBehaviour {

        /// <summary>
        /// Opens this gui, closing any previously open ones.
        /// </summary>
        public void open() {
            if (GuiManager.currentGui != null) {
                // Hide the current gui screen, only if there is one.
                // In the event of pressing pause while playing or when opening the
                // title screen there is no current gui.
                GuiManager.currentGui.setVisible(false);
            }
            GuiManager.currentGui = this;
            GuiManager.currentGui.setVisible(true);
        }

        private void OnEnable() {
            this.onGuiOpen();
        }

        private void OnDisable() {
            this.onGuiClose();
        }

        /// <summary>
        /// Called when the gui closes for any reason.
        /// </summary>
        public virtual void onGuiOpen() { }

        /// <summary>
        /// Called when the gui is opened.
        /// </summary>
        public virtual void onGuiClose() { }

        /// <summary>
        /// Called when the escape key is pressed.
        /// </summary>
        public virtual void onEscape() {
            GuiScreen fallback = this.getEscapeCallback();
            if (fallback != null) {
                fallback.open();
            }
        }

        /// <summary>
        /// Enables or disables the gui.
        /// </summary>
        public void setVisible(bool flag) {
            this.gameObject.SetActive(flag);
        }

        /// <summary>
        /// Returns the screen to go to when escape is pressed.
        /// </summary>
        public virtual GuiScreen getEscapeCallback() {
            return null;
        }

        /// <summary>
        /// Plays the sound effect when a button is clicked.
        /// </summary>
        public void playClickSound() {
            SoundManager sm = SoundManager.singleton;
            sm.getUiSource().PlayOneShot(sm.uiButtonClick);
        }

        public void CALLBACK_gotoGui(GuiScreen screen) {
            this.playClickSound();
            screen.open();
        }
    }
}
