#define DEBUG_LOAD

using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using VoxelEngine.Util;
using VoxelEngine.Level;
using VoxelEngine.Entities;

namespace VoxelEngine.ChunkLoaders {

    public class ChunkLoaderBase {
        public const int LOCKED_Y = 0;
        public const int INFINITE = 1;
        public const int REGION_DEBUG = 2;

        protected World world;
        protected EntityPlayer player;
        protected int maxBuildPerLoop = 1;
        protected ChunkPos previousOccupiedChunkPos = null;
        protected Queue<ChunkPos> buildQueue = new Queue<ChunkPos>();
        protected int loadRadius;

        public ChunkLoaderBase(World world, EntityPlayer player, int loadRadius) {
            this.world = world;
            this.player = player;
            this.loadRadius = loadRadius;

#if DEBUG_LOAD
            Stopwatch s = new Stopwatch();
            s.Start();
#endif
            this.loadChunks(this.getOccupiedChunkPos(this.player.transform.position));
#if DEBUG_LOAD
            s.Stop();
            UnityEngine.Debug.Log("Generation took " + s.Elapsed);
#endif
            this.generateChunks(10000);

#if DEBUG_LOAD
            //Force chunks to bake their meshes
            foreach (Chunk c in this.world.loadedChunks.Values) {
                c.Update();
            }
#endif
        }

        public virtual void updateChunkLoader() {
            ChunkPos pos = this.getOccupiedChunkPos(this.player.transform.position);
            if (this.previousOccupiedChunkPos == null ||
                pos.x != this.previousOccupiedChunkPos.x ||
                pos.y != this.previousOccupiedChunkPos.y ||
                pos.z != this.previousOccupiedChunkPos.z) {
                this.loadChunks(pos);
            }
            this.previousOccupiedChunkPos = pos;

            if (this.generateChunks(this.maxBuildPerLoop) == 0) {
                this.unloadChunks(pos);
            }
        }

        //Returns the position of the chunk the player is in
        protected ChunkPos getOccupiedChunkPos(Vector3 playerPos) {
            return new ChunkPos(
                Mathf.FloorToInt(playerPos.x / Chunk.SIZE),
                Mathf.FloorToInt(playerPos.y / Chunk.SIZE),
                Mathf.FloorToInt(playerPos.z / Chunk.SIZE));
        }

        //Returns the number of chunks generated
        protected int generateChunks(int max) {
            int builtChunks = 0;
            if (this.buildQueue.Count > 0) {
                while (this.buildQueue.Count > 0 && builtChunks < max) {
                    this.world.loadChunk(this.buildQueue.Dequeue());
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
            }
        }

        //Adds all the chunks close to the player to the list of chunks to generate.
        protected void loadChunks(ChunkPos occupiedChunkPos) {
            for (int x = -this.loadRadius; x < this.loadRadius + 1; x++) {
                for (int z = -this.loadRadius; z < this.loadRadius + 1; z++) {
                    this.loadYAxis(occupiedChunkPos, x, z);
                }
            }
        }

        protected bool toFar(float occupiedChunkPos, float questionableChunkPos) {
            return (Mathf.Abs(occupiedChunkPos - questionableChunkPos) > this.loadRadius);
        }

        /// <summary>
        /// Returns true if the passed chunk should be unloaded, false otherwise.
        /// </summary>
        protected virtual bool isOutOfBounds(ChunkPos occupiedChunkPos, Chunk chunk) {
            return false;
        }

        protected virtual void loadYAxis(ChunkPos occupiedChunkPos, int x, int z) {}
    }
}