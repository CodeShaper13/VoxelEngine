using SimplexNoise;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class WorldGeneratorCaves : WorldGeneratorBase {

        public WorldGeneratorCaves(World world, long seed) : base(world, seed) {
        }

        public override Vector3 getSpawnPoint() {
            return new Vector3(8, 5, 13);
        }

        public override void generateChunk(Chunk c) {
            base.generateChunk(c);

            if(c.chunkPos.x == 0 && c.chunkPos.y == 0 && c.chunkPos.z == 0) {
                this.generateStartRoom(c);
                return;
            }

            bool inCrackChunk = c.chunkPos.y % 2 != 0;
            byte closerToOrgin = c.world.worldData.stoneLayers.getStone(Mathf.FloorToInt(c.chunkPos.y / 2));
            byte fartherFromOrgin = c.world.worldData.stoneLayers.getStone(Mathf.FloorToInt(c.chunkPos.y / 2 + (c.chunkPos.y < 0 ? -1 : 1)));

            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        Block block = Block.stone;
                        byte meta = (c.chunkPos.y < 0 && inCrackChunk) ? fartherFromOrgin : closerToOrgin;

                        if (inCrackChunk) {
                            float crackNoise = getNoise(c.pos.x + x, c.pos.y, c.pos.z + z, 0.05f);
                            if (y > (Chunk.SIZE / 2) + (crackNoise * 2)) {
                                meta = c.chunkPos.y > 0 ? fartherFromOrgin : closerToOrgin;
                            }
                        }

                        if (this.getNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.05f) >= 0.85f) {
                            if(Random.value < 0.75f) {
                                block = Block.coalOre;
                            }
                        }

                        if (this.getNoise(c.pos.x + x, (c.pos.y + y) * 2, c.pos.z + z, 0.01f) >= 0.85f) {
                            block = Block.lava;
                        }

                        c.setBlock(x, y, z, block);
                        c.setMeta(x, y, z, meta);
                    }
                }
            }

            c.setBlock(Random.Range(0, Chunk.SIZE), Random.Range(0, Chunk.SIZE), Random.Range(0, Chunk.SIZE), Block.uraniumOre);

            this.generateRubyPatch(c, Random.Range(0, Chunk.SIZE - 2), Random.Range(0, Chunk.SIZE - 2), Random.Range(0, Chunk.SIZE - 2));
        }

        private void generateRubyPatch(Chunk c, int x, int y, int z) {
            for(int i = 0; i < 3; i++) {
                for(int j = 0; j < 3; j++) {
                    for(int k = 0; k < 3; k++) {
                        if(Random.value < 0.35f) {
                            c.setBlock(x + i, y + j, z + k, Block.rubyOre);
                        }
                    }
                }
            }
        }

        private void generateShaft(Chunk chunk) {
            for (int x = 6; x < 11; x++) {
                for (int z = 6; z < 11; z++) {
                    if (x > 6 && x < 10 && z > 6 && z < 10) {
                        this.setColumn(chunk, x, z, x == 8 && z == 8 ? Block.cable : Block.air);
                    }
                    else {
                        this.setColumn(chunk, x, z, Block.mossyBrick);
                    }
                }
            }
        }

        private void setColumn(Chunk chunk, int x, int z, Block block) {
            for (int y = 0; y < Chunk.SIZE; y++) {
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
            for (int x = 2; x < 14; x++) {
                for (int z = 10; z < Chunk.SIZE; z++) {
                    chunk.setBlock(x, 0, z, Block.mossyBrick);
                }
            }
        }

        public float getNoise(int x, int y, int z, float scale) {
            return (Noise.Generate(x * scale, y * scale, z * scale));
        }

        public int GetNoise(int x, int y, int z, float scale, int max) {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}
