using UnityEngine;
using VoxelEngine.Items;
using VoxelEngine.Render;

namespace VoxelEngine.Entities {

    public class EntityThrowable : Entity {

        public override void onConstruct() {
            // Set the entity's mesh.
            this.GetComponent<MeshFilter>().mesh = RenderManager.getItemMesh(Item.pebble, 0, true);
        }

        public override void onEntityCollision(Entity otherEntity) {
            base.onEntityCollision(otherEntity);

            if (otherEntity is EntityLiving) {
                ((EntityLiving)otherEntity).damage(1, "Smacked by a Flying Pebble!");
            }
            this.world.killEntity(this);
        }
    }
}
