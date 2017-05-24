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
using System;
using VoxelEngine.ChunkLoaders;

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

                if (this.worldData.writeToDisk) {
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

        private void Update() {
            if(this.generator is WorldGeneratorCaves) {
                ((WorldGeneratorCaves)this.generator).debugDisplay();
            }
        }

        private Transform createWrapper(string name) {
            Transform t = new GameObject().transform;
            t.parent = this.transform;
            t.name = name;
            return t;
        }

        public Entity spawnEntity(GameObject prefab, NbtCompound tag) {
            Entity entity = this.func_01(prefab, true);
            entity.readFromNbt(tag);
            return entity;
        }

        public Entity spawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) {
            Entity entity = this.func_01(prefab, true);
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            return entity;
        }

        private Entity func_01(GameObject prefab, bool placeInWrapper) {
            GameObject gameObject = GameObject.Instantiate(prefab);
            if(placeInWrapper) {
                gameObject.transform.parent = this.entityWrapper;
            }
            Entity entity = gameObject.GetComponent<Entity>();
            entity.world = this;
            this.entityList.Add(entity);
            return entity;
        }

        public EntityPlayer spawnPlayer(GameObject prefab) {
            EntityPlayer player = (EntityPlayer)this.func_01(prefab, false);
            player.name = "Player";
            if(!this.nbtIOHelper.readPlayerFromDisk(player)) {
                // No player file was found.
                player.transform.position = this.worldData.spawnPos;
                player.transform.rotation = Quaternion.identity;
                player.setupFirstTimePlayer();
            }
            return player;
        }

        /// <summary>
        /// Kills an entity, removing it from the world.
        /// </summary>
        public void killEntity(Entity entity) {
            this.entityList.Remove(entity);
            GameObject.Destroy(entity.gameObject);
        }

        public EntityItem spawnItem(ItemStack stack, Vector3 position, Quaternion rotation, Vector3 force) {
            EntityItem entityItem = (EntityItem)this.spawnEntity(EntityRegistry.item.prefab, position, rotation);
            entityItem.setStack(stack);
            entityItem.rBody.AddForce(force, ForceMode.Impulse);

            return entityItem;
        }

        /// <summary>
        /// Loads a new chunk, loading it if the save exists, otherwise we generate a new one.
        /// </summary>
        public Chunk loadChunk(Chunk chunk, NewChunkInstructions instructions) {
            chunk.initChunk(this, instructions);

            this.loadedChunks.Add(instructions.chunkPos, chunk);

            if (!this.nbtIOHelper.readChunk(chunk)) {
                this.generator.generateChunk(chunk);

                // Simulate lighting updates.
                // TODO

                chunk.isDirty = true; // New chunks need to be baked.
            }
            return chunk;
        }

        public void unloadChunk(Chunk chunk) {
            this.saveChunk(chunk, true);

            // Destroy the TileEntity gameObjects within the chunk.
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

        /// <summary>
        /// Sets a block.  Using a meta of -1 will not change the meta
        /// </summary>
        public void setBlock(BlockPos pos, Block block, int meta = -1, bool updateNeighbors = true) {
            this.setBlock(pos.x, pos.y, pos.z, block, meta, updateNeighbors);
        }

        /// <summary>
        /// Sets a block.  Using a meta of -1 will not change the meta
        /// </summary>
        public void setBlock(int x, int y, int z, Block newBlock, int newMeta = -1, bool updateNeighbors = true) {
            Chunk chunk = this.getChunk(x, y, z);
            if (chunk != null) {
                // Position of the setBlock event within the chunk
                int localChunkX = x - chunk.pos.x;
                int localChunkY = y - chunk.pos.y;
                int localChunkZ = z - chunk.pos.z;

                BlockPos pos = new BlockPos(x, y, z);

                Block oldBlock = chunk.getBlock(localChunkX, localChunkY, localChunkZ);
                int oldBlockMeta = chunk.getMeta(localChunkX, localChunkY, localChunkZ);
                oldBlock.onDestroy(this, pos, oldBlockMeta);

                chunk.setBlock(localChunkX, localChunkY, localChunkZ, newBlock);

                // Set meta if it's specified.  255 means don't change
                int meta1 = (newMeta == -1 ? chunk.getMeta(localChunkX, localChunkY, localChunkZ) : newMeta);
                if(newMeta != 255) {
                    chunk.setMeta(localChunkX, localChunkY, localChunkZ, newMeta);
                }
                newBlock.onPlace(this, pos, meta1);

                // Update surrounding blocks
                if (updateNeighbors) {
                    foreach (Direction dir in Direction.all) {
                        BlockPos shiftedPos = pos.move(dir);
                        this.getBlock(shiftedPos).onNeighborChange(this, shiftedPos, this.getMeta(shiftedPos), dir.getOpposite());
                    }
                    this.dirtyChunkIfEqual(x - chunk.pos.x, 0, new BlockPos(x - 1, y, z));
                    this.dirtyChunkIfEqual(x - chunk.pos.x, Chunk.SIZE - 1, new BlockPos(x + 1, y, z));
                    this.dirtyChunkIfEqual(y - chunk.pos.y, 0, new BlockPos(x, y - 1, z));
                    this.dirtyChunkIfEqual(y - chunk.pos.y, Chunk.SIZE - 1, new BlockPos(x, y + 1, z));
                    this.dirtyChunkIfEqual(z - chunk.pos.z, 0, new BlockPos(x, y, z - 1));
                    this.dirtyChunkIfEqual(z - chunk.pos.z, Chunk.SIZE - 1, new BlockPos(x, y, z + 1));
                }

                // Update lighting.
                //this.func(new BlockPos(x, y, z), newBlock.emittedLight);
                //this.updateLighting(x, y, z, newBlock, oldBlock);

                /*
                if(newBlock.emittedLight != 0) {
                    timeFuncCalled = 0;
                    this.lightRegionForBlock(x, y, z, newBlock.emittedLight);
                    //Debug.Log(timeFuncCalled);
                }
                */
            }
        }

        public int getMeta(BlockPos pos) {
            return this.getMeta(pos.x, pos.y, pos.z);
        }

        public int getMeta(int x, int y, int z) {
            Chunk chunk = this.getChunk(x, y, z);
            return chunk != null ? chunk.getMeta(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z) : 0;
        }

        /// <summary>
        /// Returns the light at (x, y, z) or 0 if the chunk is not loaded
        /// </summary>
        public int getLight(int x, int y, int z) {
            Chunk chunk = this.getChunk(x, y, z);
            if(chunk != null) {
                return chunk.getLight(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z);
            } else {
                return 0;
            }
        }

        public TileEntityBase getTileEntity(int x, int y, int z) {
            return this.getTileEntity(new BlockPos(x, y, z));
        }
        
        /// <summary>
        /// Returns the TileEntity at pos, or null if it can't be found.
        /// </summary>
        public TileEntityBase getTileEntity(BlockPos pos) {
            Chunk chunk = this.getChunk(pos);
            if(chunk != null) {
                return this.getChunk(pos).tileEntityDict[pos];
            }
            return null;
        }

        /// <summary>
        /// Adds the passed TileEntity into the world, throwing an exception if there is already a TileEntity there.
        /// </summary>
        public void addTileEntity(BlockPos pos, TileEntityBase tileEntity) {
            Chunk chunk = this.getChunk(pos);
            if(chunk != null) {
                if(chunk.tileEntityDict.ContainsKey(pos)) {
                    throw new Exception("Error!  A second TileEntity was added to the same location!");
                }
                this.getChunk(pos).tileEntityDict.Add(pos, tileEntity);
            }
        }

        /// <summary>
        /// Removes a TileEntity from the world.
        /// </summary>
        public void removeTileEntity(BlockPos pos) {
            Chunk chunk = this.getChunk(pos);
            if(chunk != null) {
                chunk.tileEntityDict.Remove(pos);
            }
        }

        private void updateLighting(int x, int y, int z, Block oldBlock, Block newblock) {
            // Find the radius to update
            int radius = 0;
            int voxelLevel = this.getLight(x, y, z);

            if (newblock.emittedLight == 0) {
                if (voxelLevel == 0) {
                    return; // No lighting updates to do.
                } else { // Light greater than 0.
                    radius = voxelLevel;
                }
            } else if(newblock.emittedLight > 0 || oldBlock.emittedLight > 0) { // The new or old block emitts light.
                radius = voxelLevel;
            } else if(newblock.emittedLight == 0) {
                radius = Mathf.Max(
                    this.getLight(x + 1, y, z),
                    this.getLight(x - 1, y, z),
                    this.getLight(x, y + 1, z),
                    this.getLight(x, y - 1, z),
                    this.getLight(x, y, z + 1),
                    this.getLight(x, y, z - 1)); // Largest adjacent value.
            }

            // 1. Find every edge voxel that needs updating

            // 2. Update every edge voxel

            // 3. Make every inside square one less that the outside ones

            // 4. Caculate light for every square within the region
        }

        private void setLight(int x, int y, int z, int level) {
            Chunk chunk = this.getChunk(x, y, z);
            if (chunk != null) {
                chunk.setLight(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, level);
            }
        }

        private static int timeFuncCalled;

        /// <summary>
        /// Moves out from passes pos, lighting the area with the emmit light from the block.
        /// </summary>
        private void lightRegionForBlock(int x, int y, int z, int lightLevel) {
            timeFuncCalled++;

            // Return if the new light is less than 0.
            if(lightLevel <= 0) {
                return;
            }

            Chunk chunk = this.getChunk(x, y, z);

            // Return if the chunk is not loaded.
            if (chunk == null) {
                return;
            }

            int localizedX = x - chunk.pos.x;
            int localizedY = y - chunk.pos.y;
            int localizedZ = z - chunk.pos.z;

            // Return if the block is solid, it can't have light and light can't spread through it.
            if (chunk.getBlock(localizedX, localizedY, localizedZ).isSolid) {
                return;
            }

            int oldLight = chunk.getLight(localizedX, localizedY, localizedZ);

            if(oldLight >= lightLevel) {
                return; // The block's light is greater than the new one, we shouldn't change it or keep spreading
            } else {
                chunk.setLight(localizedX, localizedY, localizedZ, lightLevel);
            }

            lightLevel -= 1;
            this.lightRegionForBlock(x + 1, y, z, lightLevel);
            this.lightRegionForBlock(x - 1, y, z, lightLevel);
            this.lightRegionForBlock(x, y + 1, z, lightLevel);
            this.lightRegionForBlock(x, y - 1, z, lightLevel);
            this.lightRegionForBlock(x, y, z + 1, lightLevel);
            this.lightRegionForBlock(x, y, z - 1, lightLevel);
        }

        /// <summary>
        /// Like set block, but makes a dropped item appear.  Note, this calls World.setBlock to actually set the block to air.
        /// </summary>
        public void breakBlock(BlockPos pos, ItemTool brokenWith) {
            ItemStack[] dropList = this.getBlock(pos).getDrops(this, pos, this.getMeta(pos), brokenWith);
            if(dropList != null) {
                for(int i = 0; i < dropList.Length; i++) {
                    float f = 0.5f;
                    Vector3 offset = new Vector3(UnityEngine.Random.Range(-f, f), UnityEngine.Random.Range(-f, f), UnityEngine.Random.Range(-f, f));
                    this.spawnItem(dropList[i], pos.toVector() + offset, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0), Vector3.zero);
                }
            }
            this.setBlock(pos, Block.air);
        }

        private void dirtyChunkIfEqual(int value1, int value2, BlockPos worldChunkPos) {
            if (value1 == value2) {
                Chunk chunk = getChunk(worldChunkPos);
                if (chunk != null) {
                    chunk.isDirty = true;
                }
            }
        }

        /// <summary>
        /// Saves the entire world, including chunks, players and the world data.  Deletes entities if the world is closing.
        /// </summary>
        public void saveEntireWorld(bool despawnEntities) {
            //http://answers.unity3d.com/questions/850451/capturescreenshot-without-ui.html To hide UI
            this.nbtIOHelper.writeWorldImageToDisk();

            this.nbtIOHelper.writeWorldDataToDisk(this.worldData);
            this.nbtIOHelper.writePlayerToDisk(Main.singleton.player);

            foreach (Chunk chunk in this.loadedChunks.Values) {
                this.saveChunk(chunk, despawnEntities);
            }
        }

        /// <summary>
        /// Saves the passde chunk to disk.
        /// </summary>
        private void saveChunk(Chunk chunk, bool deleteEntities) {
            NbtCompound tag = new NbtCompound("chunk");
            chunk.writeToNbt(tag, deleteEntities);

            //if (chunk.isModified) {
            this.nbtIOHelper.writeChunkToDisk(chunk, tag);
            //    chunk.isModified = false;
            //}
        }
    }
}
