using System.IO;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Level;

namespace VoxelEngine.GUI {

    public class PlayWorldButton : MonoBehaviour {
        public Image background;
        public Image worldImage;
        public Text worldInfo;
        public Button playButton;
        public int index;

        public void init(WorldData data, GuiScreenWorldSelect gsws, int buttonCallbackID) {
            Texture2D t = this.loadWorldImage(data.worldName);
            if (t != null) {
                int smallDim = t.width > t.height ? t.height : t.width;
                Sprite sprite = Sprite.Create(t, new Rect((t.width - t.height) / 2, 0, smallDim, smallDim), Vector2.one);
                this.worldImage.sprite = sprite;
            }
            this.worldInfo.text = "Name: " + data.worldName + "\nLast Loaded: " + data.lastLoaded.ToString();
            this.playButton.onClick.AddListener(() => { gsws.selectWorldCallback(this); });
            this.index = buttonCallbackID;
        }

        private Texture2D loadWorldImage(string worldName) {
            string name = "saves/" + worldName + "/worldImage.png";
            if (File.Exists(name)) {
                byte[] fileData = File.ReadAllBytes(name);
                Texture2D image = new Texture2D(2, 2);
                image.LoadImage(fileData);
                return image;
            }
            return null;
        }
    }
}
