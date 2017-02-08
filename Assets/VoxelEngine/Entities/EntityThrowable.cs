using System;
using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Render.Items;

namespace VoxelEngine.Entities {

    public class EntityThrowable : Entity {

        public new void Awake() {
            base.Awake();

            IRenderItem r = Item.pebble.itemRenderer;
            Mesh mesh = r.renderItem(new ItemStack(Item.pebble));
            this.GetComponent<MeshFilter>().mesh = mesh;
            this.GetComponent<MeshRenderer>().material = References.list.itemMaterial;
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
