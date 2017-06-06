using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.Command {

    public abstract class CommandBase {

        /// <summary> The name that's typed into the command line. </summary>
        public string commandName;
        /// <summary> A string that shows the command syntax. </summary>
        public string syntax;
        public string description;

        public CommandBase(string name, string syntax, string description) {
            this.commandName = name;
            this.syntax = syntax;
            this.description = description;
        }

        /// <summary>
        /// Prints a message out to the chat log.
        /// </summary>
        public void logMessage(string message) {
            Main.singleton.textWindow.logMessage(message);
        }

        // Unused.
        public virtual string[] getTabOptions() {
            return new string[0];
        }

        /// <summary>
        /// Called to run the command.  Args is a string array of every command arg, not including the command name.
        /// Return a string with a message to print out, or null for nothing.
        /// </summary>
        public abstract string runCommand(World world, EntityPlayer player, string[] args);
    }
}
