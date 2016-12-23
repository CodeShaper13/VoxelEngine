public class ItemTool : Item {

    public float time;
    public ToolType toolType;
    public Block.Type effectiveOn;

    public ItemTool(float time, ToolType toolType, Block.Type effectiveOn) : base() {
        this.time = time;
        this.toolType = toolType;
        this.effectiveOn = effectiveOn;
    }

    public enum ToolType {
        PICKAXE,
        SHOVEL
    }
}
