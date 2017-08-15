using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.Render;

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
        private HudCamera hudCamera;

        public ContainerManager() {
            this.hudCamera = RenderManager.instance.hudCamera;

            ContainerManager.containerHotbar = GameObject.Instantiate(References.list.containerHotbar).GetComponent<ContainerHotbar>();
            for (int i = 0; i < ContainerManager.containerHotbar.slots.Length; i++) {
                ContainerManager.containerHotbar.slots[i].container = ContainerManager.containerHotbar;
                ContainerManager.containerHotbar.slots[i].index = i;
            }
            ContainerManager.containerHotbar.gameObject.SetActive(false);
            RenderManager.instance.hudCamera.bindToHudCamera(ContainerManager.containerHotbar.GetComponent<Canvas>());

            ContainerManager.containerInventory = this.buildContainer("Inventory", 5, 5, true);
            ContainerManager.containerChest = this.buildContainer("Chest", 3, 3);

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

        /// <summary>
        /// Opens and initializes a container.
        /// </summary>
        public void openContainer(EntityPlayer player, Container container, ContainerData containerData) {
            //player.fpc.allowInput = false;

            if (container == ContainerManager.containerInventory) {
                this.contRight = container.onOpen(containerData, player);
            } else {
                this.contRight = ContainerManager.containerInventory.onOpen(player.dataInventory, player);
                this.contLeft = container.onOpen(containerData, player);
            }

            Main.hideMouse(false);
        }

        /// <summary>
        /// Closes the open container, doing any required cleanup.
        /// </summary>
        public void closeContainer(EntityPlayer player) {
            if(this.isContainerOpen()) {
                //player.fpc.allowInput = true;
                if (this.heldStack != null) {
                    player.dropItem(this.heldStack);
                    this.heldStack = null;
                }
                this.contLeft = this.closeIfNotNull(this.contLeft);
                this.contRight = this.closeIfNotNull(this.contRight);

                Main.hideMouse(true);
            }
        }

        /// <summary>
        /// Returns the opposite container, used for shift clicking hotkey.
        /// </summary>
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

        /// <summary>
        /// Draws all the items for any open container.
        /// </summary>
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

            ContainerManager.containerHotbar.renderContents();
        }

        //TODO fix text position
        /// <summary>
        /// Draws the currently held item and info text
        /// </summary>
        private void renderHeldItem() {
            if (this.heldStack != null) {
                Vector3 mousePosition = this.hudCamera.orthoCamera.ScreenToWorldPoint(Input.mousePosition);
                //Vector3 v = this.hudCamera.orthoCamera.ScreenToViewportPoint(Input.mousePosition);
                //this.heldTextName.transform.localPosition = new Vector3((v.x - 0.5f) * 800, (v.y - 0.5f) * 600);

                //Vector3 mousePosition = this.hudCamera.orthoCamera.ScreenToWorldPoint(Input.mousePosition);

                this.heldTextName.transform.localPosition = this.hudCamera.orthoCamera.ScreenToViewportPoint(Input.mousePosition);

                RenderHelper.renderStack(this.heldStack, mousePosition, Quaternion.identity);
            }
        }

        /// <summary>
        /// Creates a container gameObject, prepares it for use and returns it.
        /// </summary>
        private Container buildContainer(string name, int xSize, int ySize, bool isPlayerInventory = false) {
            Vector3 orgin = isPlayerInventory ? References.list.containerRightOrgin.localPosition : References.list.containerLeftOrgin.localPosition;
            int slotSize = 80;
            float xOffset = ((xSize - 1) * slotSize / 2);
            float yOffset = ((ySize - 1) * slotSize / 2);

            GameObject canvas = GameObject.Instantiate(References.list.conatinerPartCanvas);
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
                    //Debug.Log(index);
                    slot.container = container;
                    slot.index = index;
                    container.slots[index] = slot;
                    index++;
                }
            }

            this.hudCamera.bindToHudCamera(canvas.GetComponent<Canvas>());

            // Hide the container ui for now.
            canvas.SetActive(false);

            return container;
        }

        /// <summary>
        /// Closes the passed container, if it is not null, then returns null.
        /// </summary>
        private Container closeIfNotNull(Container container) {
            if (container != null) {
                container.onClose();
                container.gameObject.SetActive(false);
                container = null;
            }
            return container;
        }
    }
}
