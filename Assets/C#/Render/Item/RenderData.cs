//Class that can be overriden to provide special rendering for the itme
public class RenderData {
    
    public virtual MeshData renderItem(ItemStack item) {
        return new MeshData();
    }
}
