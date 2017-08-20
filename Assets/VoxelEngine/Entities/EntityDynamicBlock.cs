using fNbt;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Containers;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Entities {

    public class EntityDynamicBlock : Entity {

        private Block block;
        private int meta;
        private NbtCompound blockNbt;
        private BlockPos startPos;

        private float timeSinceMovement;
        private Vector3 lastPos;

        public override void onConstruct() {
            this.rBody.constraints = RigidbodyConstraints.FreezeRotation;

            this.startPos = BlockPos.fromVector3(this.transform.position);
        }

        protected override void onEntityUpdate() {
            if(this.transform.position != this.lastPos) {
                this.timeSinceMovement = 0f;
                this.lastPos = this.transform.position;
            } else {
                this.timeSinceMovement += Time.deltaTime;
            }

            if(this.timeSinceMovement > 0.1f) {
                this.world.setBlock(BlockPos.fromVector3(this.transform.position), this.block, this.meta);
                this.world.killEntity(this);
            }
        }

        public override void onEntityCollision(Entity otherEntity) {
            if(otherEntity is EntityLiving) {
                Vector3 p = this.transform.position;
                float damage =
                    (Mathf.Abs(this.startPos.x - p.x) +
                    Mathf.Abs(this.startPos.x - p.x) +
                    Mathf.Abs(this.startPos.x - p.x)) * 2;
                ((EntityLiving)otherEntity).damage((int)damage, "entity.falling.damage");
                this.dropAsItem();
            }
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.Add(new NbtInt("blockId", this.block.id));
            tag.Add(new NbtInt("meta", this.meta));
            if(this.blockNbt != null) {
                tag.Add(new NbtCompound("blockNbt", this.blockNbt));
            }
            NbtHelper.writeDirectBlockPos(tag, this.startPos, "start");

            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.setTile(
                Block.getBlockFromId(tag.Get<NbtInt>("blockId").Value),
                tag.Get<NbtInt>("meta").Value);

            tag.TryGet<NbtCompound>("blockNbt", out this.blockNbt);
            this.startPos = NbtHelper.readDirectBlockPos(tag, "start");
        }

        public void setTile(Block block, int meta) {
            this.block = block;
            this.meta = meta;

            MeshBuilder meshBuilder = RenderManager.getMeshBuilder();
            meshBuilder.setMaxLight();
            this.block.renderer.renderBlock(this.block, this.meta, meshBuilder, 0, 0, 0, RenderFace.ALL, RenderManager.AIR_ARRAY);
            this.GetComponent<MeshFilter>().mesh = meshBuilder.getGraphicMesh();
        }

        private void dropAsItem() {
            this.world.spawnItem(new ItemStack(this.block, this.meta), this.transform.position, EntityItem.randomRotation(), EntityItem.randomForce(1f));
            this.world.killEntity(this);
        }
    }
}
