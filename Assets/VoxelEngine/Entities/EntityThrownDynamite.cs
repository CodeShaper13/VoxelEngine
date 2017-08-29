using VoxelEngine.Items;

namespace VoxelEngine.Entities {

    public class EntityThrownDynamite : EntityThrowable {

        public override void onEntityCollision(Entity otherEntity) {
            this.world.makeExplosion((IExplosiveObject)Item.dynamite, this.transform.position);

            this.world.killEntity(this);
        }

        public override Item getItemToRenderAs() {
            return Item.dynamite;
        }
    }
}
