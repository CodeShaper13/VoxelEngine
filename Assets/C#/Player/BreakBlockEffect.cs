using UnityEngine;

public class BreakBlockEffect : MonoBehaviour {
    private float mineTimer = 0.0f;
    private bool isTerminated = true;

    private MeshRenderer mr;
    private MeshFilter mf;
    private ParticleSystem ps;

    void Awake() {
        this.mr = this.GetComponent<MeshRenderer>();
        this.mf = this.GetComponent<MeshFilter>();
        this.ps = this.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    //Beging the breaking of a block
    public void beginBreak(Vector3 pos, Block block, byte meta) {
        this.mr.enabled = true;
        MeshData meshData = new MeshData();
        BlockModel model = block.getModel(meta);
        model.preRender(block, meta, meshData);
        model.renderBlock(0, 0, 0, new bool[6] {true, true, true, true, true, true });
        this.mf.mesh = model.meshData.toMesh();
        this.transform.position = pos;

        //set the right texture
        int x = block.getTexturePos(Direction.UP, 0).x;
        int y = Mathf.Abs(block.getTexturePos(Direction.UP, 0).y - 3);

        this.ps.Play();
    }

    public void terminate() {
        this.mineTimer = 0.0f;
        this.mr.enabled = false;
        this.ps.Stop();
        this.isTerminated = true;
    }

    public void update(Player player, Block block, byte meta) {
        if(this.isTerminated) {
            this.beginBreak(player.posLookingAt.toVector(), block, meta);
            this.isTerminated = false;
        }

        this.mineTimer += Time.deltaTime;
        if (block != Block.air) { //Hacky safety check
            if (this.mineTimer >= block.mineTime) {
                player.world.setBlock(player.posLookingAt, Block.air);
                foreach (ItemStack s in block.getDrops(meta)) {
                    float f = 0.5f;
                    Vector3 offset = new Vector3(Random.Range(-f, f), Random.Range(-f, f), Random.Range(-f, f));
                    player.world.spawnItem(s, player.posLookingAt.toVector() + offset, Quaternion.Euler(0, Random.Range(0, 360), 0));
                }
                this.mineTimer = 0.0f;
            }
        }
        else {
            //print("ERROR  We are trying to break air?");
        }
    }
}
