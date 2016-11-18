using UnityEngine;

public class OrePatch {
    public Block oreBlock;
    public float genOdds;
    private int vainBounds;
    public int perChunk;

    public OrePatch(Block coal, float genOdds, int vainBounds, int perChunk) {
        this.oreBlock = coal;
        this.genOdds = genOdds;
        this.vainBounds = vainBounds;
        this.perChunk = perChunk;
    }

    public void generatePatch(Chunk c, BlockPos pos) {
        for(int i = -vainBounds; i < vainBounds + 1; i++) {
            for (int j = -vainBounds; j < vainBounds + 1; j++) {
                for (int k = -vainBounds; k < vainBounds + 1; k++) {
                    BlockPos pos1 = new BlockPos(c.pos.x + pos.x + i, c.pos.y + pos.y + j, c.pos.z + pos.z + k);
                    if(c.world.getBlock(pos1) != Block.air && Random.value < this.genOdds) {
                        c.world.setBlock(pos1, this.oreBlock, false);
                    }
                }
            }
        }
    }
}
