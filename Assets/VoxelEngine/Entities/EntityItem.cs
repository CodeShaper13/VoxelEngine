using fNbt;
using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Render;


namespace VoxelEngine.Entities {

    public class EntityItem : Entity, ICollecting {

        private ItemStack stack;
        /// <summary> How long the stack has been around.  Used to determine if it can be picked up or should dewpawn. </summary>
        private float timeAlive;
        private MeshFilter filter;

        public override void onConstruct() {
            this.filter = this.GetComponent<MeshFilter>();
            this.timeAlive = -2f;

            this.setShadow(0.6f, 0.75f);
        }

        protected override void onPostConstruct() {
            // Needs this.stack to be set, so call it here instead of onConstruct()
            this.calculateMesh();
        }

        protected override void onEntityUpdate() {
            this.transform.Rotate(0, Time.deltaTime * 25, 0);
            this.timeAlive += Time.deltaTime;

            if(this.timeAlive >= 300) { // 5 minutes.
                this.world.killEntity(this);
            } else {
                // Try and combine with another EntityItem or get picked up by a player. 
                ICollecting iCollecting;
                Entity otherEntity;
                for (int i = this.world.entityList.Count - 1; i >= 0; i--) {
                    otherEntity = this.world.entityList[i];
                    if (otherEntity != this && otherEntity is ICollecting) {
                        iCollecting = (ICollecting)otherEntity;
                        if (Vector3.Distance(this.transform.position, otherEntity.transform.position) <= iCollecting.getPickupRadius()) {
                            if (this.timeAlive >= 0) {
                                if (iCollecting.tryPickupStack(this.stack) == null) {
                                    this.world.killEntity(this);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string getMagnifyingText() {
            return "An item, bumping into it will let you pick it up.";
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
            tag.Add(this.stack.writeToNbt());
            tag.Add(new NbtFloat("pickupDelay", this.timeAlive));
            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);
            this.stack = new ItemStack(tag.Get<NbtCompound>("stack"));
            this.timeAlive = tag.Get<NbtFloat>("pickupDelay").FloatValue;
        }

        public ItemStack tryPickupStack(ItemStack stack) {
            if(this.timeAlive > 0) {
                return stack;
            }
            if(this.stack.Equals(stack)) {
                // Stacks are the same, we might be able to merge.
                ItemStack leftover = this.stack.merge(stack);
                this.calculateMesh();
                return leftover;
            } else {
                return stack;
            }
        }

        public float getPickupRadius() {
            return 0.5f;
        }

        public void setStack(ItemStack stack) {
            if(stack == null) {
                Debug.Log("Can't set EntityItem.stack to null!");
            }
            this.stack = stack;
            float scale = this.stack.item.id <= 255 ? 0.35f : 0.5f;
            this.transform.localScale = new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// Creates and sets the items mesh.
        /// </summary>
        private void calculateMesh() {
            if (this.stack == null) {
                Debug.LogWarning("Items may not have a stack of null!  Killing Entity");
                this.world.killEntity(this);
            }
            else if (stack.item.id == 0) { // Air
                Debug.LogWarning("Items may not be air!  Killing Entity");
                this.world.killEntity(this);
            }
            else {
                int modelCount;
                if (this.stack.count >= 25) {
                    modelCount = 4;
                }
                else if (this.stack.count >= 17) {
                    modelCount = 3;
                }
                else if (this.stack.count >= 9) {
                    modelCount = 2;
                }
                else {
                    modelCount = 1;
                }

                this.filter.mesh = RenderManager.getItemMesh(this.stack.item, this.stack.meta, true);
                this.filter.mesh.RecalculateNormals();
            }
        }

        public static Quaternion randomRotation() {
            return Quaternion.Euler(0, Random.Range(0, 359), 0);
        }

        public static Vector3 randomForce(float range) {
            return new Vector3(Random.Range(-range, range), Random.Range(0, range), Random.Range(-range, range));
        }
    }
}
