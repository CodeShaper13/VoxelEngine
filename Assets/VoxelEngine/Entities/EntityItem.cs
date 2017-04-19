using fNbt;
using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Render;

namespace VoxelEngine.Entities {

    public class EntityItem : Entity, ICollecting {

        public ItemStack stack;

        /// <summary> How long until the stack can be picked up or merge with another stack. </summary>
        private float pickupDelay;
        private MeshFilter filter;

        protected new void Awake() {
            base.Awake();
            this.filter = this.GetComponent<MeshFilter>();
            this.pickupDelay = 2f;
        }

        protected new void Start() {
            // Start will interact with the material, so call calculateMesh() first.
            this.calculateMesh(true);

            base.Start();
        }

        /// <summary>
        /// Creates and sets the items mesh.
        /// </summary>
        private void calculateMesh(bool lookupMaterial) {
            if(this.stack == null) {
                Debug.LogWarning("Items may not have a stack of null!  Killing Entity");
                this.world.killEntity(this);
            } else if(stack.item.id == 0) { // Air
                Debug.LogWarning("Items may not be air!  Killing Entity");
                this.world.killEntity(this);
            } else {
                int modelCount;
                if (this.stack.count >= 25) {
                    modelCount = 4;
                } else if (this.stack.count >= 17) {
                    modelCount = 3;
                } else if (this.stack.count >= 9) {
                    modelCount = 2;
                } else {
                    modelCount = 1;
                }

                this.filter.mesh = this.stack.item.getPreRenderedMesh(this.stack.meta);
                this.filter.mesh.RecalculateNormals();
                
                if(lookupMaterial) {
                    this.GetComponent<MeshRenderer>().material = RenderManager.getMaterial(this.stack.item.id);
                }
            }
        }

        public override void onEntityUpdate() {
            base.onEntityUpdate();

            this.transform.Rotate(0, Time.deltaTime * 25, 0);
            this.pickupDelay -= Time.deltaTime;

            for (int i = this.world.entityList.Count - 1; i >= 0; i--) {
                Entity entity = this.world.entityList[i];
                if (entity != this && entity is ICollecting && this.pickupDelay <= 0 && Vector3.Distance(this.transform.position, entity.transform.position) <= 0.5f) {
                    if((entity is EntityItem && ((EntityItem)entity).pickupDelay <= 0)) {
                        continue;
                    }
                    if (((ICollecting)entity).tryPickupStack(this.stack) == null) {
                        this.world.killEntity(this);
                        break;
                    }
                }
            }
        }

        public override string getMagnifyingText() {
            return "An item, bumping into it will let you pick it up.";
        }

        public override byte getEntityId() {
            return 2;
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(this.stack.writeToNbt());
            tag.Add(new NbtFloat("pickupDelay", this.pickupDelay));
            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);
            this.stack = new ItemStack(tag.Get<NbtCompound>("stack"));
            this.pickupDelay = tag.Get<NbtFloat>("pickupDelay").FloatValue;
        }

        public ItemStack tryPickupStack(ItemStack stack) {
            if(this.stack.equals(stack)) {
                ItemStack leftover = this.stack.merge(stack);
                this.calculateMesh(false);
                return leftover;
            } else {
                return stack;
            }
        }
    }
}
