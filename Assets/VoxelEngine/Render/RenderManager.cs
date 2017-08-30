using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Items;
using VoxelEngine.Render.BlockRender;
using VoxelEngine.Render.Items;

namespace VoxelEngine.Render {

    public class RenderManager {
        
        public static RenderManager instance;

        public static readonly Block[] AIR_ARRAY = new Block[6] { Block.air, Block.air, Block.air, Block.air, Block.air, Block.air };

        public static readonly BlockRenderer BED = new BlockRendererBed();
        public static readonly BlockRenderer BUTTON = new BlockRendererButton();
        public static readonly BlockRenderer CHEST = new BlockRendererChest();
        public static readonly BlockRenderer COBWEB = new BlockRendererCobweb();
        public static readonly BlockRenderer CROSS = new BlockRendererCorn();
        public static readonly BlockRenderer CUBE = new BlockRendererCube();
        public static readonly BlockRenderer FENCE = new BlockRendererFence();
        public static readonly BlockRenderer FLUID = new BlockRendererFluid();
        public static readonly BlockRenderer LADDER = new BlockRendererLadder();
        public static readonly BlockRenderer LOGIC_PLATE = new BlockRendererLogicPlate();
        public static readonly BlockRenderer LOGIC_DELAYER = new BlockRendererLogicDelayer();
        public static readonly BlockRenderer MUSHROOM = new BlockRendererMesh(References.list.mushroomPrefab).setOffsetVector(new Vector3(0, -0.5f, 0)).useColliderComponent();
        public static readonly BlockRenderer RAIL = new BlockRendererRail();
        public static readonly BlockRenderer ROOF = new BlockRendererRoof();
        public static readonly BlockRenderer SLAB = new BlockRendererSlab();
        public static readonly BlockRenderer STAIR = new BlockRendererStairs();
        public static readonly BlockRenderer TORCH = new BlockRendererTorch();
        public static readonly BlockRenderer WIRE = new BlockRendererWire();

        // Used for dev debugging.
        public static readonly BlockRenderer TEST = new BlockRendererTest();
        public static readonly BlockRenderer MIRROR_TEST = new BlockRendererMesh(References.list.mirrorTestPrefab);

        /// <summary> Flat item renderer. </summary>
        public static readonly IRenderItem ITEM_RENDERER_FLAT = new RenderItemBillboard();

        /// <summary> This is set in the Awake method of HudCamera.cs </summary>
        public HudCamera hudCamera;
        public LightColors lightColors;
        public bool useSmoothLighting;

        private MeshBuilder reusableMeshBuilder;
        private PreBakedItem[] preBakedItemMeshes;

        public RenderManager() {
            RenderManager.instance = this;

            this.lightColors = new LightColors();
            this.reusableMeshBuilder = new MeshBuilder();

            this.preRenderItems();

            this.useSmoothLighting = true;
        }

        /// <summary>
        /// Returns a ready to use meshBuilder.
        /// </summary>
        public static MeshBuilder getMeshBuilder() {
            RenderManager.instance.reusableMeshBuilder.prepareForReuse();
            return RenderManager.instance.reusableMeshBuilder;
        }

        public static Mesh getItemMesh(Item item, int meta, bool is3d) {
            int id = item.id;

            if (id < 0 || id >= RenderManager.instance.preBakedItemMeshes.Length) {
                Debug.LogWarning("Could not find prerendered mesh for " + item.getName(meta) + ":" + meta + "  The Item was never baked on startup!  Using placeholder mesh!");
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
