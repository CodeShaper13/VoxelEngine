using VoxelEngine.Entities.Registry;

namespace VoxelEngine.Items {

    public class ItemDynamite : ItemThrowable, IExplosiveObject {

        public ItemDynamite(int id) : base(id, EntityRegistry.thrownDynamite) { }

        protected override float getThrowSpeed() {
            return 10f;
        }

        public float getExplosionSize() {
            return 4f;
        }
    }
}
