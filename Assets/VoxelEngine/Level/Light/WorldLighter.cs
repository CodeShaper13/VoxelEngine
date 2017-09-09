using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Level.Light {

    public class WorldLighter {

        private World world;

        private Queue<LightRemovalNode> removalQueue;
        private Queue<BlockPos> queue;
        private byte[] lightLookup;
        private int orginX;
        private int orginY;
        private int orginZ;
        private CachedChunk3x3 region;

        public WorldLighter(World world) {
            this.world = world;
            this.removalQueue = new Queue<LightRemovalNode>();
            this.queue = new Queue<BlockPos>();
        }

        private int getLight(int worldX, int worldY, int worldZ) {
            return region.getLight(worldX - this.orginX, worldY - this.orginY, worldZ - this.orginZ);
        }

        private Block getBlock(int worldX, int worldY, int worldZ) {
            return region.getBlock(worldX - this.orginX, worldY - this.orginY, worldZ - this.orginZ);
        }

        public void updateLighting(int newLight, int startX, int startY, int startZ) {
            Chunk c = this.world.getChunk(startX, startY, startZ);
            this.region = CachedChunk3x3.getNewRegion(this.world, c);

            this.orginX = c.worldPos.x;
            this.orginY = c.worldPos.y;
            this.orginZ = c.worldPos.z;
            /*

            // Make x, y, and z local within the chunk
            x -= c.worldPos.x;
            y -= c.worldPos.y;
            z -= c.worldPos.z;

            // Populate the lookup table.
            int i, j, k;
            for (i = 0; i <= 29; i++) {
                for (j = 0; j <= 29; j++) {
                    for (k = 0; k <= 29; k++) {
                        this.lightLookup[(j * Chunk.SIZE * Chunk.SIZE) + (k * Chunk.SIZE) + i] =
                            (byte)this.region.getLight(x + i - 14, y + j - 14, z + k - 14);
                    }
                }
            }
            */

            int x, y, z, neighborLevel, lightLevel;
            LightRemovalNode node;

            this.removalQueue.Enqueue(new LightRemovalNode(startX, startY, startZ, this.getLight(startX, startY, startZ)));
            this.world.setLight(startX, startY, startZ, 0);

            while (this.removalQueue.Count > 0) {
                node = this.removalQueue.Dequeue();
                x = node.x;
                y = node.y;
                z = node.z;
                lightLevel = node.lightLevel;

                // -X
                neighborLevel = this.getLight(x - 1, y, z);
                if (neighborLevel != 0 && neighborLevel < lightLevel) {
                    this.world.setLight(x - 1, y, z, 0);
                    this.removalQueue.Enqueue(new LightRemovalNode(x - 1, y, z, neighborLevel));
                }
                else if (neighborLevel >= lightLevel) {
                    this.queue.Enqueue(new BlockPos(x - 1, y, z));
                }

                // +X
                neighborLevel = this.getLight(x + 1, y, z);
                if (neighborLevel != 0 && neighborLevel < lightLevel) {
                    this.world.setLight(x + 1, y, z, 0);
                    this.removalQueue.Enqueue(new LightRemovalNode(x + 1, y, z, neighborLevel));
                }
                else if (neighborLevel >= lightLevel) {
                    this.queue.Enqueue(new BlockPos(x + 1, y, z));
                }

                // -Y
                neighborLevel = this.getLight(x, y - 1, z);
                if (neighborLevel != 0 && neighborLevel < lightLevel) {
                    this.world.setLight(x, y - 1, z, 0);
                    this.removalQueue.Enqueue(new LightRemovalNode(x, y - 1, z, neighborLevel));
                }
                else if (neighborLevel >= lightLevel) {
                    this.queue.Enqueue(new BlockPos(x, y - 1, z));
                }

                // +Y
                neighborLevel = this.getLight(x, y + 1, z);
                if (neighborLevel != 0 && neighborLevel < lightLevel) {
                    this.world.setLight(x, y + 1, z, 0);
                    this.removalQueue.Enqueue(new LightRemovalNode(x, y + 1, z, neighborLevel));
                }
                else if (neighborLevel >= lightLevel) {
                    this.queue.Enqueue(new BlockPos(x, y + 1, z));
                }

                // -Z
                neighborLevel = this.getLight(x, y, z - 1);
                if (neighborLevel != 0 && neighborLevel < lightLevel) {
                    this.world.setLight(x, y, z - 1, 0);
                    this.removalQueue.Enqueue(new LightRemovalNode(x, y, z - 1, neighborLevel));
                }
                else if (neighborLevel >= lightLevel) {
                    this.queue.Enqueue(new BlockPos(x, y, z - 1));
                }

                // +Z
                neighborLevel = this.getLight(x, y, z + 1);
                if (neighborLevel != 0 && neighborLevel < lightLevel) {
                    this.world.setLight(x, y, z + 1, 0);
                    this.removalQueue.Enqueue(new LightRemovalNode(x, y, z + 1, neighborLevel));
                }
                else if (neighborLevel >= lightLevel) {
                    this.queue.Enqueue(new BlockPos(x, y, z + 1));
                }
            }

            this.world.setLight(startX, startY, startZ, newLight);

            this.queue.Enqueue(new BlockPos(startX, startY, startZ));

            BlockPos pos;
            while (this.queue.Count > 0) {
                pos = this.queue.Dequeue();
                x = pos.x;
                y = pos.y;
                z = pos.z;

                lightLevel = this.getLight(x, y, z);

                if (!this.getBlock(x - 1, y, z).isSolid && this.getLight(x - 1, y, z) + 2 <= lightLevel) {
                    this.world.setLight(x - 1, y, z, lightLevel - 1);
                    this.queue.Enqueue(new BlockPos(x - 1, y, z));
                }
                if (!this.getBlock(x + 1, y, z).isSolid && this.getLight(x + 1, y, z) + 2 <= lightLevel) {
                    this.world.setLight(x + 1, y, z, lightLevel - 1);
                    this.queue.Enqueue(new BlockPos(x + 1, y, z));
                }
                if (!this.getBlock(x, y - 1, z).isSolid && this.getLight(x, y - 1, z) + 2 <= lightLevel) {
                    this.world.setLight(x, y - 1, z, lightLevel - 1);
                    this.queue.Enqueue(new BlockPos(x, y - 1, z));
                }
                if (!this.getBlock(x, y + 1, z).isSolid && this.getLight(x, y + 1, z) + 2 <= lightLevel) {
                    this.world.setLight(x, y + 1, z, lightLevel - 1);
                    this.queue.Enqueue(new BlockPos(x, y + 1, z));
                }
                if (!this.getBlock(x, y, z - 1).isSolid && this.getLight(x, y, z - 1) + 2 <= lightLevel) {
                    this.world.setLight(x, y, z - 1, lightLevel - 1);
                    this.queue.Enqueue(new BlockPos(x, y, z - 1));
                }
                if (!this.getBlock(x, y, z + 1).isSolid && this.getLight(x, y, z + 1) + 2 <= lightLevel) {
                    this.world.setLight(x, y, z + 1, lightLevel - 1);
                    this.queue.Enqueue(new BlockPos(x, y, z + 1));
                }
            }

            this.removalQueue.Clear();
            this.queue.Clear();
        }
    }
}
