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

        protected World world;
        protected Queue<NewChunkInstructions> buildQueue;
        protected int loadRadius;

        private int maxBuildPerLoop = 4;
        private EntityPlayer player;
        private ChunkPos previousOccupiedChunkPos;
        private Queue<Chunk> cachedUnusedChunks;

        public ChunkLoaderBase(World world, EntityPlayer player, int loadRadius) {
            this.world = world;
            this.player = player;
            this.loadRadius = loadRadius;
            this.buildQueue = new Queue<NewChunkInstructions>();
            this.cachedUnusedChunks = new Queue<Chunk>();

            this.loadChunks(this.getOccupiedChunkPos(this.player.transform.position));

            // Generate all of the chunks around the player.
            this.generateChunksFromInstructions(-1);

            // Render all of the chunks right away.
            foreach (Chunk c in this.world.loadedChunks.Values) {
                if(!c.isReadOnly) {
                    c.renderChunk();
                }
            }
        }

        /// <summary>
        /// Called by the player to update the chunk loader.
        /// </summary>
        public void updateChunkLoader() {
            ChunkPos playerPos = this.getOccupiedChunkPos(this.player.transform.position);
            if(!(playerPos.Equals(this.previousOccupiedChunkPos))) {
                this.loadChunks(playerPos);
            }
            this.previousOccupiedChunkPos = playerPos;

            this.unloadChunks(playerPos);
            this.generateChunksFromInstructions(this.maxBuildPerLoop);
        }

        /// <summary>
        /// Unloads all chunks that are out of bounds.
        /// </summary>
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
        /// Creates new chunk instructions for all the chunks around the player and adds them to the list.
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

        /// <summary>
        /// Returns the position of the chunk the player is in.
        /// </summary>
        private ChunkPos getOccupiedChunkPos(Vector3 playerPos) {
            return new ChunkPos(
                MathHelper.floor(playerPos.x / Chunk.SIZE),
                MathHelper.floor(playerPos.y / Chunk.SIZE),
                MathHelper.floor(playerPos.z / Chunk.SIZE));
        }

        /// <summary>
        /// Generates up to the passed number of chunks, or infinite if -1.  Then returns the total number generated.
        /// </summary>
        private int generateChunksFromInstructions(int max) {
            int builtChunks = 0;
            while (this.buildQueue.Count > 0 && (builtChunks < max || max == -1)) {
                NewChunkInstructions instructions = this.buildQueue.Dequeue();
                Chunk chunk;
                if (this.cachedUnusedChunks.Count > 0) {
                    // Pull out and reset an old chunk.
                    chunk = this.cachedUnusedChunks.Dequeue();
                    chunk.gameObject.SetActive(true);
                    chunk.filter.mesh.Clear(); // TODO Large performance hit?
                }
                else {
                    // The cache queue is empty, create a new chunk from scratch.
                    chunk = GameObject.Instantiate(References.list.chunkPrefab, this.world.chunkWrapper).GetComponent<Chunk>();
                }

                chunk.transform.position = instructions.toChunkVector();
                chunk.isDirty = true;

                this.world.loadChunk(chunk, instructions);

                builtChunks++;
            }
            return builtChunks;
        }
    }
}