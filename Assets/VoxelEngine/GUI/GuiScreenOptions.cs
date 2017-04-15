namespace VoxelEngine.GUI {

    public class GuiScreenOptions : GuiScreen {

        public void okCallback() {
            // Save options...

            this.onEscape();
        }

        public void cancelCallback() {
            // Revert to previous options

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
