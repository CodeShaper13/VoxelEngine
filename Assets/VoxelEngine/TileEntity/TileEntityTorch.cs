using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.TileEntity {

    public class TileEntityTorch : TileEntityGameObject {

        public TileEntityTorch(World world, int x, int y, int z) : base(world, x, y, z, References.list.torchPrefab) {
            this.gameObject.transform.position = new Vector3(x, y, z);
        }

        public override int getId() {
            return 4;
        }
    }
}
