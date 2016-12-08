using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    //Constants
    private float movementSpeed = 5.0f; //5
    private float mouseSensitivity = 3f; //2.5f
    private float jumpSpeed = 5.0f;
    private float upDownRange = 60.0f;
    //State
    private float verticalRotation = 0.0f;
    private float verticalVelocity = 0.0f;

    public World world;
    public Text debugTextGUI;
    public GameObject blockBreakObj;
    public BreakBlockEffect blockBreakEffect;

    //How long the player has been hitting a block
    public float mineTimer;
    //The last pos the player has been looking at
    public BlockPos posLookingAt;
    public bool rayHit;

    public PlayerInventory pInventory;

    public Camera hudCamera;

    private bool showDebugInfo = true;
    private float reach = 3.5f;

    public CharacterController cc;

    void Awake() {
        Cursor.visible = false;

        Item.initBlockItems();

        this.blockBreakEffect = GameObject.Instantiate(this.blockBreakObj).GetComponent<BreakBlockEffect>();
        this.blockBreakEffect.gameObject.SetActive(false);
        this.pInventory = new PlayerInventory(this.hudCamera);

        this.cc = this.GetComponent<CharacterController>();
    }

    void Update() {
        this.handleMovement();

        RaycastHit hit;
        this.rayHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, this.reach);

        BlockPos p = BlockPos.fromRaycast(hit, false);
        if (!(p.Equals(this.posLookingAt)) || this.rayHit == false) {
            this.mineTimer = 0.0f;
        }
        this.posLookingAt = p;

        this.blockBreakEffect.gameObject.SetActive(false);
        if (hit.transform != null && hit.transform.tag != null) {
            if (hit.transform.tag == "Chunk") {
                this.blockBreakEffect.gameObject.SetActive(true);
                this.blockBreakEffect.transform.position = this.posLookingAt.toVector();

                if(Input.GetMouseButton(0)) {
                    this.mineTimer += Time.deltaTime;
                    Block block = this.world.getBlock(this.posLookingAt);
                    if (block != Block.air) { //Hacky safety check
                        if (this.mineTimer >= block.mineTime) {
                            ItemStack[] stacks = block.getDrops(this.world.getMeta(this.posLookingAt));
                            this.world.setBlock(this.posLookingAt, Block.air);
                            foreach (ItemStack s in stacks) {
                                this.world.spawnItem(s, this.posLookingAt.toVector());
                            }
                            this.mineTimer = 0.0f;
                        }
                    }
                    else { print("ERROR  We are trying to break air?"); }
                }
            }
            else if (hit.transform.tag == "Entity") {
                if(Input.GetMouseButtonDown(0)) {
                    Entity e = hit.transform.GetComponent<Entity>();
                    e.onPlayerHit(this);
                }
            }
        }

        //Right click
        if (this.rayHit && Input.GetMouseButtonDown(1)) {
            ItemStack s = this.pInventory.hotbar[this.pInventory.index];
            if(s != null) {
                this.pInventory.hotbar[this.pInventory.index] = s.item.onRightClick(this.world, s, hit);
            }
        }

        this.handleInput();
        this.updateDebugInfo();
        this.pInventory.drawHotbar();
    }

    void OnCollisionEnter(Collision collision) {
        EntityItem entityItem = collision.gameObject.GetComponent<EntityItem>();
        if(entityItem != null) {
            ItemStack s = this.pInventory.addItemStack(entityItem.stack);
            if(s == null) {
                GameObject.Destroy(entityItem.gameObject);
            }
        }
    }

    private void handleInput() {
        if(Input.GetKeyDown(KeyCode.F3)) {
            this.showDebugInfo = !this.showDebugInfo;
        }
        if(Input.GetKeyDown(KeyCode.Q)) {
            ItemStack s = this.pInventory.dropItem(0, false); //Input.GetKey(KeyCode.LeftControl));
            if(s != null) {
                this.world.spawnItem(s, this.transform.position + (Vector3.up / 2) + this.transform.forward);
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
            s.Append("DEBUG: (Press F3 to toggle)\n\n");
            s.Append("Position: " + this.transform.position.ToString() + "\n");
            s.Append("Rotation: " + this.transform.eulerAngles.ToString() + "\n");
            s.Append("Looking At: " + this.posLookingAt.ToString() + "\n");
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
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        //Movement
        float f = this.getSpeed();
        float forwardSpeed = Input.GetAxis("Vertical") * f;
        float sideSpeed = Input.GetAxis("Horizontal") * f;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && cc.isGrounded) {
            verticalVelocity = jumpSpeed;
        }

        Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

        speed = transform.rotation * speed;

        cc.Move(speed * Time.deltaTime);
    }
}
