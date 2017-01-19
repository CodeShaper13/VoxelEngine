using UnityEngine;

namespace VoxelEngine {

    public class References : MonoBehaviour {
        public static References list; //Singleton

        public GameObject blockBreakEffect;

        public GameObject containerHotbar;
        public GameObject containerInventory;
        public GameObject containerChest;
        public GameObject containerCrafting;
        public GameObject containerFurnace;

        public void Awake() {
            References.list = this;
        }
    }
}
