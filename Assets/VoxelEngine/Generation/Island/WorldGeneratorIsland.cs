using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Entities;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Generation.Island {

    public class WorldGeneratorIsland : WorldGeneratorBase {

        private Collider heightmapCollider;

        public WorldGeneratorIsland(World world, int seed) : base(world, seed) {
            this.heightmapCollider = GameObject.Find("DebugHeightMesh").GetComponent<Collider>();
        }

        public override void generateChunk(Chunk chunk) {
            RaycastHit hit;
            for (int x = 0; x < Chunk.SIZE; x++) {
                for(int z = 0; z < Chunk.SIZE; z++) {
                    int height = 0;
                    if(this.heightmapCollider.Raycast(new Ray(new Vector3(chunk.worldPos.x + x, 1000, chunk.worldPos.z + z), Vector3.down), out hit, 10000f)) {
                        height = (int)hit.point.y;
                    } else {
                        height = 1;
                    }

                    int i = chunk.worldPos.y;

                    for(int y = 0; y < Chunk.SIZE; y++) {
                        Block b;
                        int j = i + y;
                        if (j < height - 3) {
                            b = Block.stone;
                        } else if(j < height) {
                            b = Block.dirt;
                        } else if(j == height) {
                            b = Block.grass;
                        } else {
                            b = Block.air;
                        }


                        chunk.setBlock(x, y, z, b);
                    }
                }
            }
        }

        public override ChunkLoaderBase getChunkLoader(EntityPlayer player) {
            return new ChunkLoaderInfinite(player.world, player);
        }

        public override Vector3 getSpawnPoint(World world) {
            RaycastHit hit;
            Physics.Raycast(new Ray(new Vector3(0, 100000, 0), Vector3.down), out hit, 1000000, Layers.BLOCKS);
            return hit.point + Vector3.up * 10;
        }
    }
}
