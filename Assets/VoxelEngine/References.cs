using UnityEngine;

namespace VoxelEngine {

    public class References : MonoBehaviour {
        public static References list; //Singleton

        // Materials
        public Material blockMaterial;
        public Material itemMaterial;
        public Material unlitBlockMaterial;
        public Material unlitItemMaterial;

        // Other prefabs
        public GameObject blockBreakEffect;
        public GameObject worldPrefab;
        public GameObject chunkPrefab;

        // Containers gui prefabs
        public GameObject containerHotbar;

        public GameObject containerHeldText;

        // Container Builder Parts
        public GameObject conatinerPartCanvas;
        public GameObject containerPartSlot;

        public Transform containerLeftOrgin;
        public Transform containerRightOrgin;

        // TileEntity prefabs
        public GameObject chestPrefab;
        public GameObject glorbPrefab;
        public GameObject lanternPrefab;
        public GameObject torchPrefab;
        public GameObject mushroomPrefab;

        public void initReferences() {
            References.list = this;
        }

        public static Material getUnlitMaterial(int id) {
            return id < 256 ? References.list.unlitBlockMaterial : References.list.unlitItemMaterial;
        }

        public static Material getMaterial(int id) {
            return id < 256 ? References.list.blockMaterial : References.list.itemMaterial;
        }
    }
}
