using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VoxelEngine.Containers {

    public class Slot : MonoBehaviour, IPointerClickHandler {

        //TODO max count for slot?
        //TODO Maybe an enum?  Wood, stone...
        public int allowedItemTypes;


        /// <summary> The slot index within the container that this coresponds to. </summary>
        public int index;
        /// <summary> Reference to the container that this slot belongs to. </summary>
        public Container container;
        public Text slotText;

        private void Awake() {
            this.slotText = this.GetComponentInChildren<Text>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            if(!Main.singleton.containerManager.isContainerOpen()) {
                return; // Stop interaction with the hotbar when the game is paused.
            }

            bool leftBtn = eventData.button == PointerEventData.InputButton.Left;
            bool rightBtn = eventData.button == PointerEventData.InputButton.Right;
            bool middleBtn = eventData.button == PointerEventData.InputButton.Middle;

            this.container.onSlotClick(this.index, leftBtn, rightBtn, middleBtn);            
        }
    }
}
