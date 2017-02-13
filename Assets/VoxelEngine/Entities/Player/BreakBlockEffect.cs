using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Render.BlockRender;
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
        public void beginBreak(World world, int x, int y, int z, Block block, byte meta) {
            this.meshRenderer.enabled = true;
            MeshData meshData = new MeshData();

            BlockRenderer renderer = block.renderer;
            if(renderer != null && renderer.renderInWorld) {
                Block[] surroundingBlocks = new Block[6];
                for (int i = 0; i < 6; i++) {
                    Direction d = Direction.all[i];
                    surroundingBlocks[i] = world.getBlock(x + d.direction.x, y + d.direction.y, z + d.direction.z);
                }
                this.meshFilter.mesh = renderer.renderBlock(block, meta, meshData, 0, 0, 0, new bool[6] { true, true, true, true, true, true }, surroundingBlocks).toMesh();
            }

            this.transform.position = new Vector3(x, y, z);

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
                this.beginBreak(player.world, lookingAt.x, lookingAt.y, lookingAt.z, block, meta);
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
            if (this.mineTimer >= block.mineTime) {
                player.world.breakBlock(lookingAt, tool);
                this.mineTimer = 0.0f;
            }
        }
    }
}
