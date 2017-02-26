using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VoxelEngine.Containers {

    public class Slot : MonoBehaviour, IPointerClickHandler {

        //TODO max count for slot?
        //TODO Maybe an enum?  Wood, stone...
        public int allowedItemTypes;

        public Container container;
        private ItemStack contents;
        private Text slotText;

        public void Awake() {
            this.slotText = this.GetComponentInChildren<Text>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            ContainerManager cm = Main.singleton.containerManager;
            ItemStack heldStack = cm.heldStack;

            bool left = eventData.button == PointerEventData.InputButton.Left;
            bool right = eventData.button == PointerEventData.InputButton.Right;

            if(Input.GetKey(KeyCode.LeftShift)) {
                Container jumpTarget = cm.getOppositeContainer(this.container);
                this.setContents(jumpTarget.addItemStack(this.contents));
            } else {
                if (left) {
                    if (heldStack == null && this.contents != null) {
                        // Pick up the stack
                        cm.setHeldStack(this.contents);
                        this.setContents(null);
                    }
                    else if (heldStack != null && this.contents == null) {
                        // Drop off the stack
                        this.setContents(heldStack);
                        cm.setHeldStack(null);
                    }
                    else if (heldStack != null && this.contents != null) {
                        // Not equal, swap stacks
                        this.setContents(heldStack);
                        cm.setHeldStack(this.contents);
                    }
                }
                else if (right) {
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
            }
        }

        public ItemStack getContents() {
            return this.contents;
        }

        // Sets the contents of the slot, updating the count text
        public void setContents(ItemStack stack) {
            this.contents = stack;
            this.updateSlotText();
        }

        public void updateSlotText() {
            this.slotText.text = (this.contents == null ? string.Empty : this.contents.count.ToString());
        }
    }
}
