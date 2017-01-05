namespace VoxelEngine.GUI {

    public class GuiScreenOptions : GuiScreen {

        public GuiScreen pauseScreen;

        public override GuiScreen onEscape(Main voxelEngine) {
            this.setActive(false);
            GuiScreen gui;
            if (voxelEngine.worldObj == null) {
                gui = this.escapeFallback; //Title
            }
            else {
                gui = this.pauseScreen;
            }
            gui.setActive(true);
            return gui;
        }
    }
}
