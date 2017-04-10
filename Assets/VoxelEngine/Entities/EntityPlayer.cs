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

    public class EntityPlayer : Entity, ICollecting {

        public float reach = 4f;

        // References
        public FadeText magnifyingText;
        public HeartTremble heartEffect;
        public DamageFlash damageEffect;
        public Slider hungerSlider;

        public FirstPersonController fpc;
        private BreakBlockEffect blockBreakEffect;
        public Transform mainCamera;
        private LightFlicker lightObj;
        private ItemStack lastHeldItem;
        private ChunkLoaderBase chunkLoader;
        public BlockPos posLookingAt;
        public ContainerHotbar containerHotbar;

        // State
        public float hunger;
        private float hungerDamageTimer;
        public ContainerData dataHotbar;
        public ContainerData dataInventory;

        public ContainerManager contManager;

        private new void Awake() {
            base.Awake();

            this.mainCamera = Camera.main.transform;
            this.fpc = this.GetComponent<FirstPersonController>();
            this.lightObj = this.GetComponent<LightFlicker>();

            this.dataHotbar = new ContainerData(9, 1);
            this.dataInventory = new ContainerData(5, 5);

            this.contManager = Main.singleton.containerManager;

            this.containerHotbar = ContainerManager.containerHotbar;
            this.containerHotbar.onOpen(this.dataHotbar, this);

            this.blockBreakEffect = GameObject.Instantiate(References.list.blockBreakEffect).GetComponent<BreakBlockEffect>();
        }

        private new void Start() {
            switch (WorldType.getFromId(this.world.worldData.worldType).chunkLoaderType) {
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
            // If the player is dead, don't update them
            if(this.health <= 0) {
                return;
            }

            base.onEntityUpdate();

            this.chunkLoader.updateChunkLoader();

            ItemStack heldStack = this.containerHotbar.getHeldItem();

            if (!this.contManager.isContainerOpen()) {
                // Find out what the palayer is looking at
                PlayerRayHit playerHit = this.getPlayerRayHit();

                if (playerHit != null) {
                    // We are looking at something
                    if (playerHit.hitBlock()) {
                        if (Input.GetMouseButton(0)) {
                            this.blockBreakEffect.update(this, this.posLookingAt, playerHit.hitState.block, playerHit.hitState.meta);
                        }
                        if (Input.GetMouseButtonDown(1)) {
                            if (!playerHit.hitState.block.onRightClick(this.world, this, this.posLookingAt, playerHit.hitState.meta)) {
                                if (heldStack != null) {
                                    heldStack.item.onRightClick(this.world, this, heldStack, playerHit);
                                }
                            }
                        }
                    } else if (playerHit.hitEntity()) {
                        if (Input.GetMouseButtonDown(0)) {
                            // Player is hitting an entity
                            int damage = 1;
                            if (heldStack != null && heldStack.item is ItemSword) {
                                damage = ((ItemSword)heldStack.item).damageAmount;
                            }
                            playerHit.entity.damage(damage, "Player");
                        }
                        if (Input.GetMouseButtonDown(1)) {
                            // Player is right clicking on an entity
                            playerHit.entity.onEntityInteract(this);
                        }
                    }
                } else {
                    // We are clicking on the air
                    if (Input.GetMouseButtonDown(1) && heldStack != null) {
                        this.containerHotbar.setHeldItem(heldStack.item.onRightClick(this.world, this, heldStack, playerHit));
                    }
                }                
            }

            /*
            //TODO this can be optimized, when held is null it is called every frame
            if (this.lastHeldItem == null || (this.lastHeldItem != null && heldStack != null && !this.lastHeldItem.equals(heldStack))) {
                bool isHoldingLight = false;
                if (heldStack != null && heldStack.item is ItemBlock) {
                    Block b = ((ItemBlock)heldStack.item).block;
                    if(b is ILightSource) {
                        this.copyLightData(((ILightSource)b).getPrefab());
                        isHoldingLight = true;
                    }
                }
                this.lightObj.enabled = isHoldingLight;
                this.lightObj.lightObj.enabled = isHoldingLight;
            }
            */

            // Update hunger
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
                this.contManager.closeContainer(this);

                this.scatterContainerContents(this.world, this.dataHotbar);
                this.scatterContainerContents(this.world, this.dataInventory);

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
            tag.Add(new NbtInt("selectedHotbarIndex", this.containerHotbar.index));
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
            this.containerHotbar.index = tag.Get<NbtInt>("selectedHotbarIndex").IntValue;
        }

        public ItemStack tryPickupStack(ItemStack stack) {
            ItemStack leftover = this.containerHotbar.addItemStack(stack);
            return leftover;
        }

        public void handleInput() {
            bool isShiftDown = Input.GetKey(KeyCode.LeftShift);

            // Keycodes for each of the number keys across the keyboard
            for (int i = 0; i < 9; i++) {
                if(Input.GetKeyDown((KeyCode)(i + 49))) {
                    if(isShiftDown) {
                        if (this.containerHotbar.index != i) {
                            ItemStack held = this.containerHotbar.getHeldItem();
                            this.containerHotbar.setHeldItem(this.dataHotbar.getStack(i, 0));
                            this.dataHotbar.setStack(i, 0, held);
                            this.containerHotbar.updateHudItemName();
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
                ItemStack toDrop = null;
                ItemStack stack = this.containerHotbar.getHeldItem();
                if (stack != null) {
                    int count = (isShiftDown ? stack.count : 1);
                    toDrop = new ItemStack(stack.item, stack.meta, count);
                    this.containerHotbar.setHeldItem(stack.safeDeduction(count));
                }
                if (toDrop != null) {
                    this.dropItem(toDrop);
                }
            }

            float f = Input.GetAxis("Mouse ScrollWheel");
            if (f != 0) {
                this.containerHotbar.scroll(f > 0 ? -1 : (f < 0 ? 1 : 0));
            }

            if (Input.GetKeyDown(KeyCode.E) && !this.contManager.isContainerOpen()) {
                this.contManager.openContainer(this, ContainerManager.containerInventory, this.dataInventory);
            }
        }

        // Sets the magnifying text
        public void setMagnifyingText(string text) {
            this.magnifyingText.showAndStartFade(text, 3);
        }

        // Sets the players hunger, clamping it between 0 and 100
        public void setHunger(float amount) {
            if (amount > 100f) {
                amount = 100f;
            }
            if (amount < 0f) {
                amount = 0f;
            }
            this.hunger = amount;
        }

        // Cleans up the player, destroying any associate GameObjects and disabling others
        public void cleanupPlayerObj() {
            this.containerHotbar.gameObject.SetActive(false);
            GameObject.Destroy(this.blockBreakEffect.gameObject);
            this.heartEffect.enabled = false;
        }

        // Configures a first time player, setting the starting inventory and the default health
        public void setupFirstTimePlayer() {
            this.containerHotbar.slots[0].setContents(new ItemStack(Block.ladder, 0, 25));
            this.containerHotbar.slots[1].setContents(new ItemStack(Block.torch, 0, 12));
            this.containerHotbar.slots[2].setContents(new ItemStack(Block.mushroom, 0, 16));
            this.containerHotbar.slots[3].setContents(new ItemStack(Block.fence, 0, 16));
            this.containerHotbar.slots[4].setContents(new ItemStack(Item.pebble, 0, 16));
            this.containerHotbar.slots[5].setContents(new ItemStack(Item.magnifyingGlass, 0));
            this.containerHotbar.slots[6].setContents(new ItemStack(Block.chest, 0, 16));
            this.containerHotbar.slots[7].setContents(new ItemStack(Block.rail, 0, 16));
            this.containerHotbar.slots[8].setContents(new ItemStack(Block.mossyBrick, 0, 16));
            this.health = 100;
            this.heartEffect.healthText.text = this.health + "%";
            this.hunger = 75;
        }

        // Returns a PlayerHitObject, representing what the player is looking at, or null if they are not looking at anything
        private PlayerRayHit getPlayerRayHit() {
            RaycastHit hit;
            bool rayHit = Physics.Raycast(new Ray(this.mainCamera.position, this.mainCamera.forward), out hit, this.reach);

            if(rayHit) {
                // We are looking at something
                BlockPos newLookPos = BlockPos.fromRaycastHit(hit);

                if (!(newLookPos.Equals(this.posLookingAt)) || !Input.GetMouseButton(0)) {
                    //We are either looking at a new thing or no longer holding the mouse button down
                    this.blockBreakEffect.terminate();
                }
                this.posLookingAt = newLookPos;

                if (hit.transform.CompareTag("Chunk") || hit.transform.CompareTag("Block")) {
                    return new PlayerRayHit(this.world.getBlock(this.posLookingAt), this.world.getMeta(this.posLookingAt), this.posLookingAt, hit);
                } else if (hit.transform.CompareTag("Entity")) {
                    return new PlayerRayHit(hit.transform.GetComponent<Entity>(), hit);
                } else {
                    Debug.Log("Player is looking at an object with an unknown tag, " + hit.transform.tag);
                    return null;
                }
            } else {
                // We're not looking at anything
                this.blockBreakEffect.terminate();
            }

            return null;
        }

        // Drops an item, via 'q' or closing a container
        public void dropItem(ItemStack stack) {
            EntityItem e = this.world.spawnItem(stack, this.transform.position + (Vector3.up / 2), Quaternion.Euler(0, this.transform.eulerAngles.y, 0));
            e.rBody.AddForce(this.transform.forward * 2.5f, ForceMode.Impulse);
        }

        // Unused
        /// <summary>
        /// Copies info from a prefab to the held light object
        /// </summary>
        private void copyLightData(GameObject obj) {
            LightFlicker l = obj.GetComponent<LightFlicker>();
            this.lightObj.minIntensity = l.minIntensity;
            this.lightObj.maxIntensity = l.maxIntensity;
            this.lightObj.flickerSpeed = l.flickerSpeed;
            this.lightObj.lightObj.range = l.lightObj.range;
            this.lightObj.lightObj.color = l.lightObj.color;
            this.lightObj.lightObj.intensity = l.lightObj.intensity;
        }

        // Scatters all the contents of a container, used when the player dies
        private void scatterContainerContents(World world, ContainerData d) {
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