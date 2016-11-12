using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour {
    static BlockPos[] chunkLoadPositions = {   new BlockPos( 0, 0,  0), new BlockPos(-1, 0,  0), new BlockPos( 0, 0, -1), new BlockPos( 0, 0,  1), new BlockPos( 1, 0,  0),
                             new BlockPos(-1, 0, -1), new BlockPos(-1, 0,  1), new BlockPos( 1, 0, -1), new BlockPos( 1, 0,  1), new BlockPos(-2, 0,  0),
                             new BlockPos( 0, 0, -2), new BlockPos( 0, 0,  2), new BlockPos( 2, 0,  0), new BlockPos(-2, 0, -1), new BlockPos(-2, 0,  1),
                             new BlockPos(-1, 0, -2), new BlockPos(-1, 0,  2), new BlockPos( 1, 0, -2), new BlockPos( 1, 0,  2), new BlockPos( 2, 0, -1),
                             new BlockPos( 2, 0,  1), new BlockPos(-2, 0, -2), new BlockPos(-2, 0,  2), new BlockPos( 2, 0, -2), new BlockPos( 2, 0,  2),
                             new BlockPos(-3, 0,  0), new BlockPos( 0, 0, -3), new BlockPos( 0, 0,  3), new BlockPos( 3, 0,  0), new BlockPos(-3, 0, -1),
                             new BlockPos(-3, 0,  1), new BlockPos(-1, 0, -3), new BlockPos(-1, 0,  3), new BlockPos( 1, 0, -3), new BlockPos( 1, 0,  3),
                             new BlockPos( 3, 0, -1), new BlockPos( 3, 0,  1), new BlockPos(-3, 0, -2), new BlockPos(-3, 0,  2), new BlockPos(-2, 0, -3),
                             new BlockPos(-2, 0,  3), new BlockPos( 2, 0, -3), new BlockPos( 2, 0,  3), new BlockPos( 3, 0, -2), new BlockPos( 3, 0,  2),
                             new BlockPos(-4, 0,  0), new BlockPos( 0, 0, -4), new BlockPos( 0, 0,  4), new BlockPos( 4, 0,  0), new BlockPos(-4, 0, -1),
                             new BlockPos(-4, 0,  1), new BlockPos(-1, 0, -4), new BlockPos(-1, 0,  4), new BlockPos( 1, 0, -4), new BlockPos( 1, 0,  4),
                             new BlockPos( 4, 0, -1), new BlockPos( 4, 0,  1), new BlockPos(-3, 0, -3), new BlockPos(-3, 0,  3), new BlockPos( 3, 0, -3),
                             new BlockPos( 3, 0,  3), new BlockPos(-4, 0, -2), new BlockPos(-4, 0,  2), new BlockPos(-2, 0, -4), new BlockPos(-2, 0,  4),
                             new BlockPos( 2, 0, -4), new BlockPos( 2, 0,  4), new BlockPos( 4, 0, -2), new BlockPos( 4, 0,  2), new BlockPos(-5, 0,  0),
                             new BlockPos(-4, 0, -3), new BlockPos(-4, 0,  3), new BlockPos(-3, 0, -4), new BlockPos(-3, 0,  4), new BlockPos( 0, 0, -5),
                             new BlockPos( 0, 0,  5), new BlockPos( 3, 0, -4), new BlockPos( 3, 0,  4), new BlockPos( 4, 0, -3), new BlockPos( 4, 0,  3),
                             new BlockPos( 5, 0,  0), new BlockPos(-5, 0, -1), new BlockPos(-5, 0,  1), new BlockPos(-1, 0, -5), new BlockPos(-1, 0,  5),
                             new BlockPos( 1, 0, -5), new BlockPos( 1, 0,  5), new BlockPos( 5, 0, -1), new BlockPos( 5, 0,  1), new BlockPos(-5, 0, -2),
                             new BlockPos(-5, 0,  2), new BlockPos(-2, 0, -5), new BlockPos(-2, 0,  5), new BlockPos( 2, 0, -5), new BlockPos( 2, 0,  5),
                             new BlockPos( 5, 0, -2), new BlockPos( 5, 0,  2), new BlockPos(-4, 0, -4), new BlockPos(-4, 0,  4), new BlockPos( 4, 0, -4),
                             new BlockPos( 4, 0,  4), new BlockPos(-5, 0, -3), new BlockPos(-5, 0,  3), new BlockPos(-3, 0, -5), new BlockPos(-3, 0,  5),
                             new BlockPos( 3, 0, -5), new BlockPos( 3, 0,  5), new BlockPos( 5, 0, -3), new BlockPos( 5, 0,  3), new BlockPos(-6, 0,  0),
                             new BlockPos( 0, 0, -6), new BlockPos( 0, 0,  6), new BlockPos( 6, 0,  0), new BlockPos(-6, 0, -1), new BlockPos(-6, 0,  1),
                             new BlockPos(-1, 0, -6), new BlockPos(-1, 0,  6), new BlockPos( 1, 0, -6), new BlockPos( 1, 0,  6), new BlockPos( 6, 0, -1),
                             new BlockPos( 6, 0,  1), new BlockPos(-6, 0, -2), new BlockPos(-6, 0,  2), new BlockPos(-2, 0, -6), new BlockPos(-2, 0,  6),
                             new BlockPos( 2, 0, -6), new BlockPos( 2, 0,  6), new BlockPos( 6, 0, -2), new BlockPos( 6, 0,  2), new BlockPos(-5, 0, -4),
                             new BlockPos(-5, 0,  4), new BlockPos(-4, 0, -5), new BlockPos(-4, 0,  5), new BlockPos( 4, 0, -5), new BlockPos( 4, 0,  5),
                             new BlockPos( 5, 0, -4), new BlockPos( 5, 0,  4), new BlockPos(-6, 0, -3), new BlockPos(-6, 0,  3), new BlockPos(-3, 0, -6),
                             new BlockPos(-3, 0,  6), new BlockPos( 3, 0, -6), new BlockPos( 3, 0,  6), new BlockPos( 6, 0, -3), new BlockPos( 6, 0,  3),
                             new BlockPos(-7, 0,  0), new BlockPos( 0, 0, -7), new BlockPos( 0, 0,  7), new BlockPos( 7, 0,  0), new BlockPos(-7, 0, -1),
                             new BlockPos(-7, 0,  1), new BlockPos(-5, 0, -5), new BlockPos(-5, 0,  5), new BlockPos(-1, 0, -7), new BlockPos(-1, 0,  7),
                             new BlockPos( 1, 0, -7), new BlockPos( 1, 0,  7), new BlockPos( 5, 0, -5), new BlockPos( 5, 0,  5), new BlockPos( 7, 0, -1),
                             new BlockPos( 7, 0,  1), new BlockPos(-6, 0, -4), new BlockPos(-6, 0,  4), new BlockPos(-4, 0, -6), new BlockPos(-4, 0,  6),
                             new BlockPos( 4, 0, -6), new BlockPos( 4, 0,  6), new BlockPos( 6, 0, -4), new BlockPos( 6, 0,  4), new BlockPos(-7, 0, -2),
                             new BlockPos(-7, 0,  2), new BlockPos(-2, 0, -7), new BlockPos(-2, 0,  7), new BlockPos( 2, 0, -7), new BlockPos( 2, 0,  7),
                             new BlockPos( 7, 0, -2), new BlockPos( 7, 0,  2), new BlockPos(-7, 0, -3), new BlockPos(-7, 0,  3), new BlockPos(-3, 0, -7),
                             new BlockPos(-3, 0,  7), new BlockPos( 3, 0, -7), new BlockPos( 3, 0,  7), new BlockPos( 7, 0, -3), new BlockPos( 7, 0,  3),
                             new BlockPos(-6, 0, -5), new BlockPos(-6, 0,  5), new BlockPos(-5, 0, -6), new BlockPos(-5, 0,  6), new BlockPos( 5, 0, -6),
                             new BlockPos( 5, 0,  6), new BlockPos( 6, 0, -5), new BlockPos( 6, 0,  5) };

    public World world;
    private BlockPos previousWorldPos = new BlockPos(0, 0, 0);

    public List<BlockPos> updateList = new List<BlockPos>();
    private List<BlockPos> buildList = new List<BlockPos>();

    int timer = 0;

    void Start() {
        this.handleChunkLoading(this.getPlayerPos());
        print("Generation took " + (Time.realtimeSinceStartup));
    }

    // Update is called once per frame
    void Update() {
        BlockPos playerPos = this.getPlayerPos();
        if(playerPos.x != this.previousWorldPos.x || playerPos.y != this.previousWorldPos.y || playerPos.z != this.previousWorldPos.z) { this.handleChunkLoading(playerPos); }
        this.previousWorldPos = playerPos;

        //if(DeleteChunks()) { return; }
        //FindChunksToLoad();
        //LoadAndRenderChunks();
    }

    private BlockPos getPlayerPos() {
        return new BlockPos(Mathf.FloorToInt(this.transform.position.x / Chunk.SIZE) * Chunk.SIZE, Mathf.FloorToInt(this.transform.position.y / Chunk.SIZE) * Chunk.SIZE, Mathf.FloorToInt(this.transform.position.z / Chunk.SIZE) * Chunk.SIZE);
    }

    private void handleChunkLoading(BlockPos playerPos) {
        //foreach(Chunk c in this.world.loadedChunks.Values) {
        //    c.flag = false;
        //}

        int loadDistance = 1;

        for (int i = -loadDistance; i < loadDistance + 1; i++) {
            for (int j = -loadDistance; j < loadDistance + 1; j++) {
                for(int k = -4; k < 4; k++) {
                    int x = i * Chunk.SIZE + playerPos.x;
                    int y = k * Chunk.SIZE + playerPos.y;
                    int z = j * Chunk.SIZE + playerPos.z;

                    if (world.getChunk(x, y, z) == null) {
                        world.loadChunk(x, y, z);
                        world.getChunk(x, y, z).dirty = true;
                    }
                    //TODO read to chunk world.GetChunk(x, y, z).flag = true;
                }
            }
        }

        //Delete old chunks
        //List<WorldPos> chunksToDelete = new List<WorldPos>();
       // foreach (KeyValuePair<WorldPos, Chunk> c in this.world.loadedChunks) {
        //    if(c.Value.flag == false) {
        //        chunksToDelete.Add(c.Key);
        //    }
        //}
        //foreach (WorldPos pos in chunksToDelete) {
        //    this.world.DestroyChunk(pos.x, pos.y, pos.z);
        //}
    }

    //returns the next chunk column to load?
    private void FindChunksToLoad() {
        int i1 = 0;

        //Get the position of this gameobject to generate around
        BlockPos playerPos = this.getPlayerPos();

        //If there aren't already chunks to generate
        if (updateList.Count == 0) {
            for (int i = 0; i < chunkLoadPositions.Length; i++) {
                BlockPos newChunkPos = new BlockPos(chunkLoadPositions[i].x * Chunk.SIZE + playerPos.x, 0, chunkLoadPositions[i].z * Chunk.SIZE + playerPos.z);

                Chunk newChunk = world.getChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);

                //If the chunk already exists and (it's already
                //rendered or in queue to be rendered), stop and continue to the next spot in the list
                if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos))) {
                    continue;
                }

                //load a column of chunks in this position
                for (int y = -4; y < 4; y++) {
                    for (int x = newChunkPos.x - Chunk.SIZE; x <= newChunkPos.x + Chunk.SIZE; x += Chunk.SIZE) {
                        for (int z = newChunkPos.z - Chunk.SIZE; z <= newChunkPos.z + Chunk.SIZE; z += Chunk.SIZE) {
                            buildList.Add(new BlockPos(x, y * Chunk.SIZE, z));
                        }
                    }
                    updateList.Add(new BlockPos(newChunkPos.x, y * Chunk.SIZE, newChunkPos.z));
                }
                i1 += 1;
                //return;
                if(i1 > 1) {
                    return;
                }
            }
        }
    }

    void LoadAndRenderChunks() {
        if (buildList.Count != 0) {
            for (int i = 0; i < buildList.Count; i++) { //&& i < 8; i++) { //was 8, instead of 32
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }
            //If chunks were built return early
            return;
        }
        if (updateList.Count != 0) {
            Chunk chunk = world.getChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk.dirty = true;
            updateList.RemoveAt(0);
        }
    }

    void BuildChunk(BlockPos pos) {
        if (world.getChunk(pos.x, pos.y, pos.z) == null) {
            world.loadChunk(pos.x, pos.y, pos.z);
        }
    }

    bool DeleteChunks() {
        if (timer == 10) {
            var chunksToDelete = new List<BlockPos>();
            foreach (var chunk in world.loadedChunks) {
                float distance = Vector3.Distance(new Vector3(chunk.Value.pos.x, 0, chunk.Value.pos.z), new Vector3(transform.position.x, 0, transform.position.z));

                if (distance > 256) {
                    chunksToDelete.Add(chunk.Key);
                }
            }

            foreach (var chunk in chunksToDelete) {
                world.unloadChunk(chunk.x, chunk.y, chunk.z);
            }
            timer = 0;
            return true;
        }
        timer++;
        return false;
    }
}