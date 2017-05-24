using VoxelEngine.Blocks;

namespace VoxelEngine.Items {

    public class ItemTool : Item {

        public float time;
        public EnumToolType toolType;
        public EnumBlockType effectiveOn;

        public ItemTool(int id, float time, EnumToolType toolType, EnumBlockType effectiveOn) : base(id) {
            this.time = time;
            this.toolType = toolType;
            this.effectiveOn = effectiveOn;

            this.setMaxStackSize(1);
        }
    }
}
