using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using VoxelEngine.Util;
using VoxelEngine.Level;

namespace VoxelEngine.Entities.Player.ChunkLoaders {

    public class ChunkLoader : MonoBehaviour {
        public bool infiniteY;

        protected World world;
        protected int maxBuildPerLoop = 1;
        protected ChunkPos previousOccupiedChunkPos = null;
        protected Queue<ChunkPos> buildQueue = new Queue<ChunkPos>();

        protected int loadRadius = 3;
        private int worldHeight = 3;

        public void Start() {
            this.world = this.GetComponent<EntityPlayer>().world;
            Stopwatch s = new Stopwatch();
            s.Start();
            this.loadChunks(this.getOccupiedChunkPos());
            print("Generation took " + s.Elapsed);
            this.buildChunks(10000);
            foreach(Chunk c in this.world.loadedChunks.Values) {
                c.Update();
            }
        }

        public void Update() {
            ChunkPos pos = this.getOccupiedChunkPos();
            if (this.previousOccupiedChunkPos == null ||
                pos.x != this.previousOccupiedChunkPos.x ||
                pos.y != this.previousOccupiedChunkPos.y ||
                pos.z != this.previousOccupiedChunkPos.z) {
                this.loadChunks(pos);
            }
            this.previousOccupiedChunkPos = pos;

            if (this.buildChunks(this.maxBuildPerLoop) == 0) {
                this.unloadChunks(pos);
            }
        }

        //Returns the position of the chunk the player is in
        protected ChunkPos getOccupiedChunkPos() {
            return new ChunkPos(
                Mathf.FloorToInt(this.transform.position.x / Chunk.SIZE),
                Mathf.FloorToInt(this.transform.position.y / Chunk.SIZE),
                Mathf.FloorToInt(this.transform.position.z / Chunk.SIZE));
        }

        //Returns true if the passed world coords are too far away from the player, used to check if a chunk should be unloaded
        protected bool toFarOnAxis(float occupiedChunkPos, float questionableChunkPos) {
            return (Mathf.Abs(occupiedChunkPos - questionableChunkPos) > this.loadRadius);
        }

        //Builds chunks from the list, building up to the passed value and returning the number built.
        protected int buildChunks(int max) {
            int builtChunks = 0;
            if (this.buildQueue.Count > 0) {
                while (this.buildQueue.Count > 0 && builtChunks < max) {
                    this.world.loadChunk(this.buildQueue.Dequeue());
                    builtChunks++;
                }
            }
            return builtChunks;
        }

        protected virtual void unloadChunks(ChunkPos occupiedChunkPos) {
            Queue<Chunk> removals = new Queue<Chunk>();
            foreach (Chunk c in this.world.loadedChunks.Values) {
                if(this.infiniteY) {
                    if (this.toFarOnAxis(occupiedChunkPos.x, c.chunkPos.x) ||
                        this.toFarOnAxis(occupiedChunkPos.y, c.chunkPos.y) ||
                        this.toFarOnAxis(occupiedChunkPos.z, c.chunkPos.z)) {
                        removals.Enqueue(c);
                    }
                } else {
                    if (this.toFarOnAxis(occupiedChunkPos.x, c.chunkPos.x) ||
                        this.toFarOnAxis(occupiedChunkPos.z, c.chunkPos.z)) {
                        removals.Enqueue(c);
                    }
                }
            }
            foreach (Chunk c in removals) {
                this.world.unloadChunk(c);
            }
        }

        //Adds all the chunks close to the player to the list of chunks to generate.
        protected virtual void loadChunks(ChunkPos occupiedChunkPos) {
            for (int x = -this.loadRadius; x < this.loadRadius + 1; x++) {
                for (int z = -this.loadRadius; z < this.loadRadius + 1; z++) {
                    ChunkPos pos = null;
                    Chunk chunk = null;
                    if(!this.infiniteY) {
                        for (int y = 0; y < this.worldHeight; y++) {
                            pos = new ChunkPos(x + occupiedChunkPos.x, y, z + occupiedChunkPos.z);
                            chunk = world.getChunk(pos);
                            if (chunk == null && !this.buildQueue.Contains(pos)) {
                                this.buildQueue.Enqueue(pos);
                            }
                        }
                    } else {
                        for (int y = -this.worldHeight; y < this.worldHeight + 1; y++) {
                            pos = new ChunkPos(x + occupiedChunkPos.x, y + occupiedChunkPos.y, z + occupiedChunkPos.z);
                            chunk = world.getChunk(pos);
                            if (chunk == null && !this.buildQueue.Contains(pos)) {
                                this.buildQueue.Enqueue(pos);
                            }
                        }
                    }
                }
            }
        }
    }
}