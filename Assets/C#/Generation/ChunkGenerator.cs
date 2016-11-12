using SimplexNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChunkGenerator {
    private float stoneBaseHeight = -24;
    private float stoneBaseNoise = 0.05f;
    private float stoneBaseNoiseHeight = 4;

    private float stoneMountainHeight = 48;
    private float stoneMountainFrequency = 0.008f;
    private float stoneMinHeight = -12;

    private float dirtBaseHeight = 1;
    private float dirtNoise = 0.04f;
    private float dirtNoiseHeight = 3;

    private float caveFrequency = 0.025f;
    private int caveSize = 7;

    private float treeFrequency = 0.2f;
    private int treeDensity = 3;


    public float caveThreshold = 0.5f;

    public ChunkGenerator() {

    }

    public void generateChunk(Chunk c) {
        if(c.chunkX == 0 && c.chunkY == 0 && c.chunkZ == 0) {
            this.generateOrgin(c);
        } else {
            for(int x = 0; x < 16; x++) {
                for(int y = 0; y < 16; y++) {
                    for(int z = 0; z < 16; z++) {
                        float f = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.1f);
                        if (f >= 0.5f) {
                            float f1 = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.25f);
                            c.setBlock(x, y, z, f1 > 0 ? Block.stone : Block.dirt);
                        } else {
                            c.setBlock(x, y, z, Block.air);
                        }
                    }
                }
            }
            c.dirty = true;
        }
    }

    private void generateOrgin(Chunk c) {
        for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
            c.blocks[i] = Block.air;
        }
        c.dirty = true;
    }

    public int GetNoise(int x, int y, int z, float scale) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f));
    }

    public int GetNoise(int x, int y, int z, float scale, int max) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}
