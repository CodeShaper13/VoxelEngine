using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Util;
using VoxelEngine.Level;
using VoxelEngine.Entities;
using System;

namespace VoxelEngine.ChunkLoaders {

    public class ChunkLoaderBase {
        public const int LOCKED_Y = 0;
        public const int INFINITE = 1;
        public const int REGION_DEBUG = 2;

        protected World world;
        protected EntityPlayer player;
        protected int maxBuildPerLoop = 4;
        protected ChunkPos previousOccupiedChunkPos;
        protected Queue<NewChunkInstructions> buildQueue;
        protected int loadRadius;
        protected Queue<Chunk> cachedUnusedChunks;

        public ChunkLoaderBase(World world, EntityPlayer player, int loadRadius) {
            this.world = world;
            this.player = player;
            this.loadRadius = loadRadius;
            this.buildQueue = new Queue<NewChunkInstructions>();
            this.cachedUnusedChunks = new Queue<Chunk>();

            this.loadChunks(this.getOccupiedChunkPos(this.player.transform.position));

            this.generateChunks(-1);

            foreach (Chunk c in this.world.loadedChunks.Values) {
                if(!c.isReadOnly) {
                    c.renderChunk();
                }
            }
        }

        public virtual void updateChunkLoader() {
            ChunkPos pos = this.getOccupiedChunkPos(this.player.transform.position);
            if (pos.x != this.previousOccupiedChunkPos.x || pos.y != this.previousOccupiedChunkPos.y || pos.z != this.previousOccupiedChunkPos.z) {
                this.loadChunks(pos);
            }
            this.previousOccupiedChunkPos = pos;

            this.unloadChunks(pos);
            this.generateChunks(this.maxBuildPerLoop);
        }

        /// <summary>
        /// Returns the position of the chunk the player is in.
        /// </summary>
        protected ChunkPos getOccupiedChunkPos(Vector3 playerPos) {
            return new ChunkPos(
                Mathf.FloorToInt(playerPos.x / Chunk.SIZE),
                Mathf.FloorToInt(playerPos.y / Chunk.SIZE),
                Mathf.FloorToInt(playerPos.z / Chunk.SIZE));
        }

        /// <summary>
        /// Generates up to the passed number of chunks, or infinite if -1.  Then returns the total number generated.
        /// </summary>
        protected int generateChunks(int max) {
            int builtChunks = 0;
            if (this.buildQueue.Count > 0) {
                while (this.buildQueue.Count > 0 && (builtChunks < max || max == -1)) {
                    NewChunkInstructions instructions = this.buildQueue.Dequeue();
                    Chunk chunk;
                    if(this.cachedUnusedChunks.Count > 0) {
                        chunk = this.cachedUnusedChunks.Dequeue();
                        chunk.gameObject.SetActive(true);
                    } else {
                        // The cache queue is empty, create a new chunk from scratch.
                        GameObject chunkGameObject = GameObject.Instantiate(References.list.chunkPrefab) as GameObject;
                        chunkGameObject.transform.parent = this.world.chunkWrapper;
                        chunk = chunkGameObject.GetComponent<Chunk>();
                    }

                    chunk.transform.position = instructions.toChunkVector();
                    chunk.isDirty = true;

                    this.world.loadChunk(chunk, instructions);

                    builtChunks++;
                }
            }
            return builtChunks;
        }

        protected void unloadChunks(ChunkPos occupiedChunkPos) {
            Queue<Chunk> removals = new Queue<Chunk>();
            foreach (Chunk chunk in this.world.loadedChunks.Values) {
                if(this.isOutOfBounds(occupiedChunkPos, chunk)) {
                    removals.Enqueue(chunk);
                }
            }
            foreach (Chunk c in removals) {
                this.world.unloadChunk(c);
                c.gameObject.SetActive(false);
                c.gameObject.name = "WAITING...";
                c.resetChunk();
                this.cachedUnusedChunks.Enqueue(c);
            }
        }

        /// <summary>
        /// Adds all the chunks close to the player to the list of chunks to generate.
        /// </summary>
        protected virtual void loadChunks(ChunkPos occupiedChunkPos) {
            int x, z;
            bool flagX, flagZ;
            for (x = -this.loadRadius; x <= this.loadRadius; x++) {
                for (z = -this.loadRadius; z <= this.loadRadius; z++) {
                    flagX = (x == loadRadius || x == -loadRadius);
                    flagZ = (z == loadRadius || z == -loadRadius);
                    if (!(flagX && flagZ)) {
                        this.loadYAxis(occupiedChunkPos, x, z, flagX || flagZ);
                    }
                }
            }
        }

        protected bool toFar(float occupiedChunkPos, float questionableChunkPos) {
            return (Math.Abs(occupiedChunkPos - questionableChunkPos) > this.loadRadius);
        }

        /// <summary>
        /// Returns true if the passed chunk should be unloaded, false otherwise.
        /// </summary>
        protected virtual bool isOutOfBounds(ChunkPos occupiedChunkPos, Chunk chunk) {
            return false;
        }

        protected virtual void loadYAxis(ChunkPos occupiedChunkPos, int x, int z, bool isReadOnly) {}
    }
}