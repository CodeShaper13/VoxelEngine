using System;
using fNbt;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities.Player;
using VoxelEngine.Items;
using VoxelEngine.Util;
using UnityStandardAssets.Characters.FirstPerson;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Generation;
using VoxelEngine.TileEntity;

namespace VoxelEngine.Entities {

    public class EntityPlayer : Entity {
        public Text magnifyingText;
        public RawImage healthBarImage;

        public FirstPersonController fpc;
        private BreakBlockEffect blockBreakEffect;
        public Transform mainCamera;
        public Container containerElement;
        private LightFlicker lightObj;
        private ItemStack lastHeldItem;

        public float reach = 3.5f;
        public float magnifyingTimer;
        public BlockPos posLookingAt;
        public ItemStack heldStack;

        public ContainerHotbar containerHotbar;

        public ContainerDataHotbar dataHotbar;
        public ContainerData dataInventory;

        public ChunkLoaderBase chunkLoader;

        public new void Awake() {
            base.Awake();

            this.mainCamera = Camera.main.transform;
            this.fpc = this.GetComponent<FirstPersonController>();
            this.lightObj = this.GetComponent<LightFlicker>();

            this.dataHotbar = new ContainerDataHotbar();
            this.dataInventory = new ContainerData(2, 2);

            //Setup the hotbar
            this.containerHotbar = GameObject.Instantiate(References.list.containerHotbar).GetComponent<ContainerHotbar>();
            this.containerHotbar.initContainer(this.dataHotbar, this);

            this.blockBreakEffect = GameObject.Instantiate(References.list.blockBreakEffect).GetComponent<BreakBlockEffect>();
        }

        public void Start() {
            switch(WorldType.getFromId(this.world.worldData.worldType).chunkLoaderType) {
                case 0:
                    this.chunkLoader = new ChunkLoaderLockedY(this.world, this);
                    break;
                case 1:
                    this.chunkLoader = new ChunkLoaderInfinite(this.world, this);
                    break;
                case 2:
                    this.chunkLoader = new ChunkLoaderRegionDebug(this.world, this);
                    break;
            }
        }

        public override void onEntityUpdate() {
            base.onEntityUpdate();

            if(this.chunkLoader != null) { //Why is there a safety check?
                this.chunkLoader.updateChunkLoader();
            }

            ItemStack heldStack = this.dataHotbar.getHeldItem();

            if (this.containerElement == null) {
                PlayerRayHit playerHit = this.getPlayerRayHit();

                if (playerHit != null) {
                    if (playerHit.state != null && playerHit.unityRaycastHit.distance <= this.reach) {
                        if (Input.GetMouseButton(0)) {
                            this.blockBreakEffect.update(this, this.posLookingAt, playerHit.state.block, playerHit.state.meta);
                        }
                        if (Input.GetMouseButtonDown(1)) {
                            playerHit.state.block.onRightClick(this.world, this, this.posLookingAt, playerHit.state.meta);
                        }
                    }
                    else if (playerHit.entity != null && playerHit.unityRaycastHit.distance <= this.reach) {
                        if (Input.GetMouseButtonDown(0)) {
                            float damage = 1;
                            if (heldStack != null && heldStack.item is ItemSword) {
                                damage = ((ItemSword)heldStack.item).damageAmount;
                            }
                            playerHit.entity.onEntityHit(this, damage);
                        }
                        if (Input.GetMouseButtonDown(1)) {
                            playerHit.entity.onEntityInteract(this);
                        }
                    }
                }

                //Right click
                if (Input.GetMouseButtonDown(1)) {
                    if (playerHit != null && heldStack != null) {
                        this.dataHotbar.setHeldItem(heldStack.item.onRightClick(this.world, this, heldStack, playerHit));
                    }
                }
            }

            //Magnifying text
            if (this.magnifyingTimer > 0) {
                this.magnifyingTimer -= Time.deltaTime;
            }
            if (this.magnifyingTimer <= 0) {
                this.magnifyingText.color = Color.Lerp(this.magnifyingText.color, Color.clear, 3 * Time.deltaTime);
            }

            //TODO this can be optimised, when held is null it is called every frame
            if (this.lastHeldItem == null || (this.lastHeldItem != null && heldStack != null && !this.lastHeldItem.equals(heldStack))) {
                bool isHoldingLight = false;
                if (heldStack != null && heldStack.item is ItemBlock) {
                    Block b = ((ItemBlock)heldStack.item).block;
                    if (b == Block.lantern) {
                        this.copyLightData(References.list.lanternPrefab);
                        isHoldingLight = true;
                    }
                    else if (b == Block.torch) {
                        this.copyLightData(References.list.lanternPrefab);
                        isHoldingLight = true;
                    }
                }
                this.lightObj.enabled = isHoldingLight;
                this.lightObj.lightObj.enabled = isHoldingLight;
            }
            this.lastHeldItem = heldStack;
        }

        public override void onEntityCollision(Entity otherEntity) {
            if (otherEntity is EntityItem) {
                EntityItem entityItem = (EntityItem)otherEntity;
                if(entityItem.pickupDelay <= 0) {
                    ItemStack stack = this.dataHotbar.addItemStack(entityItem.stack);
                    //If we were able to pick up the entire contents of the stack, kill the entity
                    if (stack == null) {
                        this.world.killEntity(otherEntity);
                    }
                }
            }
        }

        public override void setHealth(int amount) {
            if (amount > 10) {
                amount = 10;
            }
            this.health = amount;
            this.healthBarImage.uvRect = new Rect(0, 0, this.health / 10, 1);
        }

        public void loadStartingInventory() {
            this.dataHotbar.addItemStack(new ItemStack(Block.chest));
            this.dataHotbar.addItemStack(new ItemStack(Block.lantern, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Block.torch, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Item.goldSword));
            this.dataHotbar.addItemStack(new ItemStack(Item.pebble, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Item.magnifyingGlass, 2));
            this.dataHotbar.addItemStack(new ItemStack(Block.mushroom, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Block.poisonMushroom, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Block.mossyBrick, 0));

            this.setHealth(10);
        }

        private PlayerRayHit getPlayerRayHit() {
            RaycastHit hit;
            bool rayHit = Physics.Raycast(this.mainCamera.position, this.mainCamera.forward, out hit);

            //Find out what we are looking at
            BlockPos p = BlockPos.fromRaycast(hit, false);

            if (rayHit == false || !(p.Equals(this.posLookingAt)) || !Input.GetMouseButton(0)) {
                //If we are looking at a new thing, reset/remove the block break effect
                this.blockBreakEffect.terminate();
            }
            this.posLookingAt = p;

            if (hit.transform != null) {
                if (hit.transform.tag == "Chunk" || hit.transform.tag == "Block") {
                    return new PlayerRayHit(this.world.getBlock(this.posLookingAt), this.world.getMeta(this.posLookingAt), this.posLookingAt, hit);
                }
                else if (hit.transform.tag == "Entity") {
                    return new PlayerRayHit(hit.transform.GetComponent<Entity>(), hit);
                }
            }
            return null;
        }

        public void handleInput() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                ItemStack stack = this.dataHotbar.dropItem(this.dataHotbar.index, false /* Input.GetKey(KeyCode.LeftControl) */);
                if (stack != null) {
                    this.dropItem(stack);
                }
            }
            float f = Input.GetAxis("Mouse ScrollWheel");
            if (f != 0) {
                this.containerHotbar.scroll(f > 0 ? 1 : (f < 0 ? -1 : 0));
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                this.openContainer(References.list.containerInventory, this.dataInventory);
            }
        }

        private void copyLightData(GameObject obj) {
            LightFlicker l = obj.GetComponent<LightFlicker>();
            this.lightObj.minIntensity = l.minIntensity;
            this.lightObj.maxIntensity = l.maxIntensity;
            this.lightObj.flickerSpeed = l.flickerSpeed;
            this.lightObj.lightObj.range = l.lightObj.range;
            this.lightObj.lightObj.color = l.lightObj.color;
            this.lightObj.lightObj.intensity = l.lightObj.intensity;
        }

        //Opens and initiates a container
        public void openContainer(GameObject containerObj, ContainerData containerData) {
            this.fpc.allowInput = false;
            this.containerElement = GameObject.Instantiate(containerObj).GetComponent<Container>();
            this.containerElement.transform.SetParent(this.transform);
            this.containerElement.initContainer(containerData, this);
            Main.setMouseLock(false);
        }

        //Closes the open container, doing any required cleanup
        public void closeContainer() {
            this.fpc.allowInput = true;
            if (this.heldStack != null) {
                this.dropItem(this.heldStack);
                this.heldStack = null;
            }
            GameObject.Destroy(this.containerElement.gameObject);
            Main.setMouseLock(true);
        }

        private void dropItem(ItemStack stack) {
            //TODO make items not collide with the player
            this.world.spawnItem(stack, this.transform.position + (Vector3.up / 2) + this.transform.forward / 5, Quaternion.Euler(0, this.transform.eulerAngles.y, 0));
        }

        public void cleanupObject() {
            GameObject.Destroy(this.containerHotbar.gameObject);
            GameObject.Destroy(this.blockBreakEffect.gameObject);
        }

        public override byte getEntityId() {
            return 1;
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtFloat("cameraX", this.mainCamera.eulerAngles.x));
            tag.Add(this.dataHotbar.writeToNbt(new NbtCompound("hotbar")));
            tag.Add(this.dataInventory.writeToNbt(new NbtCompound("inventory")));
            //TODO jump, maybe general up/down for entities
            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);
            this.mainCamera.localRotation = Quaternion.Euler(tag.Get<NbtFloat>("cameraX").FloatValue, 0, 0);
            this.dataHotbar.readFromNbt(tag.Get<NbtCompound>("hotbar"));
            this.dataInventory.readFromNbt(tag.Get<NbtCompound>("inventory"));
        }
    }
}
