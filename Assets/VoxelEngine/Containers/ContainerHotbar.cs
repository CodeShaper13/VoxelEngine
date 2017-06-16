using UnityEngine;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.GUI.Effect;
using VoxelEngine.Render;

namespace VoxelEngine.Containers {

    public class ContainerHotbar : Container {

        public FadeText itemName;
        /// <summary> The selected index. </summary>
        private int index;
        private float stackYRot;

        private void Update() {
            if(this.stackYRot > 0) {
                this.stackYRot -= Time.deltaTime;
            }
        }

        public override Container onOpen(ContainerData data, EntityPlayer player) {
            base.onOpen(data, player);
            this.setSelected(0, false);

            return this;
        }

        public override void renderSlotStack(ItemStack stack, Vector3 position, int slotIndex) {
            RenderHelper.renderStack(stack, position, (slotIndex == this.index && this.stackYRot > 0) ? Quaternion.Euler(0, this.stackYRot * 360f, 0) : Quaternion.identity);
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
            this.setSelected(newIndex, true);
        }

        /// <summary>
        /// Sets the passed index to be the selected one, updating slot sizes and held text.
        /// </summary>
        public void setSelected(int index, bool rotateStack) {
            this.slots[this.index].transform.localScale = Vector3.one;
            this.index = index;
            this.slots[this.index].transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            this.updateHudItemName();
            this.stackYRot = 1f;
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
            return this.data.items[this.index];
        }

        /// <summary>
        /// Helper method to set the currently held item.
        /// </summary>
        public void setHeldItem(ItemStack stack) {
            this.data.items[this.index] = stack;
        }

        public int getIndex() {
            return this.index;
        }

        public void setIndex(int index) {
            this.index = index;
        }
    }
}
