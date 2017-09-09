using System;
using SimplexNoise;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;
using VoxelEngine.Generation.Island.Feature;

namespace VoxelEngine.Generation {

    public class WorldGeneratorHills : WorldGeneratorBase {
        float stoneBaseHeight = 24; //was -24
        float stoneBaseNoise = 0.05f;
        float stoneBaseNoiseHeight = 4;

        float stoneMountainHeight = 48;
        float stoneMountainFrequency = 0.008f;
        float stoneMinHeight = -12;

        float dirtBaseHeight = 1;
        float dirtNoise = 0.04f;
        float dirtNoiseHeight = 3;

        private FeatureTreeBasic tree;

        public WorldGeneratorHills(World world, int seed) : base(world, seed) {
            this.tree = new FeatureTreeBasic();
        }

        public override Vector3 getSpawnPoint(World world) {
            return new Vector3(0, 20, 0);
        }

        public override void generateChunk(Chunk chunk) {
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    chunk = makeColumn(chunk, x + chunk.worldPos.x, z + chunk.worldPos.z);
                }
            }
        }

        public override ChunkLoaderBase getChunkLoader(EntityPlayer player) {
            return new ChunkLoaderInfinite(player.world, player);
        }

        public override void populateChunk(Chunk chunk) {
            this.tree.generate(chunk, new System.Random(seed ^ chunk.chunkPos.GetHashCode()));

            /*
            chunk.setBlock(15, 15, 15, Block.wood);
            return;

            for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.SIZE; x++) {
                for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.SIZE; z++) {

                    if (this.getNoise(x, 0, z, treeFrequency, 100) < treeDensity) {
                        //we should make a tree if we can, find the height

                        int y = chunk.pos.y - 1; //begin looking at the top bloock of the lower chunk
                        bool makeTree = true;
                        while (!((world.getBlock(x, y, z).replaceable) && (world.getBlock(x, y + 1, z).replaceable))) {
                            y += 1;
                            if (y > chunk.pos.y + 15) {
                                makeTree = false;
                                break; //no spot for a tree
                            }
                        }
                        if (makeTree) {
                            for (int i = 0; i < 3; i++) {
                                world.setBlock(x, y + i, z, Block.wood);
                            }
                        }
                    }
                }
            }
            */
        }

        private Chunk makeColumn(Chunk chunk, int x, int z) {
            int stoneHeight = 0;
            stoneHeight += MathHelper.floor(this.getNoise(x, 0, z, 0.015f) * 10);

            //stoneHeight += this.getNoise(x, 0, z, stoneMountainFrequency, MathHelper.floor(stoneMountainHeight));

            //if (stoneHeight < stoneMinHeight) {
            //    stoneHeight = MathHelper.floor(stoneMinHeight);
            //}

            //stoneHeight += this.getNoise(x, 0, z, stoneBaseNoise, MathHelper.floor(stoneBaseNoiseHeight));

            int dirtHeight = stoneHeight + MathHelper.floor(dirtBaseHeight);
            dirtHeight += this.getNoise(x, 100, z, dirtNoise, MathHelper.floor(dirtNoiseHeight));

            for (int y = chunk.worldPos.y; y < (chunk.worldPos.y + Chunk.SIZE); y++) {
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
                }
                else if (y == dirtHeight) {
                    b = Block.grass;
                }
                else {
                    b = Block.air;
                }
                chunk.setBlock(x - chunk.worldPos.x, y - chunk.worldPos.y, z - chunk.worldPos.z, b);
            }
            return chunk;
        }

        public Chunk generateColumn(Chunk chunk, int x, int z) {
            int stoneHeight = MathHelper.floor(stoneBaseHeight);
            stoneHeight += this.getNoise(x, 0, z, stoneMountainFrequency, MathHelper.floor(stoneMountainHeight));

            if (stoneHeight < stoneMinHeight) {
                stoneHeight = MathHelper.floor(stoneMinHeight);
            }

            stoneHeight += this.getNoise(x, 0, z, stoneBaseNoise, MathHelper.floor(stoneBaseNoiseHeight));

            int dirtHeight = stoneHeight + MathHelper.floor(dirtBaseHeight);
            dirtHeight += this.getNoise(x, 100, z, dirtNoise, MathHelper.floor(dirtNoiseHeight));

            for (int y = chunk.worldPos.y; y < (chunk.worldPos.y + Chunk.SIZE); y++) {
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
                }
                else if (y == dirtHeight) {
                    b = Block.grass;
                }
                else {
                    b = Block.air;
                }
                chunk.setBlock(x - chunk.worldPos.x, y - chunk.worldPos.y, z - chunk.worldPos.z, b);
            }
            return chunk;
        }

        public void setBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false) {
            x -= chunk.worldPos.x;
            y -= chunk.worldPos.y;
            z -= chunk.worldPos.z;
            chunk.setBlock(x, y, z, block);
        }

        public float getNoise(int x, int y, int z, float scale) {
            return Noise.Generate(x * scale, y * scale, z * scale);
        }

        public int getNoise(int x, int y, int z, float scale, int max) {
            return MathHelper.floor((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}
