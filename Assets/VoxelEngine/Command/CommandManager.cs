namespace VoxelEngine.Command {

    public class CommandManager {

        private CommandBase[] commands;

        public CommandManager() {
            this.commands = new CommandBase[] {
                new CommandHelp(),
                new CommandGive(),
                new CommandTp(),
            };
        }

        /// <summary>
        /// Tries to run a command.  Line is the contents of the command line.
        /// </summary>
        /// <param name="line"></param>
        public void tryRunCommand(string line) {
            line = line.Trim();

            int j = line.IndexOf(' ');

            string cmdName;
            string[] args;

            if (j == -1) {
                cmdName = line;
                args = new string[0];
            } else {
                cmdName = line.Substring(0, j);
                args = line.Substring(j + 1).Split(' ');
            }

            CommandBase cmd;
            for (int i = 0; i < this.commands.Length; i++) {
                cmd = this.commands[i];
                if (cmdName == cmd.commandName) {
                    try {
                        string result = cmd.runCommand(Main.singleton.worldObj, Main.singleton.player, args);
                        if(result != null) {
                            this.func(result);
                        }
                    } catch (WrongSyntaxException exception) {
                        this.func(cmd.syntax);
                    }
                    return;
                }
            }
            this.func("Command \"" + cmdName + "\" could not be found.  Try \"help\" for a list of commands");
        }

        public CommandBase[] getCommandList() {
            return this.commands;
        }

        private void func(string s) {
            Main.singleton.textWindow.logMessage(s);
        }
    }
}