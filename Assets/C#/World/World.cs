using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour {
    public Dictionary<ChunkPos, Chunk> loadedChunks = new Dictionary<ChunkPos, Chunk>();

    public string worldName;
    public long seed;
    public WorldGeneratorBase generator;

    public GameObject chunkPrefab;
    public GameObject itemPrefab;

    private Transform chunkWrapper;
    private Transform itemWrapper;

    void Awake() {
        this.worldName = "world";
        this.generator = new WorldGeneratorFlat(this, this.seed);

        this.chunkWrapper = new GameObject().transform;
        this.chunkWrapper.parent = this.transform;
        this.chunkWrapper.name = "CHUNKS";
        this.itemWrapper = new GameObject().transform;
        this.itemWrapper.parent = this.transform;
        this.itemWrapper.name = "ITEMS";
    }

    void LateUpdate() {
        foreach (Chunk c in this.loadedChunks.Values) {
            c.updateChunk();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            this.saveWorld();
        }
    }

    public void spawnItem(ItemStack stack, Vector3 pos, Quaternion rot) {
        GameObject g = GameObject.Instantiate(this.itemPrefab);
        g.transform.parent = this.itemWrapper;
        g.transform.rotation = rot;
        g.transform.position = pos;
        EntityItem i = g.GetComponent<EntityItem>();
        i.stack = stack;
        i.initRendering();
    }

    //Loads a new chunk, loading it if the save exists, otherwise we generate a new one.
    public Chunk loadChunk(ChunkPos pos) {
        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(pos.x * 16, pos.y * 16, pos.z * 16), Quaternion.identity) as GameObject;
        newChunkObject.transform.parent = this.chunkWrapper;
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();
        newChunk.initChunk(this, pos);

        loadedChunks.Add(pos, newChunk);

        //if(!Serialization.loadChunk(newChunk)) {
            this.generator.generateChunk(newChunk);
        //}
        return newChunk;
    }

    //Unloads a chunk, removing references and saving it
    public void unloadChunk(ChunkPos pos) {
        Chunk chunk = null;
        if (loadedChunks.TryGetValue(pos, out chunk)) {
            //Serialization.SaveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            loadedChunks.Remove(pos);
        }
    }

    public Chunk getChunk(ChunkPos pos) {
        Chunk chunk = null;
        loadedChunks.TryGetValue(pos, out chunk);
        return chunk;
    }

    //Returns the chunk at x, y, z (World coordinates)
    public Chunk getChunk(int x, int y, int z) {
        int x1 = Mathf.FloorToInt(x / (float)Chunk.SIZE);
        int y1 = Mathf.FloorToInt(y / (float)Chunk.SIZE);
        int z1 = Mathf.FloorToInt(z / (float)Chunk.SIZE);
        ChunkPos c = new ChunkPos(x1, y1, z1);
        return this.getChunk(c);
    }

    public Block getBlock(BlockPos pos) {
        return this.getBlock(pos.x, pos.y, pos.z);
    }

    public Block getBlock(int x, int y, int z) {
        Chunk chunk = this.getChunk(x, y, z);
        if (chunk != null) {
            return chunk.getBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z);
        }
        else {
            return Block.air;
        }
    }

    public void setBlock(BlockPos pos, Block block, bool runUpdate = true) {
        this.setBlock(pos.x, pos.y, pos.z, block, runUpdate);
    }

    public void setBlock(int x, int y, int z, Block block, bool runUpdate = true) {
        Chunk chunk = this.getChunk(x, y, z);
        if (chunk != null) {
            chunk.setBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);

            if(runUpdate) {
                BlockPos p = new BlockPos(x, y, z);
                foreach (Direction d in Direction.all) {
                    BlockPos p1 = p.move(d);
                    this.getBlock(p1).onNeighborChange(p, d);
                    chunk = this.getChunk(p1.x, p1.y, p1.z);
                    if(chunk != null) {
                        chunk.isDirty = true;
                    }
                }
            }
            //this.UpdateIfEqual(x - chunk.pos.x, 0,              new BlockPos(x - 1, y, z));
            //this.UpdateIfEqual(x - chunk.pos.x, Chunk.SIZE - 1, new BlockPos(x + 1, y, z));
            //this.UpdateIfEqual(y - chunk.pos.y, 0,              new BlockPos(x, y - 1, z));
            //this.UpdateIfEqual(y - chunk.pos.y, Chunk.SIZE - 1, new BlockPos(x, y + 1, z));
            //this.UpdateIfEqual(z - chunk.pos.z, 0,              new BlockPos(x, y, z - 1));
            //this.UpdateIfEqual(z - chunk.pos.z, Chunk.SIZE - 1, new BlockPos(x, y, z + 1));        
        }
    }

    public byte getMeta(BlockPos pos) {
        return this.getMeta(pos.x, pos.y, pos.z);
    }

    public byte getMeta(int x, int y, int z) {
        Chunk chunk = this.getChunk(x, y, z);
        return chunk != null ? chunk.getMeta(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z) : (byte)0;
    }

    public void setMeta(BlockPos pos, byte meta) {
        this.setMeta(pos.x, pos.y, pos.z, meta);
    }

    public void setMeta(int x, int y, int z, byte meta) {
        Chunk chunk = this.getChunk(x, y, z);
        if (chunk != null) {
            BlockPos p = new BlockPos(x, y, z);
            chunk.setMeta(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, meta);
            if(chunk.getBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z).dirtyAfterMetaChange(p, meta)) {
                chunk.isDirty = true;
            }

            foreach (Direction d in Direction.all) {
                BlockPos p1 = p.move(d);
                this.getBlock(p1).onNeighborChange(p, d);
            }
        }
    }

    //What's this do?
    //void updateIfEqual(int value1, int value2, BlockPos pos) {
    //    if (value1 == value2) {
    //        Chunk chunk = getChunk(pos);
    //        if (chunk != null) {
    //            chunk.dirty = true;
    //        }
    //    }
    //}

    //Saves the world and all loaded chunks.
    public void saveWorld() {
        List<Chunk> tempChunkList = new List<Chunk>();
        foreach (Chunk c in this.loadedChunks.Values) {
            tempChunkList.Add(c);
        }
        foreach (Chunk c in tempChunkList) {
            Serialization.saveChunk(c);
            Object.Destroy(c.gameObject);
            loadedChunks.Remove(c.pos.toChunkPos());
        }
    }
}
