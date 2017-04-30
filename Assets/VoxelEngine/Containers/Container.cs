using UnityEngine;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.Util;

namespace VoxelEngine.Containers {

    public class Container : MonoBehaviour {

        public Slot[] slots;

        private ContainerData data;
        private EntityPlayer player;

        /// <summary>
        /// Called when the container is opened.
        /// </summary>
        public virtual void onOpen(ContainerData data, EntityPlayer player) {
            this.data = data;
            this.player = player;

            this.gameObject.SetActive(true);

            for (int i = 0; i < this.slots.Length; i++) {
                this.slots[i].setContents(data.items[i]);
            }
        }

        /// <summary>
        /// Called when the container is closed for any reason.
        /// </summary>
        public virtual void onClose() {
            //TODO we need a reference to the tile entity that this belongs to, to the te knows when the container is closed
            for (int i = 0; i < this.slots.Length; i++) {
                this.data.items[i] = this.slots[i].getContents();
            }
        }

        /// <summary>
        /// Called every frame to render the items in the container.
        /// </summary>
        public void renderContents() {
            ItemStack stack;
            Transform trans;
            for (int i = 0; i < this.slots.Length; i++) {
                stack = this.slots[i].getContents();
                if (stack != null) {
                    trans = this.slots[i].transform;
                    RenderHelper.renderStack(stack, trans.position + -trans.forward);
                }
            }
        }

        /// <summary>
        /// Adds the passed stack to the container, returning any we couldn't add.
        /// </summary>
        public ItemStack addItemStack(ItemStack stack) {
            if(stack == null) {
                return null;
            }

            Slot slot;
            //First try to fill up any slots that already have items
            for (int i = 0; i < this.slots.Length; i++) {
                slot = this.slots[i];
                ItemStack contents = slot.getContents();
                if (contents == null || (!contents.equals(stack)) || contents.count >= ItemStack.MAX_SIZE) {
                    continue;
                }
                // Stacks are equal and slot is not full
                stack = contents.merge(stack);
                slot.updateSlotText();

                if (stack == null) {
                    return null;
                }
            }

            //If we still have stuff to deposite, add it to an empty slot
            for (int i = 0; i < this.slots.Length; i++) {
                slot = this.slots[i];
                if (slot.getContents() == null) {
                    slot.setContents(stack);
                    return null;
                }
            }
            return stack;
        }
    }
}
