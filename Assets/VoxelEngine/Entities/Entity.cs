using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Entities {

    public abstract class Entity : MonoBehaviour {

        // State
        public int health;

        // References
        public World world;
        public Rigidbody rBody;

        protected Material entityMaterial;

        protected void Awake() {
            this.tag = "Entity";
            this.health = 10;
            this.rBody = this.GetComponent<Rigidbody>();
        }
        
        protected void Start() {
            if (this.getEntityId() != 1) { // Player entity id.
                this.entityMaterial = this.GetComponent<Renderer>().material;
            }
        }        

        private void OnCollisionEnter(Collision collision) {
            Entity otherEntity = collision.gameObject.GetComponent<Entity>();
            this.onEntityCollision(otherEntity);
        }

        private void Update() {
            if(!Main.singleton.isPaused) {
                this.onEntityUpdate();

                // Update the lighting on the entity
                if (this.entityMaterial != null) { // Player's don't need renderering and don't have their material set.
                    int lightLevel = this.world.getLight(
                        Mathf.RoundToInt(this.transform.position.x),
                        Mathf.RoundToInt(this.transform.position.y),
                        Mathf.RoundToInt(this.transform.position.z));
                    this.entityMaterial.SetColor(36, RenderManager.instance.lightHelper.getColorFromBrightness(lightLevel)); // 36 is the serialized form of _LightColor
                }
            }
        }

        public virtual void onEntityUpdate() {

        }

        public virtual void setHealth(int health) {
            this.health = health;
        }

        public virtual void onEntityCollision(Entity otherEntity) { }

        public virtual void onEntityHit(EntityPlayer player, float damage) { }

        public virtual void onEntityInteract(EntityPlayer player) { }

        //Returns true if the entity was killed by this damage
        public virtual bool damage(int amount, string message) {
            this.setHealth(this.health - amount);
            if (this.health <= 0) {
                this.world.killEntity(this);
                return true;
            }
            return false;
        }

        public virtual string getMagnifyingText() {
            return "Entity";
        }

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtByte("id", this.getEntityId()));
            tag.Add(new NbtInt("health", this.health));

            tag.Add(NbtHelper.writeVector3("position", this.transform.position));
            tag.Add(NbtHelper.writeVector3("rotation", this.transform.eulerAngles));
            tag.Add(NbtHelper.writeVector3("velocity", this.rBody.velocity));
            tag.Add(NbtHelper.writeVector3("angularVelocity", this.rBody.angularVelocity));

            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {
            this.health = tag.Get<NbtInt>("health").IntValue;

            this.transform.position = NbtHelper.readVector3(tag.Get<NbtCompound>("position"));
            this.transform.eulerAngles = NbtHelper.readVector3(tag.Get<NbtCompound>("rotation"));
            this.rBody.velocity = NbtHelper.readVector3(tag.Get<NbtCompound>("velocity"));
            this.rBody.angularVelocity = NbtHelper.readVector3(tag.Get<NbtCompound>("angularVelocity"));
        }

        public abstract byte getEntityId();
    }
}
