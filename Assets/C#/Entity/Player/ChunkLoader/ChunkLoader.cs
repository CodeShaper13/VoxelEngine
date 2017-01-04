using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class ChunkLoader : MonoBehaviour {
    protected World world;
    protected int maxBuiltPerLoop = 1;
    protected ChunkPos previousOccupiedChunkPos = null;
    protected Queue<ChunkPos> buildQueue = new Queue<ChunkPos>();

    protected int loadDistance = 1;

    private int worldHeight = 1;

    void Start() {
        this.world = this.GetComponent<EntityPlayer>().world;
        Stopwatch s = new Stopwatch();
        s.Start();
        this.loadChunks(this.getOccupiedChunkPos());
        print("Generation took " + s.Elapsed);
        this.buildChunks(10000);
    }

    // Update is called once per frame
    void Update() {
        ChunkPos p = this.getOccupiedChunkPos();
        if(this.previousOccupiedChunkPos == null || p.x != this.previousOccupiedChunkPos.x || p.z != this.previousOccupiedChunkPos.z) {
            this.loadChunks(p);
        }
        this.previousOccupiedChunkPos = p;

        if(this.buildChunks(this.maxBuiltPerLoop) == 0) {
            this.unloadChunks(p);
        }
    }

    protected ChunkPos getOccupiedChunkPos() {
        return new ChunkPos(Mathf.FloorToInt(this.transform.position.x / Chunk.SIZE), Mathf.FloorToInt(this.transform.position.y / Chunk.SIZE), Mathf.FloorToInt(this.transform.position.z / Chunk.SIZE));
    }

    //Returns true if the passed world coords are too far away from the player, used to find chunks to unload.
    protected bool toFarOnAxis(float occupiedChunkPos, float pos) {
        return (Mathf.Abs(occupiedChunkPos - pos) > (this.loadDistance));
    }

    //Builds chunks from the list, building up to the passed value and returning the number built.
    protected int buildChunks(int max) {
        int builtChunks = 0;
        if(this.buildQueue.Count > 0) {
            while(this.buildQueue.Count > 0 && builtChunks < max) {
                this.world.loadChunk(this.buildQueue.Dequeue());
                builtChunks++;
            }
        }
        return builtChunks;
    }

    protected virtual void unloadChunks(ChunkPos occupiedChunkPos) {
        List<ChunkPos> removals = new List<ChunkPos>();
        foreach (Chunk c in this.world.loadedChunks.Values) {
            if (this.toFarOnAxis(occupiedChunkPos.x, c.chunkPos.x) || this.toFarOnAxis(occupiedChunkPos.z, c.chunkPos.z)) {
                removals.Add(c.chunkPos);
            }
        }
        foreach (ChunkPos p in removals) {
            this.world.unloadChunk(p);
        }
    }

    protected virtual void loadChunks(ChunkPos occupiedChunkPos) {        
        //Add all the chunks close to the player to the list of chunks to generate.
        for (int x = -this.loadDistance; x < this.loadDistance + 1; x++) {
            for (int z = -this.loadDistance; z < this.loadDistance + 1; z++) {
                for(int y = 0; y < this.worldHeight; y++) {
                    ChunkPos pos = new ChunkPos(x + occupiedChunkPos.x, y, z + occupiedChunkPos.z);
                    Chunk chunk = world.getChunk(pos);
                    if (chunk == null && !this.buildQueue.Contains(pos)) {
                        this.buildQueue.Enqueue(pos);
                    }
                }
            }
        }
    }
}