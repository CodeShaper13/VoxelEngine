using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class World : MonoBehaviour {
    public Dictionary<ChunkPos, Chunk> loadedChunks = new Dictionary<ChunkPos, Chunk>();
    public WorldGeneratorBase generator;
    public WorldData worldData;
    public SaveHandler saveHandler;

    public GameObject chunkPrefab;
    public GameObject itemPrefab;

    private Transform chunkWrapper;
    private Transform itemWrapper;

    void Awake() {
        this.saveHandler = new SaveHandler("world1");

        this.worldData = this.saveHandler.getWorldData();

        this.generator = this.worldData.worldType == 0 ? (WorldGeneratorBase) new WorldGenerator(this, worldData.seed) : new WorldGeneratorFlat(this, this.worldData.seed);

        this.chunkWrapper = this.createWrapper("CHUNKS");
        this.itemWrapper = this.createWrapper("ITEMS");
    }

    void LateUpdate() {
        foreach (Chunk c in this.loadedChunks.Values) {
            c.updateChunk();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            this.saveEntireWorld();
        }
    }

    public Transform createWrapper(string name) {
        Transform t = new GameObject().transform;
        t.parent = this.transform;
        t.name = name;
        return t;
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
        GameObject chunkGameObject = GameObject.Instantiate(chunkPrefab, new Vector3(pos.x * 16, pos.y * 16, pos.z * 16), Quaternion.identity) as GameObject;
        chunkGameObject.transform.parent = this.chunkWrapper;
        Chunk chunk = chunkGameObject.GetComponent<Chunk>();
        chunk.initChunk(this, pos);

        chunk.isNeedingSave = true;
        chunk.isDirty = true;

        this.loadedChunks.Add(pos, chunk);

        if(!this.saveHandler.deserializeChunk(chunk)) {
            this.generator.generateChunk(chunk);
        }
        return chunk;
    }

    public void unloadChunk(ChunkPos pos) {
        Chunk chunk = this.getChunk(pos);
        if(chunk != null) {
            this.saveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            this.loadedChunks.Remove(pos);
        } else {
            Debug.LogWarning("Trying to save an unloaded chunk, something is wrong!");
        }
    }

    public Chunk getChunk(ChunkPos pos) {
        Chunk chunk = null;
        loadedChunks.TryGetValue(pos, out chunk);
        return chunk;
    }

    public Chunk getChunk(BlockPos pos) {
        return this.getChunk(pos.x, pos.y, pos.z);
    }

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

    public void setBlock(BlockPos pos, Block block, bool updateNeighbors = true) {
        Chunk chunk = this.getChunk(pos);
        if (chunk != null) {
            int x1 = pos.x - chunk.pos.x;
            int y1 = pos.y - chunk.pos.y;
            int z1 = pos.z - chunk.pos.z;
            byte meta = chunk.getMeta(x1, y1, z1);
            chunk.getBlock(x1, y1, z1).onDestroy(this, pos, meta);
            chunk.setBlock(x1, y1, z1, block);
            block.onPlace(this, pos, meta);

            if (updateNeighbors) {
                foreach (Direction d in Direction.all) {
                    BlockPos shiftedPos = pos.move(d);
                    this.getBlock(shiftedPos).onNeighborChange(pos, d);
                    chunk = this.getChunk(shiftedPos.x, shiftedPos.y, shiftedPos.z);
                    if (chunk != null) {
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

    public void setBlock(int x, int y, int z, Block block, bool updateNeighbors = true) {
        this.setBlock(new BlockPos(x, y, z), block, updateNeighbors);
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

    private void saveChunk(Chunk chunk) {
        if (chunk.isNeedingSave) {
            this.saveHandler.serializeChunk(chunk);
        }

        chunk.isNeedingSave = false;
    }

    private void saveEntireWorld() {
        this.saveHandler.serializeWorldData(this.worldData);

        foreach(Chunk chunk in this.loadedChunks.Values) {
            this.saveChunk(chunk);
        }
    }
}
