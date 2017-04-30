using UnityEngine;
using VoxelEngine.Items;

namespace VoxelEngine.Entities {

    public class EntityThrowable : Entity {

        protected override void onConstruct() {
            base.onConstruct();

            // Set the entity's mesh.
            this.GetComponent<MeshFilter>().mesh = Item.pebble.getPreRenderedMesh(0);
        }

        public override void onEntityCollision(Entity otherEntity) {
            base.onEntityCollision(otherEntity);

            if (otherEntity != null) {
                otherEntity.damage(1, "Smacked by a Flying Pebble!");
            }
            this.world.killEntity(this);
        }

        public override int getEntityId() {
            return 3;
        }
    }
}
