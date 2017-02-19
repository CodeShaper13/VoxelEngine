using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Util;

namespace VoxelEngine.Containers {

    public class Container : MonoBehaviour {

        public ContainerData data;
        public EntityPlayer player;
        public Button[] slots;
        private Text[] slotText;

        //Acts like a constructor, but since this is a GameObject we can't have a normal one
        public virtual void initContainer(ContainerData data, EntityPlayer player) {
            this.data = data;
            this.player = player;

            this.slotText = new Text[this.slots.Length];
            for (int i = 0; i < this.slots.Length; i++) {
                int j = i;
                this.slots[i].onClick.AddListener(() => { onSlotClicked(j); });
                this.slotText[i] = this.slots[i].GetComponentInChildren<Text>();
            }

            this.transform.SetParent(HudCamera.camera.transform);
            this.GetComponent<Canvas>().worldCamera = HudCamera.camera;
        }

        public virtual void drawnContents() {
            ItemStack stack;
            for (int i = 0; i < this.slots.Length; i++) {
                stack = this.data.items[i];
                if (stack != null) {
                    Transform t = this.slots[i].transform;
                    this.renderStack(stack, t.position + -t.forward);
                }
                this.slotText[i].text = (stack == null ? string.Empty : stack.count.ToString());
            }

            this.renderHeldItem();
        }

        public virtual void renderHeldItem() {
            if (this.player.heldStack != null) {
                Vector3 mousePosition = HudCamera.camera.ScreenToWorldPoint(Input.mousePosition);
                //mousePosition.z = 1;
                this.renderStack(this.player.heldStack, mousePosition);
            }
        }

        private void renderStack(ItemStack stack, Vector3 pos) {
            Item i = stack.item;
            Graphics.DrawMesh(i.getPreRenderedMesh(stack.meta), i.itemRenderer.getMatrix(pos), References.getUnlitMaterial(i.id), 8, null, 0, null, false, false);
        }

        public void onSlotClicked(int slotIndex) {
            ItemStack slotStack = this.data.items[slotIndex];
            if (this.player.heldStack == null && slotStack != null) {
                this.player.heldStack = slotStack;
                this.data.items[slotIndex] = null;
            }
            else if (this.player.heldStack != null && slotStack == null) {
                this.data.items[slotIndex] = this.player.heldStack;
                this.player.heldStack = null;
            }
            else if (this.player.heldStack != null && slotStack != null) {
                this.data.items[slotIndex] = this.player.heldStack;
                this.player.heldStack = slotStack;
            }
        }

        public virtual void onClose() {

        }
    }
}
