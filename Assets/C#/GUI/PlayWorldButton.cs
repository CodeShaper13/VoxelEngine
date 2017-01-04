using UnityEngine;
using UnityEngine.UI;

public class PlayWorldButton : MonoBehaviour {
    public Image background;
    public Image worldImage;
    public Text worldInfo;
    public Button playButton;
    public int index;

    public void init(WorldData data, GuiScreenWorldSelect gsws, int buttonCallbackID) {
        Texture2D t = data.worldImage;
        if(t != null) {
            int smallDim = t.width > t.height ? t.height : t.width;
            Sprite sprite = Sprite.Create(t, new Rect((t.width - t.height) / 2, 0, smallDim, smallDim), Vector2.one);
            this.worldImage.sprite = sprite;
        }
        this.worldInfo.text = "Name: " + data.worldName + "\nLast Loaded: " + "null/null/null";
        this.playButton.onClick.AddListener(() => { gsws.selectWorldCallback(this); });
        this.index = buttonCallbackID;
    }
}
