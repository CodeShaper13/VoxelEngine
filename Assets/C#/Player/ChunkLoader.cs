using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour {
    public World world;
    private int maxBuiltPerLoop = 4;
    private BlockPos previousOccupiedChunkPos = new BlockPos(0, 0, 0);
    private List<BlockPos> buildList = new List<BlockPos>();
    private int loadDistance = 2;

    void Start() {
        this.loadNewChunks(this.getOccupiedChunkPos());
        print("Generation took " + (Time.realtimeSinceStartup));
        this.buildChunks(1000000);
    }

    // Update is called once per frame
    void Update() {
        BlockPos p = this.getOccupiedChunkPos();
        if(p.x != this.previousOccupiedChunkPos.x || p.y != this.previousOccupiedChunkPos.y || p.z != this.previousOccupiedChunkPos.z) {
            this.loadNewChunks(p);
        }
        this.previousOccupiedChunkPos = p;

        if(this.buildChunks(this.maxBuiltPerLoop) == 0) {
            this.unloadDistantChunks(p);
        }
    }

    private BlockPos getOccupiedChunkPos() {
        return new BlockPos(Mathf.FloorToInt(this.transform.position.x / Chunk.SIZE), Mathf.FloorToInt(this.transform.position.y / Chunk.SIZE), Mathf.FloorToInt(this.transform.position.z / Chunk.SIZE));
    }

    //Returns true if the passed world coords are too far away from the player, used to find chunks to unload.
    private bool toFarOnAxis(float occupiedChunkPos, float pos) {
        return (Mathf.Abs(occupiedChunkPos - pos) > (this.loadDistance * 16));
    }

    //Builds chunks from the list, building up to the passed value and returning the number built.
    private int buildChunks(int max) {
        int builtChunks = 0;
        if(this.buildList.Count > 0) {
            for(int i = this.buildList.Count - 1; i >= 0 && builtChunks < max; i--) {
                this.world.loadChunk(this.buildList[i]).dirty = true;
                this.buildList.RemoveAt(i);
                builtChunks++;
            }
            //print("Built " + builtChunks);
        }
        return builtChunks;
    }

    private void unloadDistantChunks(BlockPos occupiedChunkPos) {
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

    private void loadNewChunks(BlockPos occupiedChunkPos) {
        occupiedChunkPos.x *= 16;
        occupiedChunkPos.y *= 16;
        occupiedChunkPos.z *= 16;
        
        //Add all the chunks close to the player to the list of chunks to generate.
        for (int i = -this.loadDistance; i < this.loadDistance + 1; i++) {
            for (int j = -this.loadDistance; j < this.loadDistance + 1; j++) {
                for(int k = -this.loadDistance; k < this.loadDistance + 1; k++) {
                    int x = i * Chunk.SIZE + occupiedChunkPos.x;
                    int y = k * Chunk.SIZE + occupiedChunkPos.y;
                    int z = j * Chunk.SIZE + occupiedChunkPos.z;

                    Chunk c = world.getChunk(x, y, z);
                    BlockPos p = new BlockPos(x, y, z);
                    if (c == null && !this.buildList.Contains(p)) {
                        this.buildList.Add(p);
                    }
                }
            }
        }
    }
}