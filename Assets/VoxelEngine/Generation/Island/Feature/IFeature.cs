using System;
using VoxelEngine.Level;

namespace VoxelEngine.Generation.Island.Feature {

    public interface IFeature {

        void generate(Chunk chunk, Random rnd);
    }
}
