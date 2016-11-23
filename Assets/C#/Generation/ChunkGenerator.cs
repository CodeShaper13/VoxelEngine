using SimplexNoise;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator {

    public World world;
    public List<OrePatch> orePatches = new List<OrePatch>();

    public ChunkGenerator(World world) {
        this.world = world;

        this.orePatches.Add(new OrePatch(Block.coal, 0.75f, 2, 2));
    }

    public void generateChunk(Chunk c) {
        //TerrainGen t = new TerrainGen();
        //t.ChunkGen(c);
        //return;

        if(c.chunkX == 0 && c.chunkY == 0 && c.chunkZ == 0) {
            this.generateSpawnChunk(c);
        } else {
            for(int x = 0; x < Chunk.SIZE; x++) {
                for(int y = 0; y < Chunk.SIZE; y++) {
                    for(int z = 0; z < Chunk.SIZE; z++) {
                        float f = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.1f); // 0.015f); Good?
                        if (f <= 0.15f) {
                            float f1 = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.25f);
                            c.setBlock(x, y, z, f1 > 0 ? Block.stone : Block.dirt);
                            //c.setBlock(x, y, z, Block.air);
                        } else {
                            c.setBlock(x, y, z, Block.air);
                        }
                    }
                }
            }
            this.populateChunk(c);
            c.dirty = true;
        }
        //this.checkForPopulation(c);
    }

    private void checkForPopulation(Chunk c) {
        //make a five by five grid around the spawned chunk for easy lookup
        Chunk[,,] isGenerated = new Chunk[5, 5, 5];

        int i, j, k, i1, j1, k1;
        for (i = -2; i < 3; i++) {
            for (j = -2; j < 3; j++) {
                for (k = -2; k < 3; k++) {
                    BlockPos p = new BlockPos(c.pos.x + (i * 16), c.pos.y + (j * 16), c.pos.z + (k * 16));
                    isGenerated[i + 2, j + 2, k + 2] = this.world.getChunk(p);
                }
            }
        }

        for (i = -1; i < 2; i++) {
            for (j = -1; j < 2; j++) {
                for (k = -1; k < 2; k++) {
                    if(isGenerated[i + 1, j + i, k + 1]) {
                        //Check if all the surounded all generated
                        bool flag = false;
                        for(i1 = -1; i1 < 2; i1++) {
                            for (j1 = -1; j1 < 2; j1++) {
                                for (k1 = -1; k1 < 2; k1++) {

                                }
                            }
                        }
                        if(flag) {

                        }
                    }
                }
            }
        }
    }

    //Generates the spawn chunk
    private void generateSpawnChunk(Chunk c) {
        for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
            c.blocks[i] = Block.air;
        }
        for(int i = 0; i < Chunk.SIZE; i++) {
            for(int j = 0; j < Chunk.SIZE; j++) {
                c.setBlock(i, 0, j, Block.wood);
            }
        }
        c.dirty = true;
    }

    private BlockPos randomChunkPos() {
        return new BlockPos(Random.Range(0, Chunk.SIZE), Random.Range(0, Chunk.SIZE), Random.Range(0, Chunk.SIZE));
    }

    public void populateChunk(Chunk c) {
        foreach(OrePatch patch in this.orePatches) {
            for(int i = 0; i < patch.perChunk; i++) {
                patch.generatePatch(c, this.randomChunkPos());
            }
        }
        c.populated = true;
        c.dirty = true;
    }

    public float GetNoise(int x, int y, int z, float scale) {
        return (Noise.Generate(x * scale, y * scale, z * scale));
    }

    public int GetNoise(int x, int y, int z, float scale, int max) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}
