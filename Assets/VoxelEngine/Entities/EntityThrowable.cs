using UnityEngine;
using VoxelEngine.Items;

namespace VoxelEngine.Entities {

    public class EntityThrowable : Entity {

        protected new void Awake() {
            base.Awake();

            this.GetComponent<MeshFilter>().mesh = Item.pebble.getPreRenderedMesh(0);
        }

        public override void onEntityCollision(Entity otherEntity) {
            base.onEntityCollision(otherEntity);

            if (otherEntity != null) {
                otherEntity.damage(1, "Smacked by a Flying Pebble!");
            }
            this.world.killEntity(this);
        }

        public override byte getEntityId() {
            return 3;
        }
    }
}
