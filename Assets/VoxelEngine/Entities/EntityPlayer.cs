using fNbt;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Containers.Data;
using VoxelEngine.Entities.Player;
using VoxelEngine.Items;
using VoxelEngine.Util;
using VoxelEngine.ChunkLoaders;
using VoxelEngine.Generation;
using VoxelEngine.GUI;
using VoxelEngine.Level;
using VoxelEngine.GUI.Effect;

namespace VoxelEngine.Entities {

    public class EntityPlayer : Entity, ICollecting {

        // References
        public FadeText magnifyingText;
        public HeartTremble heartEffect;
        public DamageFlash damageEffect;
        public Slider hungerSlider;
        public Transform handTransfrom;

        public BlockPos posLookingAt;
        public ContainerHotbar containerHotbar;
        public Transform mainCamera;

        private PlayerMover playerMover;
        private BreakBlockEffect blockBreakEffect;
        private ChunkLoaderBase chunkLoader;
        private bool onGroundLastUpdate;
        private float lastGroundedY;

        // State
        public float hunger;
        private float hungerDamageTimer;
        public ContainerData dataHotbar;
        public ContainerData dataInventory;

        public ContainerManager contManager;

        protected override void onConstruct() {
            base.onConstruct();

            this.mainCamera = Camera.main.transform;

            this.dataHotbar = new ContainerData(9, 1);
            this.dataInventory = new ContainerData(5, 5);

            this.contManager = Main.singleton.containerManager;

            this.containerHotbar = ContainerManager.containerHotbar;
            this.containerHotbar.onOpen(this.dataHotbar, this);

            this.playerMover = new PlayerMover(this);
            this.blockBreakEffect = GameObject.Instantiate(References.list.blockBreakEffect).GetComponent<BreakBlockEffect>();

            this.setMaxHealth(100);
            this.setShadow(0.75f, 0.6f);
        }

        private new void Start() {
            base.Start();

            this.chunkLoader = this.world.generator.getChunkLoader(this);
        }

        public override void onEntityUpdate() {
            // If the player is dead, don't update them.
            if (this.health <= 0) {
                return;
            }

            base.onEntityUpdate();

            this.chunkLoader.updateChunkLoader();

            this.doFallDamage();

            /*
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
            */
        }

        public override bool damage(int amount, string message) {
            this.damageEffect.startEffect();
            int oldHealth = this.health;
            this.setHealth(this.health - amount);
            this.heartEffect.startAnimation(oldHealth, this.health);
            if (this.health <= 0) {
                // Player has died
                this.contManager.closeContainer(this);

                this.scatterContainerContents(this.world, this.dataHotbar);
                this.scatterContainerContents(this.world, this.dataInventory);

                Main.hideMouse(false);
                //this.fpc.enabled = false;

                Main.singleton.openGuiScreen(GuiManager.respawn);
                GuiManager.respawn.deathMessageText.text = message;
                return true;
            }
            return false;
        }

        public override int getEntityId() {
            return 1;
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(new NbtFloat("cameraX", this.mainCamera.eulerAngles.x));
            tag.Add(this.dataHotbar.writeToNbt(new NbtCompound("hotbar")));
            tag.Add(this.dataInventory.writeToNbt(new NbtCompound("inventory")));
            tag.Add(new NbtFloat("hunger", this.hunger));
            tag.Add(new NbtFloat("hungerTimer", this.hunger));
            tag.Add(new NbtInt("selectedHotbarIndex", this.containerHotbar.getIndex()));
            tag.Add(new NbtByte("grounded", (byte)(this.onGroundLastUpdate ? 1 : 0)));
            tag.Add(new NbtFloat("lastY", this.lastGroundedY));
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
            this.containerHotbar.setIndex(tag.Get<NbtInt>("selectedHotbarIndex").IntValue);
            this.onGroundLastUpdate = tag.Get<NbtByte>("grounded").ByteValue == 1;
            this.lastGroundedY = tag.Get<NbtFloat>("lastY").FloatValue;
        }

        public ItemStack tryPickupStack(ItemStack stack) {
            if (this.health <= 0) {
                return stack;
            }
            return this.dataInventory.addItemStack(this.dataHotbar.addItemStack(stack));
        }

        public float getPickupRadius() {
            return 1.25f;
        }
        
        public void handleInput() {
            this.playerMover.updateMover();

            bool isShiftDown = Input.GetKey(KeyCode.LeftShift);
            ItemStack heldStack = this.containerHotbar.getHeldItem();

            // Find out what the player is looking at.
            PlayerRayHit playerHit = this.getPlayerRayHit();
            if (playerHit != null) {
                // We are looking at something
                if (playerHit.hitBlock()) {
                    if (Input.GetMouseButton(0)) {
                        this.blockBreakEffect.update(this, this.posLookingAt, playerHit.hitState.block, playerHit.hitState.meta);
                    }
                    if (Input.GetMouseButtonDown(1)) {
                        if (!playerHit.hitState.block.onRightClick(this.world, this, heldStack, this.posLookingAt, playerHit.hitState.meta, playerHit.getClickedBlockFace())) {
                            if (heldStack != null) {
                                ItemStack clickResult = heldStack.item.onRightClick(this.world, this, heldStack, playerHit);
                                this.containerHotbar.setHeldItem(clickResult);
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

            // Keycodes for each of the number keys across the keyboard
            for (int i = 0; i < 9; i++) {
                if(Input.GetKeyDown((KeyCode)(i + 49))) {
                    if(isShiftDown) {
                        if (this.containerHotbar.getIndex() != i) {
                            ItemStack tempIndex = this.containerHotbar.getHeldItem();
                            this.containerHotbar.setHeldItem(this.dataHotbar.getStack(i, 0));
                            this.dataHotbar.setStack(this.containerHotbar.getIndex(), 0, tempIndex);

                            this.containerHotbar.updateHudItemName();
                        }
                    } else {
                        this.containerHotbar.setSelected(i, true);
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Y)) {
                this.damage(30, "Debug Y");
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

        /// <summary>
        /// Sets the magnifying text.
        /// </summary>
        public void setMagnifyingText(string text) {
            this.magnifyingText.showAndStartFade(text, 3);
        }

        /// <summary>
        /// Sets the players hunger, clamping it between 0 and 100.
        /// </summary>
        public void setHunger(float amount) {
            if (amount > 100f) {
                amount = 100f;
            }
            if (amount < 0f) {
                amount = 0f;
            }
            this.hunger = amount;
        }

        /// <summary>
        /// Cleans up the player, destroying any associate GameObjects and disabling others.
        /// </summary>
        public void cleanupPlayerObj() {
            this.containerHotbar.gameObject.SetActive(false);
            GameObject.Destroy(this.blockBreakEffect.gameObject);
            this.heartEffect.enabled = false;
        }

        /// <summary>
        /// Configures a first time player, setting the starting inventory and the default health.
        /// </summary>
        public void setupFirstTimePlayer() {
            if(true) {
                this.dataHotbar.items[0] = new ItemStack(Block.torch, 0, 25);
                this.dataHotbar.items[1] = new ItemStack(Item.axe, 0, 12);
                this.dataHotbar.items[2] = new ItemStack(Item.shovel, 0, 16);
                this.dataHotbar.items[3] = new ItemStack(Block.rubyOre, 0, 16);
                this.dataHotbar.items[4] = new ItemStack(Item.corn, 0, 1);
                this.dataHotbar.items[5] = new ItemStack(Item.skull, 0, 25);
                this.dataHotbar.items[6] = new ItemStack(Item.coal, 0, 1);
                this.dataHotbar.items[7] = new ItemStack(Item.ruby, 0, 1);
                this.dataHotbar.items[8] = new ItemStack(Item.flesh, 0, 1);
            } else {
                this.dataHotbar.items[0] = new ItemStack(Item.pickaxe);
                this.dataHotbar.items[1] = new ItemStack(Item.shovel);
                this.dataHotbar.items[2] = new ItemStack(Item.axe);
            }

            this.health = 100;
            this.heartEffect.healthText.text = this.health + "%";
        }

        /// <summary>
        /// Returns a PlayerHitObject, representing what the player is looking at, or null if they are not looking at anything.
        /// </summary>
        private PlayerRayHit getPlayerRayHit() {
            RaycastHit hit;
            // Ignore IslandMesh, layer 11.
            bool rayHit = Physics.Raycast(new Ray(this.mainCamera.position, this.mainCamera.forward), out hit, this.getReach(), ~((1 << 2) | (1 << 11)));

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

        /// <summary>
        /// Drops an item, via 'q' or closing a container.
        /// </summary>
        public void dropItem(ItemStack stack) {
            this.world.spawnItem(stack, this.transform.position + (Vector3.up / 2), Quaternion.Euler(0, this.transform.eulerAngles.y, 0), this.transform.forward * 2.5f);
        }

        /// <summary>
        /// Reduces one from the currently held item, updating the hud and setting the slot to null if the count falls below 1.
        /// </summary>
        public void reduceHeldStackByOne() {
            this.containerHotbar.setHeldItem(this.containerHotbar.getHeldItem().safeDeduction());
        }

        /// <summary>
        /// Scatters all the contents of a container, used when the player dies.
        /// </summary>
        private void scatterContainerContents(World world, ContainerData containerData) {
            float f = 1.5f;
            ItemStack[] items = containerData.getRawItemArray();
            for (int i = 0; i < items.Length; i++) {
                Vector3 randomForce = new Vector3(Random.Range(-f, f), Random.Range(0, f), Random.Range(-f, f));
                ItemStack stack = items[i];
                if (stack != null) {
                    this.world.spawnItem(items[i], this.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), randomForce);
                    items[i] = null;
                }
            }
        }

        public float getReach() {
            return 3f;
        }

        /// <summary>
        /// Updates the players falling state, applying damage if needed.
        /// </summary>
        private void doFallDamage() {
            bool onGround = this.playerMover.isGrounded();

            if (onGround && !this.onGroundLastUpdate) {
                // We hit the ground from falling
                float dif = this.lastGroundedY - this.transform.position.y;
                if (dif > 4) {
                    this.damage(Mathf.RoundToInt(dif) * 3, "Fell to far!");
                }
            }

            if (onGround) {
                this.lastGroundedY = this.transform.position.y;
            }

            this.onGroundLastUpdate = onGround;
        }
    }
}