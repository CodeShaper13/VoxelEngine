using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Render.BlockRender;

namespace VoxelEngine.TileEntity {

    public class TileEntityTorch : TileEntityGameObject {

        public TileEntityTorch(World world, int x, int y, int z, int meta) :
            base(world, x, y, z, References.list.torchPrefab) {
            float f = 0.25f;
            Vector3 pos = new Vector3(x, y + BlockRendererTorch.SHIFT + 0.05f, z);
            if (meta == 1) { // North
                pos.z += f;
            } else if (meta == 2) { // East
                pos.x += f;
            } else if (meta == 3) { // South
                pos.z -= f;
            } else if (meta == 4) { // West
                pos.x -= f;
            }
            this.gameObject.transform.position = pos;
        }

        public override int getId() {
            return 4;
        }
    }
}
