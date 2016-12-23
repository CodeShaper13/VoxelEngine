using System;
using UnityEngine;

public class Entity : MonoBehaviour {
    public World world;
    protected int health;

    public void Awake() {
        this.health = 10;
    }

    public void Update() {
    }

    void OnCollisionEnter(Collision collision) {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity != null) {
            this.onEntityCollision(entity);
            entity.onEntityCollision(this);
        }
    }


    public virtual int getHealth() {
        return this.health;
    }

    public virtual void setHealth(int health) {
        this.health = health;
    }

    public virtual void onEntityCollision(Entity otherEntity) {

    }

    public virtual void onEntityHit(EntityPlayer player) {
        throw new NotImplementedException();
    }

    public virtual void onEntityInteract(EntityPlayer player) {
        throw new NotImplementedException();
    }
}
