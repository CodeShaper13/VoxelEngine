using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.Command {

    public class CommandHelp : CommandBase {

        public CommandHelp() : base("help", "help", "Shows a list of all the commands") { }

        public override string runCommand(World world, EntityPlayer player, string[] args) {
            this.logMessage("Commands:");
            CommandBase[] cmds = Main.singleton.commandManager.getCommandList();
            CommandBase c;
            for(int i = 0; i < cmds.Length; i ++) {
                c = cmds[i];
                this.logMessage("  " + c.syntax + ":  " + c.description);
            }
            return null;
        }
    }
}
