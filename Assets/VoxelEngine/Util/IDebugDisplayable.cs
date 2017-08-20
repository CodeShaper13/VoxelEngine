namespace VoxelEngine.Util {

    /// <summary>
    /// An interface that lets objects have a method for drawing debug infomation into the world.
    /// </summary>
    public interface IDebugDisplayable {

        /// <summary>
        /// Draws debugging lines to represent the object in question.
        /// </summary>
        void debugDisplay();
    }
}
