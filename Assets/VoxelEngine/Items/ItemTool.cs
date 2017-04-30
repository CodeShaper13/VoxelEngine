using VoxelEngine.Blocks;

namespace VoxelEngine.Items {

    public class ItemTool : Item {

        public float time;
        public EnumToolType toolType;
        public Block.Type effectiveOn;

        public ItemTool(int id, float time, EnumToolType toolType, Block.Type effectiveOn) : base(id) {
            this.time = time;
            this.toolType = toolType;
            this.effectiveOn = effectiveOn;
        }
    }
}
