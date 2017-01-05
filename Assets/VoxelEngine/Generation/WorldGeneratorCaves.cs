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

            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {

                        //float f = this.getNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.05f); //lower the scale, the more stretched out everhting is
                        //Block b = Block.stone;
                        //if(f < -0.5f) {
                        //    b = Block.coalOre;
                        //} else if(f < 0) {
                        //    b = Block.glorb;
                        //} else if(f < 0.5f) {
                        //    b = Block.goldOre;
                        //} else {
                        //    b = Block.ironGrate;
                        //}

                        //c.setBlock(x, y, z, b);
                        float f1 = this.getNoise((c.pos.x + x), (c.pos.y + y), (c.pos.z + z) * 10, 0.075f);
                        float f2 = this.getNoise((c.pos.x + x) * 2, (c.pos.y + y) * 2, (c.pos.z + z) * 2, 0.1f);
                        byte meta = 0;
                        if (f1 < -0.5f) {
                            meta = 0; //red
                        }
                        else if (f1 < 0) {
                            meta = 1; //blue
                        }
                        else if (f1 < 0.5f) {
                            meta = 2; //yellow
                        }
                        else {
                            meta = 3; //green
                        }

                        Block b = Block.stone;
                        if (f2 < -0.5f) {
                            b = Block.stone; //red
                        }
                        else if (f2 < 0) {
                            b = Block.air; // bronzeOre; //blue
                        }
                        else if (f2 < 0.5f) {
                            b = Block.ironOre; //yellow
                        }
                        else {
                            b = Block.goldOre; //green
                        }

                        c.setBlock(x, y, z, b);
                        c.setMeta(x, y, z, meta);
                        //Crack
                        //float f3 = this.getNoise((c.pos.x + x) * 10, (c.pos.y + y), (c.pos.z + z), 0.007f);
                        //float f4 = this.getNoise((c.pos.x + x) * 10, (c.pos.y + y), (c.pos.z + z), 0.00025f);
                        //bool flag = this.getCrack(f3, f4);
                        //if(flag) {
                        //       c.setBlock(x, y, z, Block.air);
                        //}

                        //float rn = this.GetNoise(c.pos.x + x, c.pos.y + y, c.pos.z + z, 0.025f);
                        //if(rn > 0.25f) {
                        //    c.setBlock(x, y, z, Block.air);
                        //}
                    }
                }
            }

            //if (chunk.chunkPos.x == 0 && chunk.chunkPos.z == 0) {
            //    if(chunk.chunkPos.y == 0) {
            //        this.generateStartRoom(chunk);
            //    } else if(chunk.chunkPos.y > 0) {
            //        this.generateShaft(chunk);
            //    }
            //} else if(chunk.chunkPos.x == 0 && chunk.chunkPos.z == -1) {
            //    //TODO
            //}
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

            //for (int x = 0; x < Chunk.SIZE; x++) {
            //    for (int z = 0; z < Chunk.SIZE; z++) {
            //        for (int y = 0; y < Chunk.SIZE; y++) {
            //            chunk.setBlock(x, y, z, y == 0 ? Block.glorb : Block.air);
            //        }
            //    }
            //}

            //this.setColumn(chunk, 8, 8, Block.cable);
        }

        public float getNoise(int x, int y, int z, float scale) {
            return (Noise.Generate(x * scale, y * scale, z * scale));
        }

        public int GetNoise(int x, int y, int z, float scale, int max) {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}
