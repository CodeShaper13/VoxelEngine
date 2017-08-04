namespace VoxelEngine.GUI {

    public class GuiScreenOptions : GuiScreen {

        public void CALLBACK_ok() {
            // Save options...
            this.playClickSound();
            this.onEscape();
        }

        public void CALLBACK_cancel() {
            // Revert to previous options
            this.playClickSound();
            this.onEscape();
        }

        public override GuiScreen getEscapeCallback() {
            if (Main.singleton.worldObj == null) {
                return GuiManager.title;
            } else {
                return GuiManager.paused;
            }
        }
    }
}
