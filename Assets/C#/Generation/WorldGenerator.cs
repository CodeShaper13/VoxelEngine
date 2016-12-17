using SimplexNoise;
using UnityEngine;

public class WorldGenerator : WorldGeneratorBase {
    float stoneBaseHeight = 24; //was -24
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 4;

    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = -12;

    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

    float treeFrequency = 0.2f;
    int treeDensity = 3;

    public WorldGenerator(World world, long seed) : base(world, seed) {

    }

    public override void generateChunk(Chunk chunk) {
        base.generateChunk(chunk);

        for (int x = 0; x < Chunk.SIZE; x++) {
            for (int z = 0; z < Chunk.SIZE; z++) {
                chunk = generateColumn(chunk, x + chunk.pos.x, z + chunk.pos.z);
            }
        }
    }

    public override void populateChunk(Chunk chunk) {
        base.populateChunk(chunk);

        chunk.setBlock(15, 15, 15, Block.leaves);
        return;

        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.SIZE; x++) {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.SIZE; z++) {

                if (this.getNoise(x, 0, z, treeFrequency, 100) < treeDensity) {
                    //we should make a tree if we can, find the height

                    int y = chunk.pos.y - 1; //begin looking at the top bloock of the lower chunk
                    bool makeTree = true;
                    while(! ((world.getBlock(x, y, z).replaceable) && (world.getBlock(x, y + 1, z).replaceable))) {
                        y += 1;
                        if(y > chunk.pos.y + 15) {
                            makeTree = false;
                            break; //no spot for a tree
                        }
                    }
                    if(makeTree) {
                        for(int i= 0; i < 3; i++) {
                            world.setBlock(x, y + i, z, Block.wood);
                        }
                    }
                }
            }
        }
    }

    public Chunk generateColumn(Chunk chunk, int x, int z) {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += this.getNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight) {
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);
        }

        stoneHeight += this.getNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += this.getNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.pos.y; y < (chunk.pos.y + Chunk.SIZE); y++) {
            Block b = Block.air;
            //int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            if (y <= stoneHeight) {
                b = Block.stone;
            }
            else if (y < dirtHeight) {// && caveSize < caveChance) {
                b = Block.dirt;
                //if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity) {
                //    CreateTree(x, y + 1, z, chunk);
                //}
            } else if(y == dirtHeight) {
                b = Block.grass;
            }
            else {
                b = Block.air;
            }
            chunk.setBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, b);
        }
        return chunk;
    }

    public void setBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false) {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;
        chunk.setBlock(x, y, z, block);
    }
}
