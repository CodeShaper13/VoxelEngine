using UnityEngine;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.GUI.Effect;

namespace VoxelEngine.Containers {

    public class ContainerHotbar : Container {

        public FadeText itemName;
        /// <summary> The selected index. </summary>
        public int index;

        public override Container onOpen(ContainerData data, EntityPlayer player) {
            base.onOpen(data, player);
            this.scroll(0);

            return this;
        }

        /// <summary>
        /// Scrolls the hotbar index in the passed direction.
        /// </summary>
        public void scroll(int scrollDirection) {
            int newIndex = this.index + scrollDirection;
            if (newIndex > 8) {
                newIndex = 0;
            } else if (newIndex < 0) {
                newIndex = 8;
            }
            this.setSelected(newIndex);
        }

        /// <summary>
        /// Sets the passed index to be the selected one, updating slot sizes and held text.
        /// </summary>
        public void setSelected(int index) {
            this.slots[this.index].transform.localScale = Vector3.one;
            this.index = index;
            this.slots[this.index].transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            this.updateHudItemName();
        }

        /// <summary>
        /// Updates the hud name with the current held item.
        /// </summary>
        public void updateHudItemName() {
            ItemStack stack = this.getHeldItem();
            this.itemName.showAndStartFade(stack == null ? string.Empty : stack.item.getName(stack.meta), 1.5f);
        }

        /// <summary>
        /// Helper method to get the currently held item.
        /// </summary>
        public ItemStack getHeldItem() {
            return this.data.items[this.index]; // this.slots[this.index].getContents();
        }

        /// <summary>
        /// Helper method to set the currently held item.
        /// </summary>
        public void setHeldItem(ItemStack stack) {
            this.data.items[this.index] = stack; // this.slots[this.index].setContents(stack);
        }
    }
}
