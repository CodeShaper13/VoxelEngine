using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.Render {

    public class RenderManager {
        
        public static RenderManager instance;

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
