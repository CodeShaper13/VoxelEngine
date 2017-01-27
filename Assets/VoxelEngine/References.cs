using UnityEngine;

namespace VoxelEngine {

    public class References : MonoBehaviour {
        public static References list; //Singleton

        public Material blockMaterial;
        public Material itemMaterial;
        public Material unlitBlockMaterial;
        public Material unlitItemMaterial;

        public GameObject blockBreakEffect;
        public GameObject worldPrefab;

        public GameObject containerHotbar;
        public GameObject containerInventory;
        public GameObject containerChest;
        public GameObject containerCrafting;
        public GameObject containerFurnace;

        public GameObject chestPrefab;
        public GameObject glorbPrefab;
        public GameObject lanternPrefab;
        public GameObject torchPrefab;

        public Mesh mushroomMesh;

        public void initReferences() {
            References.list = this;
        }

        public static Material getMaterial(int id, bool unlitVarient) {
            if(unlitVarient) {
                return id < 256 ? References.list.unlitBlockMaterial : References.list.unlitItemMaterial;
            }
            else {
                return id < 256 ? References.list.blockMaterial : References.list.itemMaterial;
            }
        }
    }
}
