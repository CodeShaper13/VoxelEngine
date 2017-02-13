using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Util;
using VoxelEngine.Generation;
using VoxelEngine.Entities;
using VoxelEngine.Containers;
using VoxelEngine.Blocks;
using VoxelEngine.Items;
using fNbt;
using VoxelEngine.TileEntity;
using VoxelEngine.Generation.Caves;

namespace VoxelEngine.Level {

    public class World : MonoBehaviour {

        public Dictionary<ChunkPos, Chunk> loadedChunks;
        public WorldGeneratorBase generator;
        public WorldData worldData;
        public NbtIOHelper nbtIOHelper;
        public List<Entity> entityList;

        public Transform chunkWrapper;
        private Transform entityWrapper;
        public Transform tileEntityWrapper;

        //Acts like a constructor.
        public void initWorld(WorldData data) {
            this.worldData = data;
            this.loadedChunks = new Dictionary<ChunkPos, Chunk>();
            this.entityList = new List<Entity>();
            this.nbtIOHelper = new NbtIOHelper(this.worldData);
            this.generator = WorldType.getFromId(this.worldData.worldType).getGenerator(this, this.worldData.seed);

            if (!this.nbtIOHelper.readGenerationData(this.generator)) {
                // There is no generation data, generate it
                if(this.generator.generateLevelData()) {
                    this.nbtIOHelper.writeGenerationData(this.generator);
                }
                this.worldData.spawnPos = this.generator.getSpawnPoint();

                if (!this.worldData.dontWriteToDisk) {
                    // Save the world data right away so we don't have a folder with chunks that is
                    // recognized as a save.
                    this.nbtIOHelper.writeWorldDataToDisk(this.worldData);
                }
            }

            this.chunkWrapper = this.createWrapper("CHUNKS");
            this.entityWrapper = this.createWrapper("ENTITIES");
            this.tileEntityWrapper = this.createWrapper("TILE_ENTITIES");

            //Main.singleton.onWorldLoadFinish();
        }

        //public void Update() {
        //    if(this.generator is WorldGeneratorCaves) {
        //        ((WorldGeneratorCaves)this.generator).debugDisplay();
        //    }
        //}

        private Transform createWrapper(string name) {
            Transform t = new GameObject().transform;
            t.parent = this.transform;
            t.name = name;
            return t;
        }

        public Entity spawnEntity(GameObject prefab, NbtCompound tag) {
            Entity entity = this.func_01(prefab);
            entity.readFromNbt(tag);
            return entity;
        }

        public Entity spawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) {
            Entity entity = this.func_01(prefab);
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            return entity;
        }

        private Entity func_01(GameObject prefab) {
            GameObject gameObject = GameObject.Instantiate(prefab);
            gameObject.transform.parent = this.entityWrapper;
            Entity entity = gameObject.GetComponent<Entity>();
            entity.world = this;
            this.entityList.Add(entity);
            return entity;
        }

        public EntityPlayer spawnPlayer(GameObject prefab) {
            GameObject gameObject = GameObject.Instantiate(prefab);
            gameObject.name = "Player";
            EntityPlayer player = gameObject.GetComponent<EntityPlayer>();
            player.world = this;
            if(!this.nbtIOHelper.readPlayerFromDisk(player)) {
                //no player file was found
                gameObject.transform.position = this.worldData.spawnPos;
                gameObject.transform.rotation = Quaternion.identity;
                player.setupFirstTimePlayer();
            }
            return player;
        }

        public void killEntity(Entity entity) {
            this.entityList.Remove(entity);
            GameObject.Destroy(entity.gameObject);
        }

        public void spawnItem(ItemStack stack, Vector3 position, Quaternion rotation) {
            EntityItem entityItem = (EntityItem)this.spawnEntity(EntityList.singleton.itemPrefab, position, rotation);
            entityItem.stack = stack;
        }

        //Loads a new chunk, loading it if the save exists, otherwise we generate a new one.
        public Chunk loadChunk(Chunk chunk, ChunkPos pos) {
            chunk.initChunk(this, pos);

            this.loadedChunks.Add(pos, chunk);

            if (!this.nbtIOHelper.readChunk(chunk)) {
                this.generator.generateChunk(chunk);
                chunk.isModified = true;
            }
            return chunk;
        }

        public void unloadChunk(Chunk chunk) {
            this.saveChunk(chunk, true);

            //Destroy the TileEntity gameObjects within the chunk.
            foreach(TileEntityBase te in chunk.tileEntityDict.Values) {
                if(te is TileEntityGameObject) {
                    GameObject.Destroy(((TileEntityGameObject)te).gameObject);
                }
            }

            this.loadedChunks.Remove(chunk.chunkPos);
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
            return this.getChunk(new ChunkPos(x1, y1, z1));
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
            this.setBlock(pos.x, pos.y, pos.z, block, updateNeighbors);
        }

        public void setBlock(int x, int y, int z, Block block, bool updateNeighbors = true) {
            Chunk chunk = this.getChunk(x, y, z);
            if (chunk != null) {
                int x1 = x - chunk.pos.x;
                int y1 = y - chunk.pos.y;
                int z1 = z - chunk.pos.z;
                BlockPos pos = new BlockPos(x, y, z);
                byte meta = chunk.getMeta(x1, y1, z1);
                chunk.getBlock(x1, y1, z1).onDestroy(this, pos, meta);
                chunk.setBlock(x1, y1, z1, block);
                block.onPlace(this, pos, meta);

                if (updateNeighbors) {
                    foreach (Direction dir in Direction.all) {
                        BlockPos shiftedPos = pos.move(dir);
                        this.getBlock(shiftedPos).onNeighborChange(this, shiftedPos, dir.getOpposite());
                    }
                    chunk.isDirty = true;
                    this.updateIfEqual(x - chunk.pos.x, 0, new BlockPos(x - 1, y, z));
                    this.updateIfEqual(x - chunk.pos.x, Chunk.SIZE - 1, new BlockPos(x + 1, y, z));
                    this.updateIfEqual(y - chunk.pos.y, 0, new BlockPos(x, y - 1, z));
                    this.updateIfEqual(y - chunk.pos.y, Chunk.SIZE - 1, new BlockPos(x, y + 1, z));
                    this.updateIfEqual(z - chunk.pos.z, 0, new BlockPos(x, y, z - 1));
                    this.updateIfEqual(z - chunk.pos.z, Chunk.SIZE - 1, new BlockPos(x, y, z + 1));
                }
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
                chunk.isDirty = true;

                foreach (Direction dir in Direction.all) {
                    BlockPos shiftedPos = p.move(dir);
                    this.getBlock(shiftedPos).onNeighborChange(this, shiftedPos, dir.getOpposite());
                }
            }
        }

        public TileEntityBase getTileEntity(BlockPos pos) {
            return this.getChunk(pos).tileEntityDict[pos];
        }

        public void addTileEntity(BlockPos pos, TileEntityBase tileEntity) {
            this.getChunk(pos).tileEntityDict.Add(pos, tileEntity);
        }

        public void removeTileEntity(BlockPos pos) {
            this.getChunk(pos).tileEntityDict.Remove(pos);
        }

        public void tickBlock(BlockPos pos, int time) {

        }

        //Like set block, but makes a dropped item appear.  Note, this calls World.setBlock to actually set the block to air.
        public void breakBlock(BlockPos pos, ItemTool brokenWith) {
            Block block = this.getBlock(pos);
            foreach (ItemStack stack in block.getDrops(this, pos, this.getMeta(pos), brokenWith)) {
                float f = 0.5f;
                Vector3 offset = new Vector3(Random.Range(-f, f), Random.Range(-f, f), Random.Range(-f, f));
                this.spawnItem(stack, pos.toVector() + offset, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
            this.setBlock(pos, Block.air);
        }

        private void updateIfEqual(int value1, int value2, BlockPos pos) {
            if (value1 == value2) {
                Chunk chunk = getChunk(pos);
                if (chunk != null) {
                    chunk.isDirty = true;
                }
            }
        }

        private void saveChunk(Chunk chunk, bool deleteEntities) {
            NbtCompound tag = new NbtCompound("chunk");
            chunk.writeToNbt(tag, deleteEntities);

            //if (chunk.isModified) {
            this.nbtIOHelper.writeChunkToDisk(chunk, tag);
            //}
            //chunk.isModified = false;
        }

        public void saveEntireWorld(bool despawnEntities) {
            //http://answers.unity3d.com/questions/850451/capturescreenshot-without-ui.html To hide UI
            this.nbtIOHelper.writeWorldImageToDisk();

            this.nbtIOHelper.writeWorldDataToDisk(this.worldData);
            this.nbtIOHelper.writePlayerToDisk(Main.singleton.player);

            foreach (Chunk chunk in this.loadedChunks.Values) {
                this.saveChunk(chunk, despawnEntities);
            }
        }
    }
}
