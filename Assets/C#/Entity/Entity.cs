using System;
using UnityEngine;

public class Entity : MonoBehaviour {

    public World world;

    public virtual void onPlayerTouch(Player player) {
    }

    public virtual void onEntityHit(Player player) {
        throw new NotImplementedException();
    }

    public virtual void onEntityInteract(Player player) {
        throw new NotImplementedException();
    }
}
