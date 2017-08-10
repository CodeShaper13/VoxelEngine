using UnityEngine;
using UnityEngine.UI;

namespace VoxelEngine.GUI.Effect {

    /// <summary>
    /// Sets a random message on the title screen.
    /// </summary>
    public class TitleTip : MonoBehaviour {

        private Text text;
        private string[] messages = new string[] {
            "Give barbed wire a shot, its cheaper and easier!",
            "Thomas Edison's lights!",
            "Gold in the Black Hills, South Dakota?",
            "Try talking with Alexander Graham Bell",
            "Farewell, Lieutenant Colonel George Custer",
            "Welcome to the Union, Colorado",
            "Goodbye Pacific whaling industry",
            "Look out for Rockefeller's oil empire!",
            "R.I.P. James Garfield",
            //"If you visit London, watch out for Jack!", // Jack the Ripper
            "Check out the new Yosemite National Park",
            "Your work will live on, Vincent Van Gogh",
            "Try a what?  W. L. Judson's zipper?",
            "Court rules \"Separate but equal\"",
            "Welcome to the poles, 	Idahoan women!",
            "An undergorund train?  In Boston?",
            "Try using a new paper clip on that",
            "World Population: 1.7 billion",
        };

        private void Awake() {
            this.text = this.GetComponent<Text>();
        }

        private void OnEnable() {
            this.text.text = this.getRandomText();
        }

        private string getRandomText() {
            if(Random.Range(-1, int.MaxValue) == 0) {
                return "1 in 2,147,483,647 chance of seeing this!";
            } else {
                return this.messages[Random.Range(0, this.messages.Length - 1)];
            }
        }
    }
}
