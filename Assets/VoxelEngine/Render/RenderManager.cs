using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Render.BlockRender;

namespace VoxelEngine.Render {

    public class RenderManager {
        
        public static RenderManager instance;

        public static BlockRenderer CUBE = new BlockRendererPrimitiveCube();
        public static BlockRenderer CROSS = new BlockRendererPrimitiveCross();
        public static BlockRenderer RAIL = new BlockRendererPrimitiveRail();
        public static BlockRenderer FENCE = new BlockRendererPrimitiveFence();
        public static BlockRenderer LADDER = new BlockRendererPrimitiveLadder();
        public static BlockRenderer LANTERN = new BlockRendererMesh(References.list.lanternPrefab).setRenderInWorld(false);
        public static BlockRenderer TORCH = new BlockRendererTorch();
        public static BlockRenderer MUSHROOM = new BlockRendererMesh(References.list.mushroomPrefab).useRandomMirror().setOffsetVector(new Vector3(0, -0.5f, 0)).useColliderComponent();
        public static BlockRenderer CHEST = new BlockRendererMesh(References.list.chestPrefab).setRenderInWorld(false);

        /// <summary> This is set in the Awake method of HudCamera.cs </summary>
        public HudCamera hudCamera;
        public LightHelper lightHelper;
        private MeshBuilder reusableMeshData;
        /// <summary> A chunk that is all air. </summary>
        public IChunk airChunk;

        public RenderManager() {
            RenderManager.instance = this;

            this.lightHelper = new LightHelper(References.list.lightColorSheet);
            this.reusableMeshData = new MeshBuilder();
            this.airChunk = new AirChunk();
        }

        /// <summary>
        /// Returns a ready to use meshBuilder.
        /// </summary>
        public MeshBuilder getMeshBuilder() {
            this.reusableMeshData.cleanup();
            return this.reusableMeshData;
        }

        /// <summary>
        /// Returns the correct material that corresponds with the block/item id
        /// </summary>
        public static Material getMaterial(int id) {
            return id < 256 ? References.list.blockMaterial : References.list.itemMaterial;
        }
    }
}
