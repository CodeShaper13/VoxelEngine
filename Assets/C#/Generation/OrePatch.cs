using SimplexNoise;
using UnityEngine;

public class OrePatch {
    public Block oreBlock;
    private float noiseScale;
    private float threshold;

    public float genOdds;
    private int vainBounds;
    public int perChunk;

    public OrePatch(Block coal, float noiseScale, float threshold) {
        this.oreBlock = coal;
        this.noiseScale = noiseScale;
        this.threshold = threshold;
        //this.genOdds = genOdds;
        //this.vainBounds = vainBounds;
        //this.perChunk = perChunk;
    }

    public void generate(Chunk c, int x, int y, int z) {
        float f = Noise.Generate(x * this.noiseScale, y * this.noiseScale, z * this.noiseScale);
        if(f < this.threshold) {
            c.setBlock(x, y, z, this.oreBlock);
        }
    }

    public void generatePatch1(Chunk c, BlockPos pos) {
        for(int i = -vainBounds; i < vainBounds + 1; i++) {
            for (int j = -vainBounds; j < vainBounds + 1; j++) {
                for (int k = -vainBounds; k < vainBounds + 1; k++) {
                    BlockPos pos1 = new BlockPos(c.pos.x + pos.x + i, c.pos.y + pos.y + j, c.pos.z + pos.z + k);
                    //if (c.world.getBlock(pos1) != Block.air && Random.value < this.genOdds) {
                    if (Random.value < this.genOdds) {
                        c.world.setBlock(pos1, this.oreBlock, false);
                    }
                }
            }
        }
    }
}
