using UnityEngine;

namespace VoxelEngine.Testing {

    public class BitShift : MonoBehaviour {

        public void Awake() {
            // Get bit
            /*
            byte i = 127;
            for(int j = 0; j < 8; j++) {
                Debug.Log("Slot: " + ((i >> j) & 1));
            }
            */

            // Set bit
            byte i = 0;
            i |= 1 << 2;
            for (int j = 0; j < 8; j++) {
                Debug.Log("Slot: " + ((i >> j) & 1));
            }
        }
    }
}
