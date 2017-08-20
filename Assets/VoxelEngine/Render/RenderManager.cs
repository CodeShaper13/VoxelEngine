using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Items;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.Render.Items;

namespace VoxelEngine.Render {

    public class RenderManager {
        
        public static RenderManager instance;

        public static bool[] TRUE_ARRAY = new bool[6] { true, true, true, true, true, true };
        public static Block[] AIR_ARRAY = new Block[6] { Block.air, Block.air, Block.air, Block.air, Block.air, Block.air };

        public static BlockRenderer BED = new BlockRendererBed();
        public static BlockRenderer CHEST = new BlockRendererChest();
        public static BlockRenderer CROSS = new BlockRendererCorn();
        public static BlockRenderer CUBE = new BlockRendererCube();
        public static BlockRenderer FENCE = new BlockRendererFence();
        public static BlockRenderer FLUID = new BlockRendererFluid();
        public static BlockRenderer LADDER = new BlockRendererLadder();
        public static BlockRenderer LOGIC_PLATE = new BlockRendererLogicPlate();
        public static BlockRenderer MUSHROOM = new BlockRendererMesh(References.list.mushroomPrefab).setOffsetVector(new Vector3(0, -0.5f, 0)).useColliderComponent();
        public static BlockRenderer RAIL = new BlockRendererRail();
        public static BlockRenderer ROOF = new BlockRendererRoof();
        public static BlockRenderer SLAB = new BlockRendererSlab();
        public static BlockRenderer STAIR = new BlockRendererStairs();
        public static BlockRenderer TORCH = new BlockRendererTorch();
        public static BlockRenderer WIRE = new BlockRendererWire();

        // Used for dev debugging.
        public static BlockRenderer MIRROR_TEST = new BlockRendererMesh(References.list.mirrorTestPrefab);

        public static IRenderItem ITEM_RENDERER_BILLBOARD = new RenderItemBillboard();

        /// <summary> This is set in the Awake method of HudCamera.cs </summary>
        public HudCamera hudCamera;
        public LightHelper lightHelper;

        private MeshBuilder reusableMeshBuilder;
        private PreBakedItem[] preBakedItemMeshes;

        public RenderManager() {
            RenderManager.instance = this;

            this.lightHelper = new LightHelper(References.list.lightColorSheet);
            this.reusableMeshBuilder = new MeshBuilder();

            this.preRenderItems();
        }

        /// <summary>
        /// Returns a ready to use meshBuilder.
        /// </summary>
        public static MeshBuilder getMeshBuilder() {
            RenderManager.instance.reusableMeshBuilder.cleanup();
            return RenderManager.instance.reusableMeshBuilder;
        }

        /// <summary>
        /// Returns the correct material that corresponds with the block/item id
        /// </summary>
        public static Material getMaterial(int id) {
            return References.list.blockMaterial;
        }

        public static Mesh getItemMesh(Item item, int meta, bool is3d) {
            int id = item.id;

            if (id < 0 || id >= RenderManager.instance.preBakedItemMeshes.Length) {
                Debug.LogWarning("Could not find prerendered mesh for " + item.getName(meta) + ":" + meta + "  Using placeholder mesh!");
                return RenderManager.getItemMesh(Block.placeholder.asItem(), 0, is3d);
            } else {
                return RenderManager.instance.preBakedItemMeshes[id].getMesh(meta, is3d);
            }
        }

        /// <summary>
        /// Prerenders all the items.
        /// </summary>
        private void preRenderItems() {
            this.preBakedItemMeshes = new PreBakedItem[Item.ITEM_LIST.Length];

            for (int i = 0; i < Item.ITEM_LIST.Length; i++) {
                Item item = Item.ITEM_LIST[i];
                if (item != null) {
                    this.preBakedItemMeshes[i] = new PreBakedItem(item);
                }
            }
        }
    }
}
