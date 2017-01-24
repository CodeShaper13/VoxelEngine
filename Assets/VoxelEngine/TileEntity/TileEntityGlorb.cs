using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.TileEntity {

    public class TileEntityGlorb : TileEntityBase {

        public GameObject light;

        public TileEntityGlorb(World world, int x, int y, int z) : base(world, x, y, z) {
            this.light = GameObject.Instantiate(References.list.glorbLight);
            this.light.transform.position = new Vector3(x, y + 1, z);
        }

        public override void onDestruction(World world, BlockPos pos, byte meta) {
            GameObject.Destroy(this.light);
            base.onDestruction(world, pos, meta);
        }

        public override int getId() {
            return 2;
        }
    }
}
