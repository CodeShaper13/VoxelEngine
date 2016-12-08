using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory {

    public Transform[] inventoryObj;
    public ItemStack[] hotbar;
    public int index;

    private Text[] itemCount;
    private Camera hudCamera;

	public PlayerInventory(Camera hudCamera) {
        this.hudCamera = hudCamera;

        this.inventoryObj = new Transform[9];
        this.itemCount = new Text[9];
        for(int i = 0; i < 9; i++) {
            this.inventoryObj[i] = GameObject.Find("HotbarSlot" + (i + 1)).transform;
            this.itemCount[i] = this.inventoryObj[i].GetChild(0).GetComponent<Text>();
        }
        this.hotbar = new ItemStack[9];
    }

    public void drawHotbar() {
        for(int i = 0; i < 9; i++) {
            ItemStack stack = this.hotbar[i];
            if(stack != null) {
                this.renderItem(stack, this.inventoryObj[i].transform);
            }
            this.itemCount[i].text = (stack == null ? string.Empty : stack.count.ToString());
        }
    }

    public ItemStack addItemStack(ItemStack newStack) {
        for(int i = 0; i < 9; i++) {
            if(this.hotbar[i] == null) {
                this.hotbar[i] = newStack;
                return null;
            }
            ItemStack leftover = this.hotbar[i].merge(newStack);
            if(leftover == null || leftover.count == 0) {
                return null;
            }
        }
        return newStack;
    }

    public ItemStack dropItem(int i, bool wholeStack) {
        if(this.hotbar[i] != null) {
            ItemStack s = new ItemStack(this.hotbar[i].item, this.hotbar[i].meta, wholeStack ? this.hotbar[i].count : 1);
            this.hotbar[i].count -= wholeStack ? this.hotbar[i].count : 1;
            if(this.hotbar[i].count <= 0) {
                this.hotbar[i] = null;
            }
            return s;
        }
        return null;
    }

    public void scroll(int i) {
        this.inventoryObj[this.index].localScale = Vector3.one;
        this.index += i;
        if(this.index > 8) {
            this.index = 0;
        }
        if(this.index < 0) {
            this.index = 8;
        }
        this.inventoryObj[this.index].localScale = new Vector3(1.15f, 1.15f, 1.15f);
    }

    private void renderItem(ItemStack stack, Transform t) {
        IRenderItem r = stack.item.itemRenderer;
        MeshData meshData = r.renderItem(stack);
        Graphics.DrawMesh(meshData.toMesh(), r.getMatrix(t), stack.item.id < 256 ? Constants.instance.blockMaterial : Constants.instance.itemMaterial, 8, null, 0, null, false, false);
    }
}
