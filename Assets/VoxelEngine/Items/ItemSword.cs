namespace VoxelEngine.Items {

    public class ItemSword : Item {

        public int damageAmount;

        public ItemSword(int id, int damage) : base(id) {
            this.damageAmount = damage;
        }
    }
}
