using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ItemStack {

    public const int MAX_SIZE = 16;

    public Item item;
    public int meta;
    public int count;

    public ItemStack(Item i, int meta = 0, int count = 1) {
        this.item = i;
        this.meta = meta;
        this.count = count;
    }

    public override bool Equals(object obj) {
        if(obj is ItemStack) {
            ItemStack s = (ItemStack)obj;
            return this.item.id == s.item.id && this.meta == s.meta;
        }
        return false;
    }

    public ItemStack merge(ItemStack otherStack) {
        if (!this.Equals(otherStack)) {
            return otherStack;
        }

        int combinedTotal = this.count + otherStack.count;

        if (combinedTotal <= ItemStack.MAX_SIZE) {
            this.count = combinedTotal;
            return null; //there is nothing left in the old stack
        }
        else {
            //there will be some leftovers, find out how many
            int freeSpace = ItemStack.MAX_SIZE - this.count;
            this.count = ItemStack.MAX_SIZE;
            otherStack.count -= freeSpace;
            return otherStack;
        }
    }
}
