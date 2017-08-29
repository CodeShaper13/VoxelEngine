using System;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Command {

    public class CommandRegion : CommandBase {

        public static BlockPos point1;
        public static BlockPos point2;

        private bool flag;

        public CommandRegion(bool flag) : base("selPoint", "", "Sets a point at the block you are looking at.") {
            this.flag = flag;
        }

        public override string runCommand(World world, EntityPlayer player, string[] args) {
            if(flag) {

            } else {
                
            }
            return "Point set";
        }
    }
}
