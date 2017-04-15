namespace VoxelEngine.GUI {

    public class GuiScreenChangeLog : GuiScreen {

        public override GuiScreen getEscapeCallback() {
            return GuiManager.title;
        }
    }
}
