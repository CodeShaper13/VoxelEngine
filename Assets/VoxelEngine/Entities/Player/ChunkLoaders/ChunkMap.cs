using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Entities.Player.ChunkLoaders {

    public class ChunkMap {
        public int radius;
        public int size;
        public int height;

        private Chunk[,,] chunkArray;

        public ChunkMap(int radius, int height) {
            this.radius = radius;
            this.size = this.radius * this.radius + 1;
            this.height = height;
            this.chunkArray = new Chunk[this.size, height, this.size];
        }

        public Chunk getChunk(int x, int y, int z) {
            int x1 = x + this.radius;
            int z1 = z + this.radius;
            if (this.inBounds(x1) && this.inBounds(z1) && y >= 0 && y < this.height) {
                return this.chunkArray[x1, y, z1];
            }
            else {
                return null;
            }
        }

        public void setChunk(int x, int y, int z, Chunk chunk) {
            int x1 = x + this.radius;
            int z1 = z + this.radius;
            this.chunkArray[x1, y, z1] = chunk;
        }

        private bool inBounds(int i) {
            return i >= 0 && i < this.size;
        }

        public List<GameObject> shiftMap(int shiftX, int shiftZ) {
            List<GameObject> garbageChunks = new List<GameObject>();

            Chunk[,,] copyMap = new Chunk[this.size, height, this.size];

            for (int x = -this.radius; x < this.radius + 1; x++) {
                for (int z = -this.radius; z < this.radius + 1; z++) {
                    for (int y = 0; y < this.height; y++) {

                        Chunk chunk = this.getChunk(x, y, z);
                        ChunkPos shiftPos = new ChunkPos(x + (shiftX), y, z + (shiftZ));

                        //Debug.Log("/ Moving chunk " + new ChunkPos(x, y, z) + " -> " + shiftPos);
                        if (shiftPos.x < -this.radius || shiftPos.x > this.radius || shiftPos.z < -this.radius || shiftPos.z > this.radius) {
                            //Debug.Log("| Chunk is out of bounds, Deleting " + chunk.gameObject.name);
                            garbageChunks.Add(chunk.gameObject);
                            //GameObject.Destroy(chunk.gameObject);
                            //delete chunk
                        }
                        else {
                            //Debug.Log("| Chunk is ok " + chunk.gameObject.name);
                            copyMap[shiftPos.x + this.radius, y, shiftPos.z + radius] = chunk;
                        }
                    }
                }
            }
            this.chunkArray = copyMap;
            return garbageChunks;
        }

        public void updateAllChunks() {
            for (int x = 0; x < this.size; x++) {
                for (int z = 0; z < this.size; z++) {
                    for (int y = 0; y < this.height; y++) {
                        if (this.chunkArray[x, y, z] != null) {
                            this.chunkArray[x, y, z].updateChunk();
                        }
                    }
                }
            }
        }
    }
}
