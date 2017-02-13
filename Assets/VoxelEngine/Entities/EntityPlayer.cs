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
using VoxelEngine.GUI;
using VoxelEngine.Level;
using VoxelEngine.GUI.Effect;

namespace VoxelEngine.Entities {

    public class EntityPlayer : Entity {

        public float reach = 3.5f;

        // References
        public FadeText magnifyingText;
        public HeartTremble heartEffect;
        public DamageFlash damageEffect;
        public Slider hungerSlider;

        public FirstPersonController fpc;
        private BreakBlockEffect blockBreakEffect;
        public Transform mainCamera;
        public Container containerElement;
        private LightFlicker lightObj;
        private ItemStack lastHeldItem;
        private ChunkLoaderBase chunkLoader;
        public BlockPos posLookingAt;
        public ContainerHotbar containerHotbar;

        // State
        public float hunger;
        private float hungerDamageTimer;
        // The stack the mouse is holding when a container is open
        public ItemStack heldStack;
        public ContainerDataHotbar dataHotbar;
        public ContainerData dataInventory;

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
                case ChunkLoaderBase.LOCKED_Y:
                    this.chunkLoader = new ChunkLoaderLockedY(this.world, this);
                    break;
                case ChunkLoaderBase.INFINITE:
                    this.chunkLoader = new ChunkLoaderInfinite(this.world, this);
                    break;
                case ChunkLoaderBase.REGION_DEBUG:
                    this.chunkLoader = new ChunkLoaderRegionDebug(this.world, this);
                    break;
                default:
                    print("ERROR! No chunk loader could be set!");
                    break;
            }
        }

        public override void onEntityUpdate() {
            if(this.health <= 0) {
                return;
            }

            base.onEntityUpdate();

            this.chunkLoader.updateChunkLoader();

            ItemStack heldStack = this.dataHotbar.getHeldItem();

            if (this.containerElement == null) {
                PlayerRayHit playerHit = this.getPlayerRayHit();

                if (playerHit != null) {
                    if(playerHit.unityRaycastHit.distance <= this.reach) {
                        if (playerHit.state != null) {
                            if (Input.GetMouseButton(0)) {
                                this.blockBreakEffect.update(this, this.posLookingAt, playerHit.state.block, playerHit.state.meta);
                            }
                            if (Input.GetMouseButtonDown(1)) {
                                if(!playerHit.state.block.onRightClick(this.world, this, this.posLookingAt, playerHit.state.meta)) {
                                    if(this.heldStack != null) {
                                        this.heldStack.item.onRightClick(this.world, this, heldStack, playerHit);
                                    }
                                }
                            }
                        } else if (playerHit.entity != null) {
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
                }

                //Right click
                if (Input.GetMouseButtonDown(1)) {
                    if (playerHit != null && heldStack != null) {
                        this.dataHotbar.setHeldItem(heldStack.item.onRightClick(this.world, this, heldStack, playerHit));
                    }
                }
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

            // Hunger
            this.hunger -= Time.deltaTime * 0.25f;
            this.hungerSlider.value = this.hunger;
            if(this.hunger <= 0f) {
                this.hunger = 0f;

                this.hungerDamageTimer -= Time.deltaTime;
                if(this.hungerDamageTimer <= -2f) {
                    this.damage(1, "You forget to eat!");
                    this.hungerDamageTimer = 0f;
                }
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
            if (amount > 100) {
                amount = 100;
            }
            if (amount < 0) {
                amount = 0;
            }
            this.heartEffect.startAnimation(this.health, amount);
            this.health = amount;
        }

        public override bool damage(int amount, string message) {
            this.damageEffect.startEffect();
            this.setHealth(this.health - amount);
            if (this.health <= 0) {
                // Player has died
                if(this.containerElement != null) {
                    this.closeContainer();
                }
                this.func_001(this.world, this.dataHotbar);
                this.func_001(this.world, this.dataInventory);

                Main.hideMouse(false);
                this.fpc.enabled = false;
                Main m = Main.singleton;
                m.openGuiScreen(m.respawnScreen);
                ((GuiScreenRespawn)m.currentGui).deathMessageText.text = message;
                return true;
            }
            return false;
        }

        public override byte getEntityId() {
            return 1;
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtFloat("cameraX", this.mainCamera.eulerAngles.x));
            tag.Add(this.dataHotbar.writeToNbt(new NbtCompound("hotbar")));
            tag.Add(this.dataInventory.writeToNbt(new NbtCompound("inventory")));
            tag.Add(new NbtFloat("hunger", this.hunger));
            tag.Add(new NbtFloat("hungerTimer", this.hunger));
            //TODO jump
            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);
            this.mainCamera.localRotation = Quaternion.Euler(tag.Get<NbtFloat>("cameraX").FloatValue, 0, 0);
            this.dataHotbar.readFromNbt(tag.Get<NbtCompound>("hotbar"));
            this.dataInventory.readFromNbt(tag.Get<NbtCompound>("inventory"));
            this.hunger = tag.Get<NbtFloat>("hunger").FloatValue;
            this.hungerDamageTimer = tag.Get<NbtFloat>("hungerTimer").FloatValue;
        }

        public void handleInput() {
            bool isShiftDown = Input.GetKey(KeyCode.LeftShift);

            // Keycodes for each of the number keys across the keyboard
            for (int i = 0; i < 9; i++) {
                if(Input.GetKeyDown((KeyCode)(i + 49))) {
                    if(isShiftDown) {
                        if (this.dataHotbar.index != i) {
                            ItemStack held = this.dataHotbar.getHeldItem();
                            this.dataHotbar.setHeldItem(this.dataHotbar.getStack(i, 0));
                            this.dataHotbar.setStack(i, 0, held);
                            this.containerHotbar.showItemName();
                        }
                    } else {
                        this.containerHotbar.setSelected(i);
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Y)) {
                this.damage(30, "Test");
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                ItemStack stack = this.dataHotbar.dropItem(this.dataHotbar.index, isShiftDown);
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

        // Opens and initializes a container
        public void openContainer(GameObject containerObj, ContainerData containerData) {
            this.fpc.allowInput = false;
            this.containerElement = GameObject.Instantiate(containerObj).GetComponent<Container>();
            this.containerElement.transform.SetParent(this.transform);
            this.containerElement.initContainer(containerData, this);
            Main.hideMouse(false);
        }

        // Closes the open container, doing any required cleanup
        public void closeContainer() {
            this.fpc.allowInput = true;
            if (this.heldStack != null) {
                this.dropItem(this.heldStack);
                this.heldStack = null;
            }
            GameObject.Destroy(this.containerElement.gameObject);
            Main.hideMouse(true);
        }

        public void setMagnifyingText(string text) {
            this.magnifyingText.showAndStartFade(text, 3);
        }

        public void setHunger(float amount) {
            if (amount > 100f) {
                amount = 100f;
            }
            if (amount < 0f) {
                amount = 0f;
            }
            this.hunger = amount;
        }

        public void cleanupObject() {
            GameObject.Destroy(this.containerHotbar.gameObject);
            GameObject.Destroy(this.blockBreakEffect.gameObject);
            this.heartEffect.enabled = false;
        }

        public void setupFirstTimePlayer() {
            this.dataHotbar.addItemStack(new ItemStack(Block.fence, 0, 10));
            this.dataHotbar.addItemStack(new ItemStack(Block.lantern, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Block.torch, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Item.food, 0, 10));
            this.dataHotbar.addItemStack(new ItemStack(Item.pebble, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Item.magnifyingGlass, 0));
            this.dataHotbar.addItemStack(new ItemStack(Block.mushroom, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Block.poisonMushroom, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Block.mossyBrick, 0));
            this.health = 100;
            this.heartEffect.healthText.text = this.health + "%";
            this.hunger = 75;
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

        private void dropItem(ItemStack stack) {
            //TODO make items not collide with the player
            this.world.spawnItem(stack, this.transform.position + (Vector3.up / 2) + this.transform.forward / 5, Quaternion.Euler(0, this.transform.eulerAngles.y, 0));
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

        private void func_001(World world, ContainerData d) {
            float f = 0.5f;
            for (int i = 0; i < d.items.Length; i++) {
                Vector3 offset = new Vector3(Random.Range(-f, f), Random.Range(-f, f), Random.Range(-f, f));
                ItemStack stack = d.items[i];
                if (stack != null) {
                    this.world.spawnItem(d.items[i], this.transform.position + offset, Quaternion.Euler(0, Random.Range(0, 360), 0));
                    d.items[i] = null;
                }
            }
        }
    }
}
