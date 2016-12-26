using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EntityPlayer : Entity {
    //Constants
    private float mouseSensitivity = 3f; //2.5f
    private float jumpSpeed = 5.0f;

    private float verticalRotation = 0.0f;
    private float verticalVelocity;

    public Text debugTextGUI;
    public Text magnifyingText;
    public RawImage healthBarImage;
    public GameObject blockBreakPrefab;

    private CharacterController cc;
    private BreakBlockEffect blockBreakEffect;
    public Transform mainCamera;
    public Container containerElement;

    public GameObject temp_prefab;
    public GameObject hotbar_prefab;

    public float reach = 3.5f;
    public float magnifyingTimer;
    private BlockPos posLookingAt;
    private bool showDebugInfo = true;

    private ContainerHotbar containerHotbar;
    public ContainerDataHotbar inventoryData;    

    public new void Awake() {
        base.Awake();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        this.mainCamera = Camera.main.transform;

        this.inventoryData = new ContainerDataHotbar();
        this.inventoryData.addItemStack(new ItemStack(Item.goldPickaxe, 0));
        this.inventoryData.addItemStack(new ItemStack(Block.uraniumOre, 2));
        this.inventoryData.addItemStack(new ItemStack(Block.lava, 4));
        this.inventoryData.addItemStack(new ItemStack(Item.goldSword));
        this.inventoryData.addItemStack(new ItemStack(Item.pebble, 0));
        this.inventoryData.addItemStack(new ItemStack(Item.magnifyingGlass, 2));
        this.inventoryData.addItemStack(new ItemStack(Block.mushroom, 0));
        this.inventoryData.addItemStack(new ItemStack(Block.poisonMushroom, 3));
        this.inventoryData.addItemStack(new ItemStack(Block.mossyBrick, 0));

        this.blockBreakEffect = GameObject.Instantiate(this.blockBreakPrefab).GetComponent<BreakBlockEffect>();
        this.containerHotbar = GameObject.Instantiate(this.hotbar_prefab).GetComponent<ContainerHotbar>();
        this.containerHotbar.initContainer(this.inventoryData);

        this.cc = this.GetComponent<CharacterController>();

        this.setHealth(10);
    }

    public new void Update() {
        base.Update();

        this.verticalVelocity += Physics.gravity.y * Time.deltaTime;
        Vector3 playerMovement = new Vector3(0, this.verticalVelocity, 0);

        if(this.containerElement != null) {
            this.containerElement.drawnContents();

            if (Input.GetKeyDown(KeyCode.Escape)) {
                GameObject.Destroy(this.containerElement.gameObject);
            }
        } else {
            playerMovement = this.movePlayer();

            PlayerRayHit playerHit = this.updatePlayerRayHit();         

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
                        ItemStack stack = this.inventoryData.getHeldItem();
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
                ItemStack stack = this.inventoryData.getHeldItem();
                if (stack != null) {
                    this.inventoryData.setHeldItem(stack.item.onRightClick(this.world, this, stack, playerHit));
                }
            }

            this.handleInput();
        }

        this.cc.Move(playerMovement * Time.deltaTime);

        //Magnifying text
        if (this.magnifyingTimer > 0) {
            this.magnifyingTimer -= Time.deltaTime;
        }
        if (this.magnifyingTimer <= 0) {
            this.magnifyingText.color = Color.Lerp(this.magnifyingText.color, Color.clear, 3 * Time.deltaTime);
        }

        this.updateDebugInfo();
        this.containerHotbar.drawnContents();
    }

    public override void onEntityCollision(Entity otherEntity) {
        if(otherEntity is EntityItem) {
            ItemStack s = this.inventoryData.addItemStack(((EntityItem)otherEntity).stack);
            if (s == null) {
                GameObject.Destroy(otherEntity.gameObject);
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

    private PlayerRayHit updatePlayerRayHit() {
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

    private void handleInput() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            ItemStack s = this.inventoryData.dropItem(0, false /* Input.GetKey(KeyCode.LeftControl) */);
            if(s != null) {
                this.world.spawnItem(s, this.transform.position + (Vector3.up / 2) + this.transform.forward, Quaternion.Euler(0, this.transform.eulerAngles.y, 0));
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.visible = true;
        }
        float f = Input.GetAxis("Mouse ScrollWheel");
        if(f != 0) {
            this.containerHotbar.scroll(f > 0 ? 1 : (f < 0 ? -1 : 0));
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            this.openContainer();
        }
    }

    public void openContainer() {
        this.containerElement = GameObject.Instantiate(this.temp_prefab).GetComponent<Container>();
        this.containerElement.transform.SetParent(this.transform);
        ContainerData cd = new ContainerData(2, 2);
        cd.setStack(0, 0, new ItemStack(Item.pebble));
        this.containerElement.initContainer(cd);
    }

    private void updateDebugInfo() {
        if (Input.GetKeyDown(KeyCode.F3)) {
            this.showDebugInfo = !this.showDebugInfo;
        }
        if (this.showDebugInfo) {
            StringBuilder s = new StringBuilder();
            s.Append("DEBUG: (Press F3 to toggle)\n");
            s.Append("WASD for movement, Q to drop item, R to save world\n\n");
            s.Append("Position: " + this.transform.position.ToString() + "\n");
            s.Append("Rotation: " + this.transform.eulerAngles.ToString() + "\n");
            s.Append("Looking At: " + this.world.getBlock(this.posLookingAt).name + ":" + this.world.getMeta(this.posLookingAt) + " " + this.posLookingAt.ToString() + "\n");
            s.Append(this.world.worldData.worldName + " " + this.world.worldData.seed);
            this.debugTextGUI.text = s.ToString();
        } else {
            this.debugTextGUI.text = string.Empty;
        }
    }

    private Vector3 movePlayer() {
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
}
