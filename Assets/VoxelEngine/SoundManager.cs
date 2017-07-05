using UnityEngine;

namespace VoxelEngine {

    public class SoundManager : MonoBehaviour {

        public static SoundManager singleton;

        public AudioClip footstep1;
        public AudioClip footstep2;
        public AudioClip footstep3;

        private void Awake() {
            SoundManager.singleton = this;
        }
    }
}
