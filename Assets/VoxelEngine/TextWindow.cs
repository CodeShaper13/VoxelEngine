using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VoxelEngine {

    /// <summary>
    /// The command window.
    /// </summary>
    public class TextWindow {

        public bool isOpen;

        /// <summary> The parent of all the chat log objects. </summary>
        private Transform root;
        /// <summary> The number of lines to text in the log. </summary>
        private int textLines;
        private InputField inputField;
        private Text outputField;

        public TextWindow(Transform root) {
            this.isOpen = false;

            this.root = root;
            this.inputField = this.root.GetChild(1).GetComponent<InputField>();
            this.outputField = this.root.GetChild(2).GetComponent<Text>();

            this.root.gameObject.SetActive(false);
        }

        public void openWindow() {
            this.isOpen = true;
            this.root.gameObject.SetActive(true);
            Main.hideMouse(false);

            // Make the input field selected.
            EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        public void closeWindow() {
            this.isOpen = false;
            this.clearInputLine();
            this.root.gameObject.SetActive(false);
            Main.hideMouse(true);
        }

        /// <summary>
        /// Called when the enter key is pressed.
        /// </summary>
        public void onEnter(string text) {
            Main.singleton.commandManager.tryRunCommand(text);
            this.clearInputLine();
        }

        /// <summary>
        /// Prints a message out to the chat log.
        /// </summary>
        public void logMessage(string message) {
            this.textLines++;
            if (this.textLines > 14) {
                // Remove oldest line
                string s = this.outputField.text;
                Debug.Log(s.IndexOf('\n'));
                this.outputField.text = s.Substring(s.IndexOf('\n') + 1);
                textLines--;
            }

            this.outputField.text += (message + '\n');
        }

        /// <summary>
        /// Makes the input line blank, or "".
        /// </summary>
        private void clearInputLine() {
            this.inputField.text = string.Empty;
        }
    }
}
