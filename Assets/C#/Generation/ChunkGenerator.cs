using SimplexNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChunkGenerator {

    public World world;

    public ChunkGenerator(World world) {
        this.world = world;
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
