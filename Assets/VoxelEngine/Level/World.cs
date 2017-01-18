using UnityEngine;
using System.Collections.Generic;
using VoxelEngine.Util;
using VoxelEngine.Level.Save;
using VoxelEngine.Generation;
using VoxelEngine.Entities;
using VoxelEngine.Containers;
using VoxelEngine.Blocks;
using VoxelEngine.Items;
using fNbt;

namespace VoxelEngine.Level {

    public class World : MonoBehaviour {
        public Dictionary<ChunkPos, Chunk> loadedChunks;
        public WorldGeneratorBase generator;
        public WorldData worldData;
        public SaveHelper saveHelper;
        public List<Entity> entityList;

        public GameObject chunkPrefab;

        private Transform chunkWrapper;
        private Transform entityWrapper;

        //Acts like a constructor.
        public void initWorld(WorldData data) {
            this.loadedChunks = new Dictionary<ChunkPos, Chunk>();
            this.entityList = new List<Entity>();

            this.worldData = data;

            this.saveHelper = new SaveHelper(this.worldData);
            this.saveHelper.writeWorldData(this.worldData); //Save it right away, so we dont have a folder with chunks that are unrecognized
            this.generator = WorldType.getFromId(this.worldData.worldType).getGenerator(this, this.worldData.seed);

            this.chunkWrapper = this.createWrapper("CHUNKS");
            this.entityWrapper = this.createWrapper("ENTITIES");

            //Main.singleton.onWorldLoadFinish();
        }

        public void runWorldUpdate() {
            foreach (Chunk c in this.loadedChunks.Values) {
                c.updateChunk();
            }
            //TODO if an entity is spawned by another, it is not updated this update
            for (int i = 0; i < this.entityList.Count; i++) {
                this.entityList[i].onEntityUpdate();
            }
        }

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
            EntityPlayer player = gameObject.GetComponent<EntityPlayer>();
            player.world = this;
            if(!this.saveHelper.readPlayer(player)) {
                //no player file was found
                gameObject.transform.position = this.generator.getSpawnPoint();
                gameObject.transform.rotation = Quaternion.identity;

                player.loadStartingInventory();
            }
            gameObject.transform.parent = this.entityWrapper;

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
        public Chunk loadChunk(ChunkPos pos) {
            GameObject chunkGameObject = GameObject.Instantiate(this.chunkPrefab, new Vector3(pos.x * 16, pos.y * 16, pos.z * 16), Quaternion.identity) as GameObject;
            chunkGameObject.transform.parent = this.chunkWrapper;
            Chunk chunk = chunkGameObject.GetComponent<Chunk>();
            chunk.initChunk(this, pos);

            chunk.isDirty = true;

            this.loadedChunks.Add(pos, chunk);

            if (!this.saveHelper.readChunk(chunk)) {
                this.generator.generateChunk(chunk);
                chunk.isModified = true;
            }
            return chunk;
        }

        public void unloadChunk(Chunk chunk) {
            this.saveChunk(chunk, true);
            GameObject.Destroy(chunk.gameObject);
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
                    foreach (Direction dir in Direction.all) {
                        BlockPos shiftedPos = pos.move(dir);
                        this.getBlock(shiftedPos).onNeighborChange(this, shiftedPos, dir.getOpposite());
                        //chunk = this.getChunk(shiftedPos.x, shiftedPos.y, shiftedPos.z);
                        //if (chunk != null) {
                        //    chunk.isDirty = true;
                        //}
                    }
                }
                chunk.isDirty = true;
                this.updateIfEqual(pos.x - chunk.pos.x, 0, new BlockPos(pos.x - 1, pos.y, pos.z));
                this.updateIfEqual(pos.x - chunk.pos.x, Chunk.SIZE - 1, new BlockPos(pos.x + 1, pos.y, pos.z));
                this.updateIfEqual(pos.y - chunk.pos.y, 0, new BlockPos(pos.x, pos.y - 1, pos.z));
                this.updateIfEqual(pos.y - chunk.pos.y, Chunk.SIZE - 1, new BlockPos(pos.x, pos.y + 1, pos.z));
                this.updateIfEqual(pos.z - chunk.pos.z, 0, new BlockPos(pos.x, pos.y, pos.z - 1));
                this.updateIfEqual(pos.z - chunk.pos.z, Chunk.SIZE - 1, new BlockPos(pos.x, pos.y, pos.z + 1));
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
                if (chunk.getBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z).dirtyAfterMetaChange(p, meta)) {
                    chunk.isDirty = true;
                }

                foreach (Direction dir in Direction.all) {
                    BlockPos shiftedPos = p.move(dir);
                    this.getBlock(shiftedPos).onNeighborChange(this, shiftedPos, dir.getOpposite());
                }
            }
        }

        //Like set block, but makes a dropped item appear.  Note, this calls World.setBlock to actually set the block to air.
        public void breakBlock(BlockPos pos, ItemTool brokenWith) {
            Block block = this.getBlock(pos);
            foreach (ItemStack stack in block.getDrops(this.getMeta(pos), brokenWith)) {
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

        private void saveChunk(Chunk chunk, bool despawnEntites) {
            NbtCompound tag = new NbtCompound("chunk");
            chunk.writeToNbt(tag);

            Entity entity;
            List<Entity> entitiesInChunk = new List<Entity>();
            for (int i = this.entityList.Count - 1; i >= 0; i--) {
                entity = this.entityList[i];
                int x = Mathf.FloorToInt((int)this.transform.position.x / (float)Chunk.SIZE);
                int y = Mathf.FloorToInt((int)this.transform.position.y / (float)Chunk.SIZE);
                int z = Mathf.FloorToInt((int)this.transform.position.z / (float)Chunk.SIZE);

                if (x == chunk.chunkPos.x && y ==chunk.chunkPos.y && z == chunk.chunkPos.z) {
                    this.entityList.Remove(entity);
                    entitiesInChunk.Add(entity);
                }
            }
            NbtList list = new NbtList("entities", NbtTagType.Compound);
            for (int i = 0; i < entitiesInChunk.Count; i++) {
                entity = entitiesInChunk[i];
                list.Add(entity.writeToNbt(new NbtCompound()));
                if(despawnEntites) {
                    GameObject.Destroy(entity.gameObject);
                }
            }
            tag.Add(list);

            //if (chunk.isModified) {
            this.saveHelper.writeChunkToDisk(chunk, tag);
            //}
            //chunk.isModified = false;
        }

        public void saveEntireWorld(bool despawnEntities) {
            //http://answers.unity3d.com/questions/850451/capturescreenshot-without-ui.html To hide UI
            ScreenshotHelper.captureScreenshot(this.saveHelper.saveFolderName + "/worldImage.png");

            this.saveHelper.writeWorldData(this.worldData);
            this.saveHelper.writePlayer(Main.singleton.player);

            foreach (Chunk chunk in this.loadedChunks.Values) {
                this.saveChunk(chunk, despawnEntities);
            }
        }
    }
}
