using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VoxelEngine.Containers {

    /// <summary>
    /// Attached to the button game object that represents each slot.
    /// </summary>
    public class Slot : MonoBehaviour, IPointerClickHandler {

        /// <summary> The slot index within the container that this coresponds to. </summary>
        public int index;
        /// <summary> Reference to the container that this slot belongs to. </summary>
        public Container container;
        private Text slotText;

        private void Awake() {
            this.slotText = this.GetComponentInChildren<Text>();
        }

        /// <summary>
        /// Called by Unity because we implement IPointerClickHandler when we click on the game object.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData) {
            if(!Main.singleton.containerManager.isContainerOpen()) {
                return; // Stop interaction with the hotbar when the game is paused.
            }

            bool leftBtn = eventData.button == PointerEventData.InputButton.Left;
            bool rightBtn = eventData.button == PointerEventData.InputButton.Right;
            bool middleBtn = eventData.button == PointerEventData.InputButton.Middle;

            this.container.onSlotClick(this.index, leftBtn, rightBtn, middleBtn);            
        }

        public void setSlotText(string text) {
            this.slotText.text = text;
        }
    }
}
