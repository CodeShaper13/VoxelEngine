using UnityEngine;
using UnityEngine.UI;

namespace VoxelEngine.GUI.Effect {

    /// <summary>
    /// Sets a random message on the title screen.
    /// </summary>
    public class TitleTip : MonoBehaviour {

        private Text text;
        private string[] messages = new string[] {
            "Message 0",
            "Message 1",
            "Message 2",
            "Message 3",
            "Message 4",
            "Message 5",
            "Message 6",
            "Message 7",
            "Message 8",
            "Message 9",
        };

        private void Awake() {
            this.text = this.GetComponent<Text>();
        }

        private void OnEnable() {
            this.text.text = this.messages[Random.Range(0, this.messages.Length - 1)];
        }
    }
}
