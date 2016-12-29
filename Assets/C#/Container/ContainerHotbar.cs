using UnityEngine;
using UnityEngine.UI;

public class ContainerHotbar : Container {

    public Text itemName;

    public override void initContainer(ContainerData data, EntityPlayer player) {
        base.initContainer(data, player);
        this.scroll(0);
    }

    public override void renderHeldItem() {
        
    }

    public void scroll(int i) {
        ContainerDataHotbar cd = (ContainerDataHotbar)this.data;
        this.slots[cd.index].transform.localScale = Vector3.one;
        cd.index += i * -1;
        if (cd.index > 8) {
            cd.index = 0;
        }
        if (cd.index < 0) {
            cd.index = 8;
        }
        this.slots[cd.index].transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);

        this.itemName.text = cd.getHeldItem() == null ? string.Empty : cd.getHeldItem().item.name;
    }
}
