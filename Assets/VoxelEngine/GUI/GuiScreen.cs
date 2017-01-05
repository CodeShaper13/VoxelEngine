using UnityEngine;

namespace VoxelEngine.GUI {

    public class GuiScreen : MonoBehaviour {

        public GuiScreen escapeFallback;

        public void OnEnable() {

        }

        //Called when the escape key is pressed and this is the active gui
        public virtual GuiScreen onEscape(Main voxelEngine) {
            if (this.escapeFallback != null) {
                this.setActive(false); //hide this screen
                this.escapeFallback.setActive(true); //show the current
                return this.escapeFallback; //return a reference to the current screen
            }
            return this;
        }

        public void setActive(bool flag) {
            this.gameObject.SetActive(flag);
        }
    }
}
