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

        /// <summary>
        /// Begins the breaking of a block.
        /// </summary>
        public void beginBreak(World world, int x, int y, int z, Block block, int meta) {
            this.meshRenderer.enabled = true;
            MeshBuilder meshBuilder = RenderManager.getMeshBuilder();

            BlockRenderer renderer = block.renderer;
            if(renderer != null && renderer.bakeIntoChunks) {
                Block[] surroundingBlocks = new Block[6];
                for (int i = 0; i < 6; i++) {
                    Direction d = Direction.all[i];
                    surroundingBlocks[i] = world.getBlock(x + d.blockPos.x, y + d.blockPos.y, z + d.blockPos.z);
                }
                renderer.renderBlock(block, meta, meshBuilder, 0, 0, 0, RenderFace.ALL, surroundingBlocks);
                this.meshFilter.mesh = meshBuilder.getGraphicMesh();
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
            if(!this.ps.isStopped) {
                this.ps.Stop();
            }
            this.isTerminated = true;
        }

        public void update(EntityPlayer player, BlockPos lookingAt, Block block, int meta) {
            if (this.isTerminated) {
                this.beginBreak(player.world, lookingAt.x, lookingAt.y, lookingAt.z, block, meta);
                this.isTerminated = false;
            }

            ItemStack stack = player.containerHotbar.getHeldItem();
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
