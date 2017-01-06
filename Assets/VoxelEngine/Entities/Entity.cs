using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Entities {

    public abstract class Entity : MonoBehaviour {
        public World world;
        public int health;

        public void Awake() {
            this.tag = "Entity";
            this.health = 10;
        }

        public void OnCollisionEnter(Collision collision) {
            Entity otherEntity = collision.gameObject.GetComponent<Entity>();
            this.onEntityCollision(otherEntity);
        }

        public virtual void onEntityUpdate() {

        }

        public virtual void setHealth(int health) {
            this.health = health;
        }

        public virtual void onEntityCollision(Entity otherEntity) { }

        public virtual void onEntityHit(EntityPlayer player, float damage) { }

        public virtual void onEntityInteract(EntityPlayer player) { }

        //Retunrs true if the entity was kiled by this damage
        public bool damage(int amount) {
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

            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {
            this.health = tag.Get<NbtInt>("health").IntValue;

            this.transform.position = NbtHelper.readVector3(tag.Get<NbtCompound>("position"));
            this.transform.eulerAngles = NbtHelper.readVector3(tag.Get<NbtCompound>("rotation"));
        }

        public abstract byte getEntityId();
    }
}
