using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.TileEntity {

    /// <summary>
    /// Base class for TileEntities that have an associated gameObject.
    /// </summary>
    public abstract class TileEntityGameObject : TileEntityBase {

        public GameObject gameObject;
        public Material[] modelMaterials;

        public TileEntityGameObject(World world, int x, int y, int z, GameObject prefab) : base(world, x, y, z) {
            this.gameObject = GameObject.Instantiate(prefab);
            this.gameObject.transform.parent = world.tileEntityWrapper;
            this.modelMaterials = this.getModelMaterials();
        }

        public override void onDestruction(World world, BlockPos pos, byte meta) {
            GameObject.Destroy(this.gameObject);
            base.onDestruction(world, pos, meta);
        }

        public virtual Material[] getModelMaterials() {
            return new Material[] { this.gameObject.GetComponent<Renderer>().material };
        }
    }
}
