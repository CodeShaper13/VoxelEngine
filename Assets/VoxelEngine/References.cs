using UnityEngine;

namespace VoxelEngine {

    public class References : MonoBehaviour {
        public static References list; //Singleton

        // Materials
        public Material blockMaterial;
        public Material itemMaterial;

        public Texture2D lightColorSheet;

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
    }
}
