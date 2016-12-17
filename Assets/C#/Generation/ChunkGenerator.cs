using SimplexNoise;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator {

    public World world;
    public List<OrePatch> orePatches = new List<OrePatch>();

    public ChunkGenerator(World world) {
        this.world = world;

        this.orePatches.Add(new OrePatch(Block.coal, 0.07f, 0.2f));
    }

    public void generateChunk(Chunk c) {
        throw new System.Exception("Dont use this class!");

        if(c.chunkPos.x == 0 && c.chunkPos.y == 0 && c.chunkPos.z == 0) {
            this.generateSpawnChunk(c);
        } else {
            for(int x = 0; x < Chunk.SIZE; x++) {
                for(int y = 0; y < Chunk.SIZE; y++) {
                    for(int z = 0; z < Chunk.SIZE; z++) {
                        //float f = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.1f); // 0.015f); Good?
                        //if (f <= 0.95f) { //0.15F
                        //float f11 = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.25f);
                        //c.setBlock(x, y, z, f11 > 0 ? Block.stone : Block.dirt);
                        //c.setBlock(x, y, z, Block.air);
                        //} else {
                        //c.setBlock(x, y, z, Block.air);
                        //}

                        c.setBlock(x, y, z, Block.stone);

                        foreach (OrePatch patch in this.orePatches) {
                            for (int i = 0; i < patch.perChunk; i++) {
                                patch.generate(c, x, y, z);
                            }
                        }

                        //Pick the right stone
                        //float f1 = this.GetNoise((c.pos.x + x), (c.pos.y + y), (c.pos.z + z), 0.015f);
                        //float f2 = this.GetNoise((c.pos.z + z), (c.pos.y + y), (c.pos.x + x), 0.025f);
                        //c.setBlock(x, y, z, this.func1(f1, f2)); // this.getStone1(f1, f2));

                        //Crack
                        float f3 = this.GetNoise((c.pos.x + x) * 10, (c.pos.y + y), (c.pos.z + z), 0.007f);
                        float f4 = this.GetNoise((c.pos.x + x) * 10, (c.pos.y + y), (c.pos.z + z), 0.00025f);
                        bool flag = this.getCrack(f3, f4);
                        if(flag) {
                            //c.setBlock(x, y, z, Block.air);
                        }

                        //Ores
                        float oreNoise = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.05f);
                        if(oreNoise >= 0.8f) {
                            this.placeOre(c, x, y, z, Block.coal);
                        }
                        if (oreNoise < -0.95f) {
                            this.placeOre(c, x, y, z, Block.diamond);
                        }

                        oreNoise = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.07f);
                        if (oreNoise >= 0.9f) {
                            this.placeOre(c, x, y, z, Block.bronze);
                        }
                        if (oreNoise < -0.90f) {
                            this.placeOre(c, x, y, z, Block.emerald);
                        }


                        if (this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.025f) >= 0.937f) {
                            //c.setBlock(x, y, z, Block.gold);
                        }

                        //Ravine
                        //float rn = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.025f);
                        //if(rn > 0.25f) {
                        //    c.setBlock(x, y, z, Block.air);
                        //}
                    }
                }
            }
            //this.populateChunk(c);
            c.isDirty = true;
        }
        //this.checkForPopulation(c);
    }

    private void placeOre(Chunk c, int x, int y, int z, Block ore) {
        if(c.getBlock(x, y, z) == Block.stone) {
            c.setBlock(x, y, z, ore);
        }
    }

    private bool getCrack(float f1, float f2) {
        if (f1 < 0.75f) {
            return false;
        }
        if (f2 < 1f) {
            //return false;
        }

        return true;
    }

    private Block func0(float i, float j) {
        int[,] map = new int[4, 4] {
            { 1, 1, 1, 1 },
            { 1, 1, 1, 1 },
            { 1, 1, 1, 1 },
            { 1, 1, 1, 1 }
        };
        int k = 0;
        return Block.BLOCK_LIST[k];
    }

    private Block func1(float i, float j) {
        if(i < 0 && j < 0) {
            return Block.stone;
        }

        if(i < -0.5f) {
            return Block.ruby;
        }
        if(j < -0.5f) {
            return Block.coal;
        }

        if(j > -0.5f && j < 0.5f && i > 0.5f) {
            return Block.diamond;
        }
        if (j > -0.5f && j < 0.5f && i > 0) {
            return Block.iron;
        }

        if (i > 0.5f && j > 0.5f) {
            return Block.leaves;
        }

        return Block.wood;
    }

    private Block func2(float y) {
        if(y < -0.5f) {
            return Block.stone;
        }
        if(y < 0) {
            return Block.wood;
        }
        if(y < 0.5f) {
            return Block.leaves;
        }
        return Block.coal;
    }

    private Block getStone1(float f1, float f2) {
        if(f1 < -0.25f) {
            return Block.stone;
        }
        if(f2 < -0.25f) {
            return Block.dirt;
        }

        if(f1 >= 0.5f && f2 >= -0.25f) {
            return Block.emerald; //seems to live in the middle of leavess
        }

        if(f1 > 0.75f || f2 > 0.5f) {
            return Block.ruby;
        }
        if (f1 > 0 || f2 > 0.5f) {
            return Block.diamond;
        }

        return Block.coal; //seems t0 live on the edge of stone
    }

    private void checkForPopulation(Chunk c) {
        //make a five by five grid around the spawned chunk for easy lookup
        Chunk[,,] isGenerated = new Chunk[5, 5, 5];

        int i, j, k, i1, j1, k1;
        for (i = -2; i < 3; i++) {
            for (j = -2; j < 3; j++) {
                for (k = -2; k < 3; k++) {
                    BlockPos p = new BlockPos(c.pos.x + (i * 16), c.pos.y + (j * 16), c.pos.z + (k * 16));
                    //isGenerated[i + 2, j + 2, k + 2] = this.world.getChunk(p);
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
        c.isDirty = true;
    }

    private BlockPos randomChunkPos() {
        return new BlockPos(Random.Range(0, Chunk.SIZE), Random.Range(0, Chunk.SIZE), Random.Range(0, Chunk.SIZE));
    }

    public void populateChunk(Chunk c) {
        c.isPopulated = true;
        c.isDirty = true;
    }

    public float GetNoise(int x, int y, int z, float scale) {
        return (Noise.Generate(x * scale, y * scale, z * scale));
    }

    public int GetNoise(int x, int y, int z, float scale, int max) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}
