using System;
using UnityEngine;

public class WorldGeneratorCaves : WorldGeneratorBase {

    public WorldGeneratorCaves(World world, long seed) : base(world, seed) {
    }

    public override Vector3 getSpawnPoint() {
        return new Vector3(10, 6, 8);
    }

    public override void generateChunk(Chunk chunk) {
        base.generateChunk(chunk);

        for (int x = 0; x < Chunk.SIZE; x++) {
            for (int z = 0; z < Chunk.SIZE; z++) {
                for (int y = 0; y < Chunk.SIZE; y++) {
                    chunk.setBlock(x, y, z, Block.stone);
                }
            }
        }

        if (chunk.chunkPos.x == 0 && chunk.pos.z == 0) {
            if(chunk.chunkPos.y == 0) {
                this.generateStartRoom(chunk);
            } else if(chunk.chunkPos.y > 0) {
                this.generateShaft(chunk);
            }
        }

        if(chunk.chunkPos.y >= 0) {
            for(int y = chunk.pos.y == 0 ? 1 : 0; y < Chunk.SIZE; y++) {
                chunk.setBlock(8, y, 8, Block.cable);
            }
        }
    }

    private void generateShaft(Chunk chunk) {
        //this.setColumn(chunk, 8, 8, Block.cable);

        for(int x = 6; x < 11; x++) {
            for(int z = 6; z < 11; z++) {
                if(x > 6 && x < 10 && z > 6 && z < 10 && x != 8 && z != 8) {
                    this.setColumn(chunk, x, z, Block.air);
                } else {
                    this.setColumn(chunk, x, z, Block.mossyBrick);
                }
            }
        }
    }

    private void setColumn(Chunk chunk, int x, int z, Block block) {
        for(int y  = 0; y < Chunk.SIZE; y++) {
            chunk.setBlock(x, y, z, block);
        }
    }

    private void generateStartRoom(Chunk chunk) {
        for (int x = 0; x < Chunk.SIZE; x++) {
            for (int z = 0; z < Chunk.SIZE; z++) {
                for (int y = 0; y < Chunk.SIZE; y++) {
                    chunk.setBlock(x, y, z, y == 0 ? Block.glorb : Block.air);
                }
            }
        }
    }
}
