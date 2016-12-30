using UnityEngine;

public class BlockGlorb : Block {

    public override ItemStack[] getDrops(byte meta, ItemTool brokenWith) {
        if(brokenWith != null && brokenWith.toolType == ItemTool.ToolType.PICKAXE) {
            return new ItemStack[] { new ItemStack(Item.glorbDust) };
        }
        return new ItemStack[0];
    }

    public override GameObject getAssociateGameObject() {
        GameObject g = new GameObject();
        g.AddComponent<Light>();
        return g;
    }
}
