using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.Util;

namespace VoxelEngine.Containers {

    public class ContainerManager {

        // References to all the prebuild containers
        public static Container containerInventory;
        public static Container containerChest;
        public static ContainerHotbar containerHotbar;

        public ItemStack heldStack;
        private Container contLeft; // Other, may not be presest
        private Container contRight; // Player inventory

        private Transform heldText;
        private Text heldTextName;
        private Text heldTextCount;

        public ContainerManager() {
            ContainerManager.containerHotbar = GameObject.Instantiate(References.list.containerHotbar).GetComponent<ContainerHotbar>();
            for (int i = 0; i < ContainerManager.containerHotbar.slots.Length; i++) {
                ContainerManager.containerHotbar.slots[i].container = ContainerManager.containerHotbar;
            }
            ContainerManager.containerHotbar.gameObject.SetActive(false);
            HudCamera.bind(ContainerManager.containerHotbar.GetComponent<Canvas>());

            ContainerManager.containerInventory = this.buildContainer("Inventory", 5, 5, true);
            ContainerManager.containerChest = this.buildContainer("Chest", 2, 2);
            this.heldText = References.list.containerHeldText.transform;
            this.heldTextName = this.heldText.transform.GetChild(0).GetComponent<Text>();
            this.heldTextCount = this.heldText.transform.GetChild(1).GetComponent<Text>();
        }

        public bool isContainerOpen() {
            return this.contRight != null;
        }

        public void setHeldStack(ItemStack stack) {
            if(stack == null) {
                this.heldStack = null;
                this.heldTextName.text = string.Empty;
                this.heldTextCount.text = string.Empty;
            } else {
                this.heldStack = stack;
                this.heldTextName.text = stack.item.getName(stack.meta);
                this.heldTextCount.text = stack.count.ToString();
            }
        }

        // Opens and initializes a container
        public void openContainer(EntityPlayer player, Container container, ContainerData containerData) {
            player.fpc.allowInput = false;

            if (container == ContainerManager.containerInventory) {
                this.contRight = func_02(container, containerData, player);
            } else {
                this.contRight = func_02(ContainerManager.containerInventory, player.dataInventory, player);
                this.contLeft = func_02(container, containerData, player);
            }

            
            Main.hideMouse(false);
        }

        // Closes the open container, doing any required cleanup
        public void closeContainer(EntityPlayer player) {
            if(this.isContainerOpen()) {
                player.fpc.allowInput = true;
                if (this.heldStack != null) {
                    player.dropItem(this.heldStack);
                    this.heldStack = null;
                }
                this.contLeft = this.closeIfNotNull(this.contLeft);
                this.contRight = this.closeIfNotNull(this.contRight);

                Main.hideMouse(true);
            }
        }

        // Returns the opposite container, used for shift clicking hotkey
        public Container getOppositeContainer(Container oppositeOf) {
            if(this.contLeft == null) {
                // Player inventory is open only
                if(oppositeOf == ContainerManager.containerInventory) {
                    return ContainerManager.containerHotbar;
                } else {
                    return ContainerManager.containerInventory;
                }
            } else {
                // An outside container is open
                if(oppositeOf == this.contRight || oppositeOf == this.contLeft) {
                    return ContainerManager.containerHotbar;
                } else {
                    return this.contLeft; // The container that is not the inventory, a chest, ect...
                }
            }
        }

        // Draws all the items for any open container
        public void drawContainerContents() {
            if(this.contRight != null) {
                this.contRight.renderContents();
            }
            if(this.contLeft != null) {
                this.contLeft.renderContents();
            }
            if(this.heldStack != null) {
                this.renderHeldItem();
            }
        }

        //TODO fix text position
        // Draws the currently held item
        private void renderHeldItem() {
            if (this.heldStack != null) {
                Vector3 mousePosition = HudCamera.cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 v = HudCamera.cam.ScreenToViewportPoint(Input.mousePosition);
                this.heldTextName.transform.localPosition = new Vector3((v.x - 0.5f) * 800, (v.y - 0.5f) * 600);

                RenderHelper.renderStack(this.heldStack, mousePosition);
            }
        }

        // Builds a container GameObject, returning it
        private Container buildContainer( string name, int xSize, int ySize, bool isPlayerInventory = false) {
            Vector3 orgin = isPlayerInventory ? References.list.containerRightOrgin.localPosition : References.list.containerLeftOrgin.localPosition;
            int slotSize = 80;
            float xOffset = ((xSize - 1) * slotSize / 2);
            float yOffset = ((ySize - 1) * slotSize / 2);

            GameObject canvas = GameObject.Instantiate(References.list.conatinerPartCanvas);
            HudCamera.bind(canvas.GetComponent<Canvas>());
            canvas.name = name;

            Container container = canvas.GetComponent<Container>();
            container.slots = new Slot[xSize * ySize];

            int index = 0;
            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    GameObject slotObj = GameObject.Instantiate(References.list.containerPartSlot);
                    slotObj.transform.SetParent(canvas.transform);
                    slotObj.transform.localPosition = new Vector3((xOffset - x * slotSize) + orgin.x, (yOffset - y * slotSize) + orgin.y);
                    Slot slot = slotObj.GetComponent<Slot>();
                    slot.container = container;
                    container.slots[index] = slot;
                    index++;
                }
            }

            canvas.SetActive(false);

            return container;
        }

        private Container closeIfNotNull(Container container) {
            if (container != null) {
                container.onClose();
                container.gameObject.SetActive(false);
                container = null;
            }
            return container;
        }

        private Container func_02(Container container, ContainerData data, EntityPlayer player) {
            container.onOpen(data, player);
            return container;
        }
    }
}
