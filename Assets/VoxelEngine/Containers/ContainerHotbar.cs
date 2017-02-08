using UnityEngine;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.GUI.Effect;

namespace VoxelEngine.Containers {

    public class ContainerHotbar : Container {

        public FadeText itemName;

        public override void initContainer(ContainerData data, EntityPlayer player) {
            base.initContainer(data, player);
            this.scroll(0);
        }

        public override void renderHeldItem() {
            //Override method but dont do anything so we dont draw a second held item, as this is always active
        }

        public void scroll(int scrollDirection) {
            ContainerDataHotbar cd = (ContainerDataHotbar)this.data;
            this.slots[cd.index].transform.localScale = Vector3.one;
            cd.index += scrollDirection * -1;
            if (cd.index > 8) {
                cd.index = 0;
            }
            if (cd.index < 0) {
                cd.index = 8;
            }
            this.slots[cd.index].transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);

            this.itemName.showAndStartFade(cd.getHeldItem() == null ? string.Empty : cd.getHeldItem().item.name, 1.5f);
        }

        public void setSelected(int index) {
            ContainerDataHotbar cd = (ContainerDataHotbar)this.data;
            this.slots[cd.index].transform.localScale = Vector3.one;
            cd.index = index;
            this.slots[cd.index].transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            this.showItemName();
        }

        public void showItemName() {
            ItemStack s = ((ContainerDataHotbar)this.data).getHeldItem();
            this.itemName.showAndStartFade(s == null ? string.Empty : s.item.name, 1.5f);
        }
    }
}
