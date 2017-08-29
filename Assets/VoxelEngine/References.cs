using UnityEngine;

namespace VoxelEngine {

    public class References : MonoBehaviour {

        /// <summary> Singleton reference to the list. </summary>
        public static References list;

        // Materials.
        public Material blockMaterial;

        // Textures.
        public Texture2D textureAtlas;
        public Texture2D lightColorSheet;
        public Texture2D debugLightColorSheet;

        // Other prefabs.
        public GameObject blockBreakEffect;
        public GameObject worldPrefab;
        public GameObject chunkPrefab;
        public GameObject shadowPrefab;

        // Containers gui prefabs.
        public GameObject containerHotbar;

        public GameObject containerHeldText;

        // Container Builder Parts.
        public GameObject conatinerPartCanvas;
        public GameObject containerPartSlot;

        public Transform containerLeftOrgin;
        public Transform containerRightOrgin;

        // TileEntity prefabs.
        public GameObject chestPrefab;
        public GameObject glorbPrefab;
        public GameObject lanternPrefab;
        public GameObject torchPrefab;
        public GameObject mushroomPrefab;
        public GameObject mirrorTestPrefab;

        /// <summary>
        /// Called from Main.Awake().
        /// </summary>
        public void loadResources() {
            this.blockMaterial = Resources.Load<Material>("Materials/BlockMaterial");

            this.textureAtlas = Resources.Load<Texture2D>("Images/textureAtlas");
            this.lightColorSheet = Resources.Load<Texture2D>("Images/light_colors");
            this.debugLightColorSheet = Resources.Load<Texture2D>("Images/light_colors_debug");

            this.blockBreakEffect = Resources.Load<GameObject>("Prefabs/BreakBlockEffect");
            this.worldPrefab = Resources.Load<GameObject>("Prefabs/World");
            this.chunkPrefab = Resources.Load<GameObject>("Prefabs/Chunk");
            this.shadowPrefab = Resources.Load<GameObject>("Prefabs/EntityShadow");

            this.chestPrefab = Resources.Load<GameObject>("Prefabs/Blocks/ChestPrefab");
            this.lanternPrefab = Resources.Load<GameObject>("Prefabs/Blocks/LanternPrefab");
            this.torchPrefab = Resources.Load<GameObject>("Prefabs/Blocks/TorchPrefab");
            this.mushroomPrefab = Resources.Load<GameObject>("Prefabs/Blocks/MushroomPrefab");
            this.mirrorTestPrefab = Resources.Load<GameObject>("Prefabs/Blocks/MirrorTestPrefab");

            References.list = this;
        }
    }
}
