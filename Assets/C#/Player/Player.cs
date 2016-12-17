using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    //Constants
    private float movementSpeed = 5.0f; //5
    private float mouseSensitivity = 3f; //2.5f
    private float jumpSpeed = 5.0f;

    //State
    private float verticalRotation = 0.0f;
    private float verticalVelocity = 0.0f;

    public World world;
    public Text debugTextGUI;
    public GameObject blockBreakObj;
    public BreakBlockEffect blockBreakEffect;

    //The last pos the player has been looking at
    public BlockPos posLookingAt;

    public PlayerInventory pInventory;

    private bool showDebugInfo = true;
    private float reach = 3.5f;

    public CharacterController cc;

    void Awake() {
        Cursor.visible = false;

        Item.initBlockItems();

        this.blockBreakEffect = GameObject.Instantiate(this.blockBreakObj).GetComponent<BreakBlockEffect>();
        this.pInventory = new PlayerInventory();

        this.cc = this.GetComponent<CharacterController>();
    }

    void Update() {
        //Move the player
        this.handleMovement();

        RaycastHit hit;
        bool rayHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, this.reach);

        //Find out what we are looking at
        BlockPos p = BlockPos.fromRaycast(hit, false);

        if (rayHit == false || !(p.Equals(this.posLookingAt)) || !Input.GetMouseButton(0)) {
            //If we are looking at a new thing, reset/remove the block break effect
            this.blockBreakEffect.terminate();
        }
        this.posLookingAt = p;


        if (hit.transform != null) {
            if (hit.transform.tag == "Chunk") {
                Block block = this.world.getBlock(this.posLookingAt);
                byte meta = this.world.getMeta(this.posLookingAt);

                if (Input.GetMouseButton(0)) {
                    this.blockBreakEffect.update(this, block, meta);
                }
                if (Input.GetMouseButtonDown(1)) {
                    block.onRightClick(this.world, this.posLookingAt, meta);                  
                }
            }
            else if (hit.transform.tag == "Entity") {
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
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if(entity != null) {
            entity.onPlayerTouch(this);
        }
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
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90); //Stop the player from looking to far up or down
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        //Movement
        float f = this.getSpeed();
        float forwardSpeed = Input.GetAxis("Vertical") * f;
        float sideSpeed = Input.GetAxis("Horizontal") * f;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && cc.isGrounded) {
            verticalVelocity = jumpSpeed;
        }

        Vector3 speed = transform.rotation * new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

        cc.Move(speed * Time.deltaTime);
    }
}
