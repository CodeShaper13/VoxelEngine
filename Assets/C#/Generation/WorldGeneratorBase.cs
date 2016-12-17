using SimplexNoise;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneratorBase {
    public World world;
    public long seed;
    public List<GenerationFeature> features;

    public WorldGeneratorBase(World world, long seed) {
        this.world = world;
        this.seed = seed;
        this.features = new List<GenerationFeature>();
    }

    public virtual void generateChunk(Chunk chunk) {
        
    }

    public virtual void populateChunk(Chunk chunk) {
        chunk.isPopulated = true;
    }

    public int getNoise(int x, int y, int z, float scale, int max) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}
