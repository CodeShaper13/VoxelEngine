using System;

namespace VoxelEngine.Command {

    /// <summary>
    /// Thrown by commands to indicate that the syntax was wrong or malformed.
    /// </summary>
    public class WrongSyntaxException : Exception { }
}
