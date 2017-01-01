using UnityEngine;
using UnityEngine.UI;

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
    private BreakBlockEffect blockBreakEffect;
    public Transform mainCamera;
    public Container containerElement;

    public float reach = 3.5f;
    public float magnifyingTimer;
    public BlockPos posLookingAt;
    public ItemStack heldStack;

    public ContainerHotbar containerHotbar;
    public ContainerDataHotbar hotbarData;
    public ContainerData inventoryData;

    public new void Awake() {
        base.Awake();

        this.mainCamera = Camera.main.transform;

        this.hotbarData = new ContainerDataHotbar();
        this.hotbarData.addItemStack(new ItemStack(Item.goldPickaxe, 0));
        this.hotbarData.addItemStack(new ItemStack(Block.glorb, 0, 16));
        this.hotbarData.addItemStack(new ItemStack(Block.lava, 4));
        this.hotbarData.addItemStack(new ItemStack(Item.goldSword));
        this.hotbarData.addItemStack(new ItemStack(Item.pebble, 0));
        this.hotbarData.addItemStack(new ItemStack(Item.magnifyingGlass, 2));
        this.hotbarData.addItemStack(new ItemStack(Block.mushroom, 0));
        this.hotbarData.addItemStack(new ItemStack(Block.poisonMushroom, 3));
        this.hotbarData.addItemStack(new ItemStack(Block.mossyBrick, 0));
        this.containerHotbar = GameObject.Instantiate(this.containerHotbarPrefab).GetComponent<ContainerHotbar>();
        this.containerHotbar.initContainer(this.hotbarData, this);
        this.inventoryData = new ContainerData(2, 2);

        this.blockBreakEffect = GameObject.Instantiate(this.blockBreakPrefab).GetComponent<BreakBlockEffect>();

        this.cc = this.GetComponent<CharacterController>();

        this.setHealth(10);
    }

    public override void onEntityUpdate() {
        base.onEntityUpdate();

        this.verticalVelocity += Physics.gravity.y * Time.deltaTime;
        Vector3 playerMovement = new Vector3(0, this.verticalVelocity, 0);

        if (this.containerElement == null) {
            playerMovement = this.getPlayerMovement();

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
                        ItemStack stack = this.hotbarData.getHeldItem();
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
                ItemStack stack = this.hotbarData.getHeldItem();
                if (stack != null) {
                    this.hotbarData.setHeldItem(stack.item.onRightClick(this.world, this, stack, playerHit));
                }
            }
        }

        this.cc.Move(playerMovement * Time.deltaTime);

        //Magnifying text
        if (this.magnifyingTimer > 0) {
            this.magnifyingTimer -= Time.deltaTime;
        }
        if (this.magnifyingTimer <= 0) {
            this.magnifyingText.color = Color.Lerp(this.magnifyingText.color, Color.clear, 3 * Time.deltaTime);
        }
    }

    public override void onEntityCollision(Entity otherEntity) {
        if(otherEntity is EntityItem) {
            ItemStack s = this.hotbarData.addItemStack(((EntityItem)otherEntity).stack);
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
        if(Input.GetKeyDown(KeyCode.Q)) {
            ItemStack stack = this.hotbarData.dropItem(this.hotbarData.index, false /* Input.GetKey(KeyCode.LeftControl) */);
            if(stack != null) {
                this.dropItem(stack);
            }
        }
        float f = Input.GetAxis("Mouse ScrollWheel");
        if(f != 0) {
            this.containerHotbar.scroll(f > 0 ? 1 : (f < 0 ? -1 : 0));
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            this.openContainer(this.containerInventoryPrefab, this.inventoryData);
        }
    }

    //Opens and initiates a container
    public void openContainer(GameObject containerObj, ContainerData containerData) {
        this.containerElement = GameObject.Instantiate(containerObj).GetComponent<Container>();
        this.containerElement.transform.SetParent(this.transform);
        this.containerElement.initContainer(containerData, this);
        VoxelEngine.setMouseLock(false);
    }

    //Closes the open container, doing any required cleanup
    public void closeContainer() {
        if(this.heldStack != null) {
            this.dropItem(this.heldStack);
            this.heldStack = null;
        }
        GameObject.Destroy(this.containerElement.gameObject);
        VoxelEngine.setMouseLock(true);
    }

    private void dropItem(ItemStack stack) {
        this.world.spawnItem(stack, this.transform.position + (Vector3.up / 2) + this.transform.forward, Quaternion.Euler(0, this.transform.eulerAngles.y, 0));
    }

    private Vector3 getPlayerMovement() {
        //Rotation
        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0.0f, rotLeftRight, 0.0f);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90); //Stop the player from looking to far up or down

        this.mainCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        //Movement
        float f = Input.GetKey(KeyCode.LeftShift) ? 9.0f : 5.0f;
        float forwardSpeed = Input.GetAxis("Vertical") * f;
        float sideSpeed = Input.GetAxis("Horizontal") * f;

        if (Input.GetButtonDown("Jump") && this.cc.isGrounded) {
            this.verticalVelocity = jumpSpeed;
        }

        return transform.rotation * new Vector3(sideSpeed, this.verticalVelocity, forwardSpeed);
    }

    public void cleanupObject() {
        GameObject.Destroy(this.containerHotbar.gameObject);
        GameObject.Destroy(this.blockBreakEffect.gameObject);
    }
}
