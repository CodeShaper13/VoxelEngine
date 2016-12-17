using UnityEngine;

public class WorldGeneratorFlat : WorldGeneratorBase {

    public WorldGeneratorFlat(World world, long seed) : base(world, seed) {

    }

    public override void generateChunk(Chunk chunk) {
        base.generateChunk(chunk);

        for (int x = 0; x < Chunk.SIZE; x++) {
            for (int z = 0; z < Chunk.SIZE; z++) {
                for (int y = 0; y < Chunk.SIZE; y++) {
                    chunk.setBlock(x, y, z, this.getBlockForHeight(y + chunk.pos.y));
                }
            }
        }
    }

    private Block getBlockForHeight(int y) {
        if(y < 16) {
            return Block.stone;
        } else if(y < 18) {
            return Block.dirt;
        } else if(y < 19) {
            return Random.Range(0, 256) == 0 ? Block.grass : Block.dirt;
        } else {
            return Block.air;
        }
    }
}
