using System;
using UnityEngine;
using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.Command {

    public class CommandTp : CommandBase {

        public CommandTp() : base("tp", "tp [x] [y] [z]", "Teleports the player to x, y, z") { }

        public override string runCommand(World world, EntityPlayer player, string[] args) {
            if(args.Length != 3) {
                throw new WrongSyntaxException();
            } else {
                int x;
                int y;
                int z;

                if (Int32.TryParse(args[0], out x) &&
                    Int32.TryParse(args[1], out y) &&
                    Int32.TryParse(args[2], out z)) {
                    player.transform.position = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
                } else {
                    throw new WrongSyntaxException();
                }
            }

            return null;
        }
    }
}
