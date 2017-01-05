using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.Entities {

    public class Entity : MonoBehaviour {
        public World world;
        public int health;

        public void Awake() {
            this.tag = "Entity";
            this.health = 10;
        }

        void OnCollisionEnter(Collision collision) {
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
    }
}
