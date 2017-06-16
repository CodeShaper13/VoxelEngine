using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Entities;
using VoxelEngine.Generation.Island.Feature;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class WorldGeneratorFlat : WorldGeneratorBase {

        private FeatureTreeBasic ftb;

        public WorldGeneratorFlat(World world, int seed) : base(world, seed) {
            this.ftb = new FeatureTreeBasic();
        }

        public override Vector3 getSpawnPoint(World world) {
            return new Vector3(0, 30, 0);
        }

        public override void generateChunk(Chunk chunk) {
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        chunk.setBlock(x, y, z, this.getBlockForHeight(y + chunk.worldPos.y));
                    }
                }
            }
        }

        public override ChunkLoaderBase getChunkLoader(EntityPlayer player) {
            return new ChunkLoaderInfinite(player.world, player);
        }

        public override void populateChunk(Chunk chunk) {
            ftb.generate(chunk, new System.Random(seed & chunk.chunkPos.GetHashCode()));
        }

        private Block getBlockForHeight(int y) {
            if (y < 16) {
                return Block.stone;
            } else if (y < 18) {
                return Block.dirt;
            } else if (y < 19) {
                return Block.grass;
            } else {
                return Block.air;
            }
        }
    }
}
