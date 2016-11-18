using UnityEngine;

public class PlayerInventory {
    public GameObject[] inventoryObj;
    public ItemStack[] hotbar = new ItemStack[9];
    public ItemStack[,] inventory = new ItemStack[9, 3];

	public PlayerInventory() {

    }
}
