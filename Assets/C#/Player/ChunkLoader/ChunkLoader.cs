using UnityEngine;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour {
    public World world;
    protected int maxBuiltPerLoop = 4;
    protected BlockPos previousOccupiedChunkPos = new BlockPos(0, 0, 0);
    protected List<BlockPos> buildList = new List<BlockPos>();

    protected int loadDistance = 3;

    private int worldHeight = 8;

    void Start() {
        print("Starting generation");
        this.loadChunks(this.getOccupiedChunkPos());
        print("Generation took " + (Time.realtimeSinceStartup));
        this.buildChunks(10000);
    }

    // Update is called once per frame
    void Update() {
        BlockPos p = this.getOccupiedChunkPos();
        if(p.x != this.previousOccupiedChunkPos.x || p.y != this.previousOccupiedChunkPos.y || p.z != this.previousOccupiedChunkPos.z) {
            this.loadChunks(p);
        }
        this.previousOccupiedChunkPos = p;

        if(this.buildChunks(this.maxBuiltPerLoop) == 0) {
            this.unloadChunks(p);
        }
    }

    protected BlockPos getOccupiedChunkPos() {
        return new BlockPos(Mathf.FloorToInt(this.transform.position.x / Chunk.SIZE), Mathf.FloorToInt(this.transform.position.y / Chunk.SIZE), Mathf.FloorToInt(this.transform.position.z / Chunk.SIZE));
    }

    //Returns true if the passed world coords are too far away from the player, used to find chunks to unload.
    protected bool toFarOnAxis(float occupiedChunkPos, float pos) {
        return (Mathf.Abs(occupiedChunkPos - pos) > (this.loadDistance * 16));
    }

    //Builds chunks from the list, building up to the passed value and returning the number built.
    protected int buildChunks(int max) {
        int builtChunks = 0;
        if(this.buildList.Count > 0) {
            for(int i = this.buildList.Count - 1; i >= 0 && builtChunks < max; i--) {
                this.world.loadChunk(this.buildList[i]).dirty = true;
                this.buildList.RemoveAt(i);
                builtChunks++;
            }
        }
        return builtChunks;
    }

    protected virtual void unloadChunks(BlockPos occupiedChunkPos) {
        occupiedChunkPos.x *= 16;
        occupiedChunkPos.y *= 16;
        occupiedChunkPos.z *= 16;

        List<BlockPos> removals = new List<BlockPos>();
        foreach (Chunk c in this.world.loadedChunks.Values) {
            BlockPos p = c.pos;
            if (this.toFarOnAxis(occupiedChunkPos.x, p.x) || this.toFarOnAxis(occupiedChunkPos.y, p.y) || this.toFarOnAxis(occupiedChunkPos.z, p.z)) {
                removals.Add(c.pos);
            }
        }
        foreach (BlockPos p in removals) {
            this.world.unloadChunk(p);
        }
    }

    protected virtual void loadChunks(BlockPos occupiedChunkPos) {
        occupiedChunkPos.x *= 16;
        occupiedChunkPos.z *= 16;
        
        //Add all the chunks close to the player to the list of chunks to generate.
        for (int x = -this.loadDistance; x < this.loadDistance + 1; x++) {
            for (int z = -this.loadDistance; z < this.loadDistance + 1; z++) {
                //Move along the y column
                for(int y = 0; y < this.worldHeight; y++) {
                    BlockPos p = new BlockPos(x * Chunk.SIZE + occupiedChunkPos.x, y * Chunk.SIZE, z * Chunk.SIZE + occupiedChunkPos.z);
                    Chunk c = world.getChunk(p);

                    if (c == null && !this.buildList.Contains(p)) {
                        this.buildList.Add(p);
                    }
                }
            }
        }
    }
}