using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.TileEntity {

    public class TileEntityLantern : TileEntityGameObject {

        public TileEntityLantern(World world, int x, int y, int z) : base(world, x, y, z, References.list.lanternPrefab) {
            this.gameObject.transform.position = new Vector3(x, y, z);
        }

        public override int getId() {
            return 3;
        }
    }
}
