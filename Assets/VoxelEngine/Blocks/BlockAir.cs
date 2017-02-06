using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockAir : Block {

        public BlockAir(byte id) : base(id) { }

        //public override void onDestroy(World world, BlockPos pos, byte meta) {
        //    base.onDestroy(world, pos, meta);
        //    Debug.Log("Error!  Air is being broken, this shouldn't happen!");
        //}
    }
}
