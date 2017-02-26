using UnityEngine;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.GUI.Effect;

namespace VoxelEngine.Containers {

    public class ContainerHotbar : Container {

        public FadeText itemName;
        public int index;

        public override void onOpen(ContainerData data, EntityPlayer player) {
            base.onOpen(data, player);
            this.scroll(0);
        }
         // Scrolls the hotbar index in the passes direction
        public void scroll(int scrollDirection) {
            int newIndex = this.index + scrollDirection;
            if (newIndex > 8) {
                newIndex = 0;
            }
            if (newIndex < 0) {
                newIndex = 8;
            }
            this.setSelected(newIndex);
        }

        // Sets the passed index to be the selected one, updating slot sizes and held text
        public void setSelected(int index) {
            this.slots[this.index].transform.localScale = Vector3.one;
            this.index = index;
            this.slots[this.index].transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            this.updateHudItemName();
        }

        // Updates the hud name with the current held item
        public void updateHudItemName() {
            ItemStack stack = this.getHeldItem();
            this.itemName.showAndStartFade(stack == null ? string.Empty : stack.item.getName(stack.meta), 1.5f);
        }

        // Helper method to get the currently held item
        public ItemStack getHeldItem() {
            return this.slots[this.index].getContents();
        }

        // Helper method to set the currently held item
        public void setHeldItem(ItemStack stack) {
            this.slots[this.index].setContents(stack);
        }
    }
}
