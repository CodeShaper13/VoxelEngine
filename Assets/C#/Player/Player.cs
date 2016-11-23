using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public World world;
    public Text debugTextGUI;
    public GameObject blockBreakObj;
    public BreakBlockEffect blockBreakEffect;

    //How long the player has been hitting a block
    public float mineTimer;
    //The last pos the player has been looking at
    public BlockPos lastLookAt;
    public bool validLook;

    public PlayerInventory pInventory;

    public Camera hudCamera;

    private bool showDebugInfo = true;
    private float reach = 4.0f;

    void Awake() {
        Item.initBlockItems();

        this.blockBreakEffect = GameObject.Instantiate(this.blockBreakObj).GetComponent<BreakBlockEffect>();
        this.blockBreakEffect.gameObject.SetActive(false);
        this.pInventory = new PlayerInventory(this.hudCamera);
    }

    void Update() {
        RaycastHit hit;
        this.validLook = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, this.reach);
        BlockPos p = BlockPos.fromRaycast(hit, false);
        if(!(p.Equals(this.lastLookAt))) {
            this.mineTimer = 0.0f;
        }
        this.lastLookAt = p;
        if(!this.validLook) {
            this.mineTimer = 0.0f;
        }
        if(Input.GetMouseButton(0) && this.validLook) {
            this.mineTimer += Time.deltaTime;
        }
        if(Input.GetMouseButton(0) && this.validLook) {
            Block b = this.world.getBlock(this.lastLookAt);
            if(b != Block.air) {
                if (this.mineTimer >= b.mineTime) {
                    //get drops and spawn them in the world
                    ItemStack[] stacks = b.getDrops(this.world.getMeta(this.lastLookAt));
                    this.world.setBlock(this.lastLookAt, Block.air);
                    foreach (ItemStack s in stacks) {
                        this.world.spawnItem(s, this.lastLookAt.toVector());
                    }
                    this.mineTimer = 0.0f;
                }
            } else {
                print("ERROR  We are trying to break air?");
                return;
            }
        }

        if (Input.GetMouseButtonDown(1) && this.validLook) {
            this.placeBlock(hit, Block.dirt, true);
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

    private void placeBlock(RaycastHit hit, Block block, bool adjacent = false) {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null) {
            return;
        }

        BlockPos pos = BlockPos.fromRaycast(hit, adjacent);
        if(this.world.getBlock(pos) == Block.air) {
            this.world.setBlock(pos, block);
        }
    }

    private void handleInput() {
        if(Input.GetKeyDown(KeyCode.F3)) {
            this.showDebugInfo = !this.showDebugInfo;
        }
        if(Input.GetKeyDown(KeyCode.Q)) {
            ItemStack s = this.pInventory.dropItem(0);//, Input.GetKey(KeyCode.LeftControl));
            if(s != null) {
                this.world.spawnItem(s, this.transform.position + (Vector3.up / 2) + this.transform.forward);
            }
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
            s.Append("Looking At: " + this.lastLookAt.ToString() + "\n");
            this.debugTextGUI.text = s.ToString();
        } else {
            this.debugTextGUI.text = string.Empty;
        }
    }
}
