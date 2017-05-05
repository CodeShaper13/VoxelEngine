namespace VoxelEngine.Blocks {

    public class BlockAir : Block {

        public BlockAir(byte id) : base(id) {
            this.setStatesUsed(0);
            this.setTransparent();
            this.setReplaceable();
            this.setRenderer(null);
        }

        //public override void onDestroy(World world, BlockPos pos, byte meta) {
        //    base.onDestroy(world, pos, meta);
        //    Debug.Log("Error!  Air is being broken, this shouldn't happen!");
        //}
    }
}
