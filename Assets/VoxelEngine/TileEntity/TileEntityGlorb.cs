using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.TileEntity {

    public class TileEntityGlorb : TileEntityGameObject {

        public TileEntityGlorb(World world, int x, int y, int z) : base(world, x, y, z, References.list.glorbPrefab) {
            this.gameObject.transform.position = new Vector3(x, y + 1, z);
        }

        public override int getId() {
            return 2;
        }
    }
}
