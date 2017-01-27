namespace VoxelEngine.Items {

    public class ItemSword : Item {

        public float damageAmount;

        public ItemSword(int id, float damage) : base(id) {
            this.damageAmount = damage;
        }
    }
}
