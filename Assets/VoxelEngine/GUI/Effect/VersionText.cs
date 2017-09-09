using UnityEngine;
using UnityEngine.UI;

namespace VoxelEngine.GUI.Effect {

    public class VersionText : MonoBehaviour {

        private void Awake() {
            this.GetComponent<Text>().text = "V. " + Main.VERSION;
        }
    }
}
