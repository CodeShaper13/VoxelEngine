using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public World world;
    public Text debugTextGUI;
    public GameObject blockBreakObj;

    //How long the player has been hitting a block
    private float mineTimer;
    private BlockPos lastLookAt;

    private bool showDebugInfo = true;

    public float reach = 4.0f;

    void Awake() {
        if(this.debugTextGUI == null || this.blockBreakObj == null) {
            throw new Exception("ERROR!  Make sure all fields in the Player are not null!");
        }
        this.blockBreakObj = GameObject.Instantiate(this.blockBreakObj);
        this.blockBreakObj.SetActive(false);
    }

    void Update() {
        RaycastHit hit;
        bool hitFlag = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, this.reach);

        BlockPos p = BlockPos.fromRaycast(hit, false);
        if(!(p.Equals(this.lastLookAt))) {
            //reset mining
            this.mineTimer = 0.0f;
        }
        this.lastLookAt = p;

        this.blockBreakObj.SetActive(Input.GetMouseButton(0));

        if(Input.GetMouseButton(0)) {
            this.mineTimer += Time.deltaTime;
            this.blockBreakObj.transform.position = this.lastLookAt.toVector();
            if(this.world.getBlock(this.lastLookAt).mineTime <= this.mineTimer) {
                this.world.setBlock(this.lastLookAt, Block.air);
                this.mineTimer = 0.0f;
            }
        }

        if (Input.GetMouseButtonDown(1) && hitFlag) {
            this.setBlock(hit, Block.dirt, true);
        }

        this.handleInput();

        this.updateDebugInfo();
    }

    private bool setBlock(RaycastHit hit, Block block, bool adjacent = false) {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null) {
            return false;
        }

        BlockPos pos = BlockPos.fromRaycast(hit, adjacent);
        chunk.world.setBlock(pos, block);

        return true;
    }

    private void handleInput() {
        this.showDebugInfo = Input.GetKeyDown(KeyCode.F3);
        if(Input.GetKeyDown(KeyCode.Q)) {
            this.world.spawnItem(new ItemStack(Item.basicItem), this.transform.position + (Vector3.up / 2) + this.transform.forward);
        }
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
