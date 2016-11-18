using UnityEngine;
using System.Collections;

public class Item {
    private static int NEXT_ID = 4096;
    private static Item[] ITEM_LIST = new Item[8191];

    public static Item basicItem = new Item();

    public int id = 4096;
    public string name = "null";
    public TexturePos texturePos;

    public RenderData renderData;

    public Item() {
        this.id = Item.NEXT_ID++;
        Item.ITEM_LIST[this.id] = this;

        this.renderData = new RenderDataBillboard();
    }

    public Item setName(string name) {
        this.name = name;
        return this;
    }

    public Item setRenderData(RenderData data) {
        this.renderData = data;
        return this;
    }

    public Item setTexture(int x, int y) {
        this.texturePos = new TexturePos(x, y);
        return this;
    }

    //Static helper methods
    public static void initBlockItems() {
        foreach(Block b in Block.BLOCK_LIST) {
            if(b != null) {
                Item.ITEM_LIST[b.id] = new Item(); //TODO finish
            }
        }
    }

    public static Item getItem(int id) {
        return Item.ITEM_LIST[id];
    }
}
