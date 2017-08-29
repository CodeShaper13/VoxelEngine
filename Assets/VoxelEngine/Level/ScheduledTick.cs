using fNbt;
using VoxelEngine.Util;

namespace VoxelEngine.Level {

    public class ScheduledTick {

        /// <summary> Seconds until the tick should take place. </summary>
        public float timeUntil;
        /// <summary> In world coords. </summary>
        public BlockPos pos;

        public ScheduledTick(BlockPos pos, float timeUntil) {
            this.timeUntil = timeUntil;
            this.pos = pos;
        }

        public ScheduledTick(NbtCompound tag) {
            this.timeUntil = tag.Get<NbtFloat>("timeUntil").Value;
            this.pos = NbtHelper.readDirectBlockPos(tag, "pos");
        }

        public NbtCompound writeToNbt() {
            NbtCompound tag = new NbtCompound();
            tag.Add(new NbtFloat("timeUntil", this.timeUntil));
            NbtHelper.writeDirectBlockPos(tag, this.pos, "pos");
            return tag;
        }
    }
}
