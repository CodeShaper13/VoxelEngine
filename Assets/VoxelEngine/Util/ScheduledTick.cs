namespace VoxelEngine.Util {

    public class ScheduledTick {

        public BlockPos pos;
        public int remainingTicks;

        public ScheduledTick(BlockPos pos, int ticks) {
            this.pos = pos;
            this.remainingTicks = ticks;
        }
    }
}
