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

namespace VoxelEngine.Entities {

    public class EntityPlayer : Entity {
        //Constants
        private float mouseSensitivity = 3f; //2.5f
        private float jumpSpeed = 5.0f;

        private float verticalRotation = 0.0f;
        private float verticalVelocity;

        public Text magnifyingText;
        public RawImage healthBarImage;
        public GameObject blockBreakPrefab;
        public GameObject containerHotbarPrefab;
        public GameObject containerInventoryPrefab;

        private CharacterController cc;
        public FirstPersonController fpc;
        private BreakBlockEffect blockBreakEffect;
        public Transform mainCamera;
        public Container containerElement;

        public float reach = 3.5f;
        public float magnifyingTimer;
        public BlockPos posLookingAt;
        public ItemStack heldStack;

        public ContainerHotbar containerHotbar;

        public ContainerDataHotbar dataHotbar;
        public ContainerData dataInventory;

        public new void Awake() {
            base.Awake();

            this.mainCamera = Camera.main.transform;
            this.cc = this.GetComponent<CharacterController>();
            this.fpc = this.GetComponent<FirstPersonController>();

            this.dataHotbar = new ContainerDataHotbar();
            this.dataInventory = new ContainerData(2, 2);

            //Setup the hotbar
            this.containerHotbar = GameObject.Instantiate(this.containerHotbarPrefab).GetComponent<ContainerHotbar>();
            this.containerHotbar.initContainer(this.dataHotbar, this);

            this.blockBreakEffect = GameObject.Instantiate(this.blockBreakPrefab).GetComponent<BreakBlockEffect>();
        }

        public override void onEntityUpdate() {
            base.onEntityUpdate();

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
                            ItemStack stack = this.dataHotbar.getHeldItem();
                            float damage = 1;
                            if (stack != null && stack.item is ItemSword) {
                                damage = ((ItemSword)stack.item).damageAmount;
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
                    ItemStack stack = this.dataHotbar.getHeldItem();
                    if (stack != null) {
                        this.dataHotbar.setHeldItem(stack.item.onRightClick(this.world, this, stack, playerHit));
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
        }

        public override void onEntityCollision(Entity otherEntity) {
            if (otherEntity is EntityItem) {
                ItemStack s = this.dataHotbar.addItemStack(((EntityItem)otherEntity).stack);
                if (s == null) {
                    this.world.killEntity(otherEntity);
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
            this.dataHotbar.addItemStack(new ItemStack(Item.goldPickaxe, 0));
            this.dataHotbar.addItemStack(new ItemStack(Block.glorb, 0, 16));
            this.dataHotbar.addItemStack(new ItemStack(Block.lava, 4));
            this.dataHotbar.addItemStack(new ItemStack(Item.goldSword));
            this.dataHotbar.addItemStack(new ItemStack(Item.pebble, 0));
            this.dataHotbar.addItemStack(new ItemStack(Item.magnifyingGlass, 2));
            this.dataHotbar.addItemStack(new ItemStack(Block.mushroom, 0));
            this.dataHotbar.addItemStack(new ItemStack(Block.poisonMushroom, 3));
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
                if (hit.transform.tag == "Chunk") {
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
                this.openContainer(this.containerInventoryPrefab, this.dataInventory);
            }
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
            this.world.spawnItem(stack, this.transform.position + (Vector3.up / 2) + this.transform.forward, Quaternion.Euler(0, this.transform.eulerAngles.y, 0));
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
            tag.Add(new NbtFloat("cameraVertical", this.verticalRotation));
            tag.Add(this.dataHotbar.writeToNbt(new NbtCompound("hotbar")));
            tag.Add(this.dataInventory.writeToNbt(new NbtCompound("inventory")));
            //TODO jump, maybe general up/down for entities
            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);
            this.verticalRotation = tag.Get<NbtFloat>("cameraVertical").FloatValue;
            this.mainCamera.localRotation = Quaternion.Euler(this.verticalRotation, 0, 0);
            this.dataHotbar.readFromNbt(tag.Get<NbtCompound>("hotbar"));
            this.dataInventory.readFromNbt(tag.Get<NbtCompound>("inventory"));
        }
    }
}
