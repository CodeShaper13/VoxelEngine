using UnityEngine;

namespace VoxelEngine.Testing {

    public class BitShift : MonoBehaviour {

        public void Awake() {
            byte i = 127;
            for(int j = 0; j < 8; j++) {
                Debug.Log("Slot: " + ((i >> j) & 1));
            }
        }
    }
}
