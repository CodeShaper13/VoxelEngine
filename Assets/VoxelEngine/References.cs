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
        public GameObject chunkPrefab;
        public GameObject shadowPrefab;
        public GameObject worldSpaceTooltipPrefab;

        // Containers gui prefabs.
        public GameObject containerHotbar;

        public GameObject containerHeldText;

        // Container Builder Parts.
        public GameObject conatinerPartCanvas;
        public GameObject containerPartSlot;

        public Transform containerLeftOrgin;
        public Transform containerRightOrgin;

        // TileEntity prefabs.
        public GameObject torchPrefab;
    }
}
