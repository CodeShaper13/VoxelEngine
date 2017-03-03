namespace VoxelEngine.GUI {

    public class GuiScreenOptions : GuiScreen {

        public GuiScreen pauseScreen;

        public override GuiScreen onEscape(Main voxelEngine) {
            return this.goBack();
        }

        public void okCallback() {
            this.goBack();
        }

        public void cancelCallback() {
            this.goBack();
        }

        private GuiScreen goBack() {
            this.setActive(false);
            GuiScreen gui;
            if (Main.singleton.worldObj == null) {
                gui = this.escapeFallback; //Title
            } else {
                gui = this.pauseScreen;
            }
            gui.setActive(true);
            return gui;
        }
    }
}
