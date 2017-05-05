using UnityEngine;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.Util;

namespace VoxelEngine.Containers {

    public class Container : MonoBehaviour {

        public Slot[] slots;

        public ContainerData data;
        private EntityPlayer player;

        /// <summary>
        /// Called when the container is opened.
        /// </summary>
        public virtual Container onOpen(ContainerData data, EntityPlayer player) {
            this.data = data;
            this.player = player;

            this.gameObject.SetActive(true);

            //for (int i = 0; i < this.slots.Length; i++) {
            //    this.slots[i].setContents(data.items[i]);
            //}

            return this;
        }

        /// <summary>
        /// Called when the container is closed for any reason.
        /// </summary>
        public virtual void onClose() {
            //TODO we need a reference to the tile entity that this belongs to, to the te knows when the container is closed
            //for (int i = 0; i < this.slots.Length; i++) {
            //    this.data.items[i] = this.slots[i].getContents();
            //}
        }

        /// <summary>
        /// Called every frame to render the items in the container.
        /// </summary>
        public void renderContents() {
            ItemStack stack;

            // Set slot text
            for (int i = 0; i < this.data.items.Length; i++) {
                //Debug.Log(i);
                stack = this.data.items[i];
                if (stack != null) {
                    //Debug.Log(stack.count);
                }
                this.slots[i].slotText.text = (stack == null || stack.count == 1 ? string.Empty : stack.count.ToString());
            }
        
            Transform trans;
            for (int i = 0; i < this.data.items.Length; i++) {
                stack = this.data.items[i];// this.slots[i].getContents();
                if (stack != null) {
                    trans = this.slots[i].transform;
                    RenderHelper.renderStack(stack, trans.position + -trans.forward);
                }
            }
        }

        public void onSlotClick(int i, bool leftBtn, bool rightBtn, bool middleBtn) {
            ContainerManager cm = Main.singleton.containerManager;
            ItemStack heldStack = cm.heldStack;
            ItemStack slotContents = this.data.items[i];

            if (Input.GetKey(KeyCode.LeftShift)) {
                Container jumpTarget = cm.getOppositeContainer(this);
                this.data.items[i] = jumpTarget.data.addItemStack(slotContents);
            } else {
                if (leftBtn) {
                    if (heldStack == null && slotContents != null) {
                        // Slot is empty, hand is occupied.  Pick up the stack.
                        cm.setHeldStack(slotContents);
                        this.data.items[i] = null;
                    }
                    else if (heldStack != null && slotContents == null) {
                        // Slot is occupied, hand is not.  Drop off the stack.
                        this.data.items[i] = heldStack;
                        cm.setHeldStack(null);
                    }
                    else if (heldStack != null && slotContents != null) {
                        // Both hand and slot have stuff.
                        if (heldStack.equals(slotContents)) {
                            // Combine, leaving leftover in hand.
                            cm.setHeldStack(slotContents.merge(heldStack));
                            this.data.items[i] = slotContents;
                        }
                        else {
                            // Swap.
                            ItemStack temp = slotContents;
                            this.data.items[i] = heldStack;
                            cm.setHeldStack(temp);
                        }
                    }
                }
                else if (rightBtn) {
                    if (heldStack == null && slotContents != null) {
                        ItemStack temp = new ItemStack(slotContents);
                        temp.count = 1;
                        cm.setHeldStack(temp);
                        this.data.items[i] = slotContents.safeDeduction();
                    }
                    else if (heldStack != null) {
                        // We're holding something
                        if (slotContents == null) {
                            // Drop one item off into the empty slot.
                            ItemStack temp = new ItemStack(heldStack);
                            temp.count = 1;
                            this.data.items[i] = temp;
                            heldStack = heldStack.safeDeduction();
                        }
                        else if (slotContents.equals(heldStack) && slotContents.count < ItemStack.MAX_SIZE) {
                            // The held type is the same as the slot, and it's not full
                            this.data.items[i] = slotContents.safeDeduction();
                            heldStack.count += 1;
                        }
                    }
                }
                else if (middleBtn) {
                    if (heldStack == null && slotContents != null && slotContents.count > 1) {
                        // Pick up half.
                        int quantity = slotContents.count / 2;

                        // Set hand
                        ItemStack stack = new ItemStack(slotContents);
                        stack.count = quantity;
                        cm.setHeldStack(stack);

                        // Set slot
                        slotContents.count -= quantity;
                        this.data.items[i] = slotContents;
                    }
                }
            }
        }

        /*
        /// <summary>
        /// Adds the passed stack to the container, returning any we couldn't add.
        /// </summary>
        public ItemStack addItemStack(ItemStack stack) {
            if(stack == null) {
                return null;
            }

            //Slot slot;
            // First try to fill up any slots that already have items
            for (int i = 0; i < this.data.items.Length; i++) {
                //slot = this.slots[i];
                ItemStack contents = this.data.items[i];// slot.getContents();
                if (contents == null || (!contents.equals(stack)) || contents.count >= ItemStack.MAX_SIZE) {
                    continue;
                }
                // Stacks are equal and slot is not full
                stack = contents.merge(stack);
                //slot.updateSlotText();

                if (stack == null) {
                    return null;
                }
            }

            // If we still have stuff to deposite, add it to an empty slot
            for (int i = 0; i < this.data.items.Length; i++) {
                //slot = this.slots[i];
                if (this.data.items[i] == null) {
                    this.data.items[i] = stack;
                    return null;
                }
            }
            return stack;
        }
        */
    }
}
