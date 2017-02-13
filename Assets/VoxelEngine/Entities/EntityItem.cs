using fNbt;
using UnityEngine;
using VoxelEngine.Containers;

namespace VoxelEngine.Entities {

    public class EntityItem : Entity {
        public ItemStack stack;
        public float pickupDelay;

        private MeshFilter filter;

        public new void Awake() {
            base.Awake();
            this.filter = this.GetComponent<MeshFilter>();
            this.pickupDelay = 1f;
        }

        public void Start() {
            this.initRendering();
        }

        public void initRendering() {
            if(this.stack == null) {
                Debug.LogWarning("Items may not have a stack of null!  Killing Entity");
                this.world.killEntity(this);
            } else if(stack.item.id == 0) { // Air
                Debug.LogWarning("Items may not be air!  Killing Entity");
                this.world.killEntity(this);
            } else {
                this.filter.mesh = this.stack.item.getPreRenderedMesh(this.stack.meta);
                this.filter.mesh.RecalculateNormals();
                this.GetComponent<MeshRenderer>().material = References.getMaterial(this.stack.item.id);
            }
        }

        public override void onEntityUpdate() {
            base.onEntityUpdate();
            this.transform.Rotate(0, Time.deltaTime * 25, 0);
            this.pickupDelay -= Time.deltaTime;
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
    }
}
