using System;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;

namespace VoxelEngine.Command {

    public class CommandGive : CommandBase {

        public CommandGive() : base("give", "give [item_name/id] <meta> <count>", "Gives the player an item or block") { }

        public override string runCommand(World world, EntityPlayer player, string[] args) {
            if(args.Length > 0) {
                Item item = null;
                int meta = 0;
                int count = 1;

                int itemId = -1;
                if(Int32.TryParse(args[0], out itemId)) {
                    if(itemId < 0 || itemId >= Item.ITEM_LIST.Length) {
                        return "Item could not be found with id of " + itemId;
                    }
                    item = Item.ITEM_LIST[itemId];
                } else {
                    return "Only an id can be used, not a name";
                }
                /*
                 else {
                    // Maybe the player types the items name.
                    string itemName = args[0].Replace('_', ' ');
                    for(int i = 0; i < Item.ITEM_LIST.Length; i ++) {
                        if(item.getName())
                    }
                }
                */

                if(args.Length > 1) {
                    if(!Int32.TryParse(args[1], out meta)) {
                        throw new WrongSyntaxException();
                    }

                    if (args.Length > 2) {
                        if (!Int32.TryParse(args[2], out count)) {
                            throw new WrongSyntaxException();
                        }
                        if(count < 0) {
                            count = 1;
                        } else if(count > item.maxStackSize) {
                            count = item.maxStackSize;
                        }
                    }
                }
                player.tryPickupStack(new ItemStack(item, meta, count));
                return "Gave Player " + itemId + ":" + meta + " x " + count;
            } else {
                throw new WrongSyntaxException();
            }
        }
    }
}
