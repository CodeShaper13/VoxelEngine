namespace VoxelEngine.Items {

    /// <summary>
    /// Used by Blocks and Items that should explode.
    /// </summary>
    public interface IExplosiveObject {

        float getExplosionSize();
    }
}
