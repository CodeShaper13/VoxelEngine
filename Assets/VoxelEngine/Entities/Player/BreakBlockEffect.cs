using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Render;
using VoxelEngine.Render.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Entities.Player {

    public class BreakBlockEffect : MonoBehaviour {
        private float mineTimer = 0.0f;
        private bool isTerminated = true;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private ParticleSystem ps;

        void Awake() {
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.ps = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        }

        //Beging the breaking of a block
        public void beginBreak(Vector3 pos, Block block, byte meta) {
            this.meshRenderer.enabled = true;
            MeshData meshData = new MeshData();
            BlockModel model = block.model;
            model.renderBlock(block, meta, meshData, 0, 0, 0, new bool[6] { true, true, true, true, true, true });
            this.meshFilter.mesh = model.meshData.toMesh();
            this.transform.position = pos;

            //set the right texture
            //int x = block.getTexturePos(Direction.UP, 0).x;
            //int y = Mathf.Abs(block.getTexturePos(Direction.UP, 0).y - 3);

            this.ps.Play();
        }

        public void terminate() {
            this.mineTimer = 0.0f;
            this.meshRenderer.enabled = false;
            this.ps.Stop();
            this.isTerminated = true;
        }

        public void update(EntityPlayer player, BlockPos lookingAt, Block block, byte meta) {
            if (this.isTerminated) {
                this.beginBreak(lookingAt.toVector(), block, meta);
                this.isTerminated = false;
            }

            ItemStack stack = player.dataHotbar.getHeldItem();
            ItemTool tool = null;
            float f = 1;
            if (stack != null && stack.item is ItemTool) {
                tool = (ItemTool)stack.item;
                if (tool.effectiveOn == block.blockType) {
                    f = tool.time;
                }
            }
            this.mineTimer += Time.deltaTime * f;
            if (block != Block.air) { //Hacky safety check
                if (this.mineTimer >= block.mineTime) {
                    player.world.breakBlock(lookingAt, tool);
                    this.mineTimer = 0.0f;
                }
            }
            else {
                //print("ERROR  We are trying to break air?");
            }
        }
    }
}
