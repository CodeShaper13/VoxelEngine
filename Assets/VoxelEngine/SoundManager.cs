using UnityEngine;

namespace VoxelEngine {

    public class SoundManager : MonoBehaviour {

        public static SoundManager singleton;

        public AudioClip footstep;
        public AudioClip uiButtonClick;

        private AudioListener listenerUi;
        private AudioListener listenerPlayer;
        private AudioSource uiSource;

        private void Awake() {
            SoundManager.singleton = this;
            this.listenerUi = this.GetComponent<AudioListener>();
            this.uiSource = this.GetComponent<AudioSource>();
        }

        /// <summary>
        /// Toggles between the active audio listener.
        /// </summary>
        public static void setUsePlayer(bool usePlayer) {
            SoundManager.singleton.listenerUi.enabled = !usePlayer;
            SoundManager.singleton.listenerPlayer.enabled = usePlayer;
        }

        public static void setPlayerListenerRef(AudioListener listener) {
            SoundManager.singleton.listenerPlayer = listener;
            SoundManager.setUsePlayer(true);
        }

        public AudioSource getUiSource() {
            return this.uiSource;
        }
    }
}
