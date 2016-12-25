using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EntityPlayer : Entity {
    //Constants
    private float mouseSensitivity = 3f; //2.5f
    private float jumpSpeed = 5.0f;

    //State
    private float verticalRotation = 0.0f;
    private float verticalVelocity = 0.0f;

    public Text debugTextGUI;
    public RawImage healthBarImage;
    public Text magnifyingText;
    public Text itemNameText;
    public GameObject blockBreakPrefab;

    private CharacterController cc;
    private BreakBlockEffect blockBreakEffect;
    private BlockPos posLookingAt;
    public InventoryPlayer pInventory;
    private bool showDebugInfo = true;
    public float reach = 3.5f;
    public Camera mainCamera;
    public float magnifyingTimer;

    public new void Awake() {
        base.Awake();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        this.mainCamera = Camera.main;

        this.blockBreakEffect = GameObject.Instantiate(this.blockBreakPrefab).GetComponent<BreakBlockEffect>();
        this.pInventory = new InventoryPlayer();

        this.cc = this.GetComponent<CharacterController>();

        this.setHealth(10);

        this.pInventory.addItemStack(new ItemStack(Item.goldPickaxe, 0));
        this.pInventory.addItemStack(new ItemStack(Block.uraniumOre, 2));
        this.pInventory.addItemStack(new ItemStack(Block.rubyOre, 4));
        this.pInventory.addItemStack(new ItemStack(Block.dirt));
        this.pInventory.addItemStack(new ItemStack(Item.pebble, 0));
        this.pInventory.addItemStack(new ItemStack(Item.magnifyingGlass, 2));
        this.pInventory.addItemStack(new ItemStack(Block.mushroom, 0));
        this.pInventory.addItemStack(new ItemStack(Block.poisonMushroom, 3));
        this.pInventory.addItemStack(new ItemStack(Block.mossyBrick, 0));
    }

    public new void Update() {
        base.Update();

        //Move the player
        this.handleMovement();

        RaycastHit hit;
        bool rayHit = Physics.Raycast(this.mainCamera.transform.position, this.mainCamera.transform.forward, out hit);

        //Find out what we are looking at
        BlockPos p = BlockPos.fromRaycast(hit, false);

        if (rayHit == false || !(p.Equals(this.posLookingAt)) || !Input.GetMouseButton(0)) {
            //If we are looking at a new thing, reset/remove the block break effect
            this.blockBreakEffect.terminate();
        }
        this.posLookingAt = p;

        PlayerRayHit playerHit = null;
        if(hit.transform != null) {
            if (hit.transform.tag == "Chunk") {
                playerHit = new PlayerRayHit(this.world.getBlock(this.posLookingAt), this.world.getMeta(this.posLookingAt), this.posLookingAt, hit);
            }
            else if (hit.transform.tag == "Entity") {
                playerHit = new PlayerRayHit(hit.transform.GetComponent<Entity>(), hit);
            }
        }

        if (playerHit != null) {
            if (playerHit.state != null && playerHit.unityRaycastHit.distance <= this.reach) {
                if (Input.GetMouseButton(0)) {
                    this.blockBreakEffect.update(this, this.posLookingAt, playerHit.state.block, playerHit.state.meta);
                }
                if (Input.GetMouseButtonDown(1)) {
                    playerHit.state.block.onRightClick(this.world, this.posLookingAt, playerHit.state.meta);                    
                }
            }
            else if (playerHit.entity != null && playerHit.unityRaycastHit.distance <= this.reach) {
                if (Input.GetMouseButtonDown(0)) {
                    playerHit.entity.onEntityHit(this);
                }
                if(Input.GetMouseButtonDown(1)) {
                    playerHit.entity.onEntityInteract(this);
                }
            }
        }

        //Right click
        if (rayHit && Input.GetMouseButtonDown(1)) {
            ItemStack stack = this.pInventory.hotbar[this.pInventory.index];
            if(stack != null) {
                this.pInventory.hotbar[this.pInventory.index] = stack.item.onRightClick(this.world, this, stack, playerHit);
            }
        }

        this.handleInput();
        this.updateDebugInfo();
        this.pInventory.drawHotbar();

        //Magnifying text
        if(this.magnifyingTimer > 0) {
            this.magnifyingTimer -= Time.deltaTime;
        }
        if(this.magnifyingTimer <= 0) {
            this.magnifyingText.color = Color.Lerp(this.magnifyingText.color, Color.clear, 3 * Time.deltaTime);
        }
    }

    public override void onEntityCollision(Entity otherEntity) {
        if(otherEntity is EntityItem) {
            ItemStack s = this.pInventory.addItemStack(((EntityItem)otherEntity).stack);
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

    private void handleInput() {
        if(Input.GetKeyDown(KeyCode.F3)) {
            this.showDebugInfo = !this.showDebugInfo;
        }
        if(Input.GetKeyDown(KeyCode.Q)) {
            ItemStack s = this.pInventory.dropItem(0, Input.GetKey(KeyCode.LeftControl));
            if(s != null) {
                this.world.spawnItem(s, this.transform.position + (Vector3.up / 2) + this.transform.forward, Quaternion.Euler(0, this.transform.eulerAngles.y, 0));
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.visible = true;
        }
        float f = Input.GetAxis("Mouse ScrollWheel");
        if(f != 0) {
            this.pInventory.scroll(f > 0 ? 1 : (f < 0 ? -1 : 0));
            ItemStack stack = this.pInventory.getHeldItem();
            this.itemNameText.text = stack == null ? string.Empty : stack.item.name;
        }
    }

    private void updateDebugInfo() {
        if(this.showDebugInfo) {
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

    private float getSpeed() {
        return Input.GetKey(KeyCode.LeftShift) ? 9.0f : 5.0f; //SPEED
    }

    private void handleMovement() {
        //Rotation
        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0.0f, rotLeftRight, 0.0f);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90); //Stop the player from looking to far up or down
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        //Movement
        float f = this.getSpeed();
        float forwardSpeed = Input.GetAxis("Vertical") * f;
        float sideSpeed = Input.GetAxis("Horizontal") * f;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && this.cc.isGrounded) {
            verticalVelocity = jumpSpeed;
        }

        Vector3 speed = transform.rotation * new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

        cc.Move(speed * Time.deltaTime);
    }
}
