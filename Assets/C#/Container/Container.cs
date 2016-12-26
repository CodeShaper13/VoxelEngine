using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour {
    public ContainerData data;
    public Button[] slots;
    private Text[] slotText;

    //Acts like a constructor, but since this is a GameObject we can't have a normal one
    public virtual void initContainer(ContainerData data) {
        this.data = data;

        this.slotText = new Text[this.slots.Length];
        for(int i = 0; i < this.slots.Length; i++) {
            this.slotText[i] = this.slots[i].GetComponentInChildren<Text>();
        }

        Camera hudCamera = GameObject.Find("HudCamera").GetComponent<Camera>();
        this.transform.SetParent(hudCamera.transform);
        this.GetComponent<Canvas>().worldCamera = hudCamera;
    }

    public virtual void drawnContents() {
        for(int i = 0; i < this.slots.Length; i++) {
            Debug.DrawLine(this.slots[i].transform.position, Vector3.zero, Color.red);

            ItemStack stack = this.data.items[i];
            if(stack != null) {
                IRenderItem r = this.data.items[i].item.itemRenderer;
                Transform t = this.slots[i].transform;
                Material m = stack.item.id < 256 ? Constants.instance.blockMaterial : Constants.instance.itemMaterial;
                Graphics.DrawMesh(r.renderItem(stack).toMesh(), r.getMatrix(t), m, 8, null, 0, null, false, false);
            }
            this.slotText[i].text = (stack == null ? string.Empty : stack.count.ToString());
        }
    }
}
