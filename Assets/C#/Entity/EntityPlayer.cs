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
    public GameObject blockBreakPrefab;

    private CharacterController cc;
    private BreakBlockEffect blockBreakEffect;
    private BlockPos posLookingAt;
    public InventoryPlayer pInventory;
    private bool showDebugInfo = true;
    private float reach = 3.5f;
    public Camera mainCamera;

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
        this.pInventory.addItemStack(new ItemStack(Block.dirt, 0, 16));
        this.pInventory.addItemStack(new ItemStack(Item.pebble, 0));
        this.pInventory.addItemStack(new ItemStack(Block.gravel, 2));
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


        if (hit.transform != null) {
            float f = Vector3.Distance(this.mainCamera.transform.position, hit.point);
            if (hit.transform.tag == "Chunk" && f <= this.reach) {
                Block block = this.world.getBlock(this.posLookingAt);
                byte meta = this.world.getMeta(this.posLookingAt);

                if (Input.GetMouseButton(0)) {
                    this.blockBreakEffect.update(this, this.posLookingAt, block, meta);
                }
                if (Input.GetMouseButtonDown(1)) {
                    block.onRightClick(this.world, this.posLookingAt, meta);                  
                }
            }
            else if (hit.transform.tag == "Entity" && f <= this.reach) {
                Entity e = hit.transform.GetComponent<Entity>();
                if (Input.GetMouseButtonDown(0)) {
                    e.onEntityHit(this);
                }
                if(Input.GetMouseButtonDown(1)) {
                    e.onEntityInteract(this);
                }
            }
        }

        //Right click
        if (rayHit && Input.GetMouseButtonDown(1)) {
            ItemStack stack = this.pInventory.hotbar[this.pInventory.index];
            if(stack != null) {
                this.pInventory.hotbar[this.pInventory.index] = stack.item.onRightClick(this.world, this, stack, hit);
            }
        }

        this.handleInput();
        this.updateDebugInfo();
        this.pInventory.drawHotbar();
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
        this.pInventory.scroll(f > 0 ? 1 : (f < 0 ? -1 : 0));
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
