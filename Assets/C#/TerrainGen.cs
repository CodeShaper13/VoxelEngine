using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGen {
    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 4;

    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = -12;

    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

    float caveFrequency = 0.025f;
    int caveSize = 7;

    float treeFrequency = 0.2f;
    int treeDensity = 3;

    public Chunk ChunkGen(Chunk chunk) {
        //for (int x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.CHUNK_SIZE + 3; x++) {
         //   for (int z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.CHUNK_SIZE + 3; z++) {
         //       chunk = ChunkColumnGen(chunk, x, z);
        //    }
        //}

        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.SIZE; x++) {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.SIZE; z++) {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z) {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight) {
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);
        }

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.SIZE; y++) {
            int caveChance = GetNoise(x, y, z, caveFrequency, 100); //Add this line
            if (y <= stoneHeight) {
                SetBlock(x, y, z, Block.stone, chunk);
            }
            else if (y <= dirtHeight && caveSize < caveChance) {
                SetBlock(x, y, z, Block.grass, chunk);
                if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity) {   //Add this line
                    CreateTree(x, y + 1, z, chunk);                                              //And this line
                }
            }
            else {
                SetBlock(x, y, z, Block.air, chunk);
            }
        }
        return chunk;
    }

    public static int GetNoise(int x, int y, int z, float scale, int max) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }

    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false) {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;
        if (Util.inChunkBounds(x) && Util.inChunkBounds(y) && Util.inChunkBounds(z)) {
            if (replaceBlocks || chunk.getBlock(x, y, z) == null) {
                chunk.setBlock(x, y, z, block); //TODO we changed the chunk.getBlock method, this might make a crash
            }
        }
    }

    void CreateTree(int x, int y, int z, Chunk chunk) {
        //create leaves
        for (int xi = -2; xi <= 2; xi++) {
            for (int yi = 4; yi <= 8; yi++) {
                for (int zi = -2; zi <= 2; zi++) {
                    SetBlock(x + xi, y + yi, z + zi, Block.leaves, chunk, true);
                }
            }
        }
        //create trunk
        for (int yt = 0; yt < 6; yt++) {
            SetBlock(x, y + yt, z, Block.wood, chunk, true);
        }
    }
}