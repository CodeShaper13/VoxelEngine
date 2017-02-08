using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Entities;

namespace VoxelEngine.GUI.Effect {

    public class HeartTremble : MonoBehaviour {

        public EntityPlayer player;
        public Text healthText;
        private float oldValue;
        private int newValue;
        private float scrollSpeed;

        public void Update() {
            if (!Main.singleton.isPaused) {
                if(this.oldValue != this.newValue) {
                    this.oldValue = Mathf.MoveTowards(this.oldValue, this.newValue, Time.deltaTime * this.scrollSpeed);                    
                    this.healthText.text = Mathf.RoundToInt(this.oldValue) + "%";
                }

                if(this.player.health <= 15 && this.player.health > 0) {
                    int i = ((int)Time.time);
                    if (i % 2 == 0) {
                        float f = Time.time - i;
                        Quaternion q;
                        if (f <= 0.25f) {
                            q = Quaternion.Euler(0, 0, 22.5f);
                        }
                        else if (f <= 0.50f) {
                            q = Quaternion.identity;
                        }
                        else if (f <= 0.75f) {
                            q = Quaternion.Euler(0, 0, -22.5f);
                        }
                        else {
                            q = Quaternion.identity;
                        }

                        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, q, 0.5f);
                    }
                }
                else {
                    this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.identity, 0.5f);
                }
            }
        }

        public void startAnimation(int oldValue, int newValue) {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.scrollSpeed = Mathf.Abs(this.oldValue - this.newValue) * 3;
        }
    }
}