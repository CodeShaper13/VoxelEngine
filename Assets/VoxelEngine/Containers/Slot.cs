using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VoxelEngine.Containers {

    public class Slot : MonoBehaviour, IPointerClickHandler {

        //TODO max count for slot?
        //TODO Maybe an enum?  Wood, stone...
        public int allowedItemTypes;

        /// <summary> Reference to the container that this slot belongs to. </summary>
        public Container container;
        /// <summary> The stack in the slot, may be null. </summary>
        private ItemStack contents;
        private Text slotText;

        private void Awake() {
            this.slotText = this.GetComponentInChildren<Text>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            ContainerManager cm = Main.singleton.containerManager;
            if(!cm.isContainerOpen()) {
                return;
            }
            ItemStack heldStack = cm.heldStack;

            bool leftBtn = eventData.button == PointerEventData.InputButton.Left;
            bool rightBtn = eventData.button == PointerEventData.InputButton.Right;
            bool middleBtn = eventData.button == PointerEventData.InputButton.Middle;

            if(Input.GetKey(KeyCode.LeftShift)) {
                Container jumpTarget = cm.getOppositeContainer(this.container);
                this.setContents(jumpTarget.addItemStack(this.contents));
            }
            else {
                if (leftBtn) {
                    if (heldStack == null && this.contents != null) {
                        // Slot is empty, hand is occupied.  Pick up the stack.
                        cm.setHeldStack(this.contents);
                        this.setContents(null);
                    }
                    else if (heldStack != null && this.contents == null) {
                        // Slot is occupied, hand is not.  Drop off the stack.
                        this.setContents(heldStack);
                        cm.setHeldStack(null);
                    }
                    else if (heldStack != null && this.contents != null) {
                        // Both hand and slot have stuff.
                        if(heldStack.equals(this.contents)) {
                            // Combine, leaving leftover in hand.
                            cm.setHeldStack(this.contents.merge(heldStack));
                            this.setContents(this.contents);
                        } else {
                            // Swap.
                            ItemStack temp = this.contents;
                            this.setContents(heldStack);
                            cm.setHeldStack(temp);
                        }
                    }
                }
                /*
                else if (rightBtn) {
                    if (heldStack == null) {
                        // Holding nothing, pick up 1 item or half the stack
                    }
                    else {
                        // drop off, if we can, 1 item or half the stack
                        if (this.contents.equals(heldStack)) {
                            // Stacks match, we can deposit
                        }
                    }
                }
                */
                else if (middleBtn) {
                    if (heldStack == null && this.contents != null && this.contents.count > 1) {
                        // Pick up half.
                        int i = this.contents.count / 2;

                        // Set hand
                        ItemStack stack = new ItemStack(this.contents);
                        stack.count = i;
                        cm.setHeldStack(stack);

                        // Set slot
                        this.contents.count -= i;
                        this.setContents(this.contents);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the contents of the slot.
        /// </summary>
        public ItemStack getContents() {
            return this.contents;
        }

        /// <summary>
        /// Sets the contents of the slot, updating the count text.
        /// </summary>
        public void setContents(ItemStack stack) {
            this.contents = stack;
            this.updateSlotText();
        }

        /// <summary>
        /// Updates the item count text.
        /// </summary>
        public void updateSlotText() {
            this.slotText.text = (this.contents == null || this.contents.count == 1 ? string.Empty : this.contents.count.ToString());
        }
    }
}
