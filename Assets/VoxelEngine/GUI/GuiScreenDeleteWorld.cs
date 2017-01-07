using System.IO;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class GuiScreenDeleteWorld : GuiScreen {

        public WorldData worldData;

        public void deleteWorldCallback() {
            Directory.Delete("saves/" + this.worldData.worldName, true);
            this.openGuiScreen(this.escapeFallback);
        }
    }
}
