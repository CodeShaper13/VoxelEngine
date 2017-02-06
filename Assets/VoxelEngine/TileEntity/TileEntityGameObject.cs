using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.TileEntity {

    public abstract class TileEntityGameObject : TileEntityBase {

        public GameObject gameObject;

        public TileEntityGameObject(World world, int x, int y, int z, GameObject prefab) : base(world, x, y, z) {
            this.gameObject = GameObject.Instantiate(prefab);
            this.gameObject.transform.parent = world.tileEntityWrapper;
        }

        public override void onDestruction(World world, BlockPos pos, byte meta) {
            GameObject.Destroy(this.gameObject);
            base.onDestruction(world, pos, meta);
        }
    }
}
