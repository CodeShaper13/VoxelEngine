using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.TileEntity {

    public class TileEntityTorch : TileEntityGameObject {

        public TileEntityTorch(World world, int x, int y, int z, byte meta) : base(world, x, y, z, References.list.torchPrefab) {
            float f = 0.35f;
            float f1 = 15.0f;
            Vector3 pos;
            Vector3 rot;
            if (meta == 1) { // North
                pos = new Vector3(x, y, z + f);
                rot = new Vector3(-f1, 0, 0);
            }
            else if (meta == 2) { // East
                pos = new Vector3(x + f, y, z);
                rot = new Vector3(0, 0, f1);
            }
            else if (meta == 3) { // South
                pos = new Vector3(x, y, z - f);
                rot = new Vector3(f1, 0, 0);
            }
            else if (meta == 4) { // West
                pos = new Vector3(x - f, y, z);
                rot = new Vector3(0, 0, -f1);
            }
            else { // 0, On floor
                pos = new Vector3(x, y, z);
                rot = Vector3.zero;
            }
            this.gameObject.transform.localPosition = pos;
            this.gameObject.transform.localEulerAngles = rot;
        }

        public override int getId() {
            return 4;
        }
    }
}
