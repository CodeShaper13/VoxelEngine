namespace VoxelEngine.Items {

    public class ItemSword : Item {

        public float damageAmount;

        public ItemSword(float damage) : base() {
            this.damageAmount = damage;
        }
    }
}
