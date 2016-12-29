using System;
using UnityEngine;

public class WorldGeneratorCaves : WorldGeneratorBase {

    public WorldGeneratorCaves(World world, long seed) : base(world, seed) {
    }

    public override Vector3 getSpawnPoint() {
        return new Vector3(8, 5, 13);
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

        if (chunk.chunkPos.x == 0 && chunk.chunkPos.z == 0) {
            if(chunk.chunkPos.y == 0) {
                this.generateStartRoom(chunk);
            } else if(chunk.chunkPos.y > 0) {
                this.generateShaft(chunk);
            }
        } else if(chunk.chunkPos.x == 0 && chunk.chunkPos.z == -1) {
            //TODO
        }
    }

    private void generateShaft(Chunk chunk) {
        for(int x = 6; x < 11; x++) {
            for(int z = 6; z < 11; z++) {
                if(x > 6 && x < 10 && z > 6 && z < 10) {
                    this.setColumn(chunk, x, z, x == 8 && z == 8 ? Block.cable : Block.air);
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
                    chunk.setBlock(x, y, z, Block.air);
                }
            }
        }

        this.generateShaft(chunk);
        for(int x = 2; x < 14; x++) {
            for(int z = 10; z < Chunk.SIZE; z++) {
                chunk.setBlock(x, 0, z, Block.mossyBrick);
            }
        }

        //for (int x = 0; x < Chunk.SIZE; x++) {
        //    for (int z = 0; z < Chunk.SIZE; z++) {
        //        for (int y = 0; y < Chunk.SIZE; y++) {
        //            chunk.setBlock(x, y, z, y == 0 ? Block.glorb : Block.air);
        //        }
        //    }
        //}

        //this.setColumn(chunk, 8, 8, Block.cable);
    }
}
