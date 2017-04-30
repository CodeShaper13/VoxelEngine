using fNbt;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Entities {

    public abstract class Entity : MonoBehaviour {

        // State
        public int health;
        private int maxHealth;

        // References
        public World world;
        public Rigidbody rBody;
        protected Material entityMaterial;

        private float shadowSize;
        private float shadowDarkness = 1f;
        private Transform shadowTransform;
        private Material shadowMaterial;

        // Don't override, use Entity.onConstruct
        private void Awake() {
            this.tag = "Entity";
            this.rBody = this.GetComponent<Rigidbody>();

            this.onConstruct();
        }

        protected void Start() {
            this.setHealth(this.maxHealth);

            // Get a reference to the Entity's material, unless they are the player who doesnt have one.
            if (this.getEntityId() != Main.singleton.player.getEntityId()) {
                this.entityMaterial = this.GetComponent<Renderer>().material;
            }

            // Create shadow.
            if(shadowSize != 0) {
                this.shadowTransform = GameObject.Instantiate(
                    References.list.shadowPrefab,
                    Vector3.zero,
                    Quaternion.Euler(90, 45, 0),
                    this.transform).transform;
                this.shadowTransform.localScale = new Vector3(this.shadowSize, this.shadowSize, this.shadowSize);
                this.shadowTransform.GetComponent<Renderer>().material.SetFloat(282, this.shadowDarkness); // 282 is the serialized form of _ShadowDarkness.
                this.shadowMaterial = this.shadowTransform.GetComponent<Renderer>().material;
            }
        }        

        // Don't override, use Entity.onEntityCollision().
        private void OnCollisionEnter(Collision collision) {
            Entity otherEntity = collision.gameObject.GetComponent<Entity>();
            this.onEntityCollision(otherEntity);
        }

        // Don't override, use Entity.onEntityUpdate().
        private void Update() {
            if(!Main.singleton.isPaused) {
                this.onEntityUpdate();

                int lightLevel = this.world.getLight(
                    Mathf.RoundToInt(this.transform.position.x),
                    Mathf.RoundToInt(this.transform.position.y),
                    Mathf.RoundToInt(this.transform.position.z));
                Color lightColor = RenderManager.instance.lightHelper.getColorFromBrightness(lightLevel);

                // Update the lighting on the entity
                if (this.entityMaterial != null) { // Player's don't need renderering and don't have their material set.
                    this.entityMaterial.SetColor(36, lightColor); // 36 is the serialized form of _LightColor.
                }

                // Update shadow.
                if(this.shadowTransform != null) {
                    RaycastHit hit;
                    // Ignore Entities and EntityItems, layers 9 and 10.
                    if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100, ~((1 << 9) | (1 << 10)))) {
                        this.shadowTransform.transform.position = hit.point + new Vector3(0, 0.0001f, 0);
                        this.shadowMaterial.SetColor(Shader.PropertyToID("_LightColor"), lightColor);
                        if(!this.shadowTransform.gameObject.activeSelf) {
                            this.shadowTransform.gameObject.SetActive(true);
                        }
                    } else {
                        // Hit nothing, hide shadow.
                        this.shadowTransform.gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// This should be used as a constructor for the entity and is equivalent to Awake()
        /// </summary>
        protected virtual void onConstruct() { }

        public virtual void onEntityUpdate() { }

        /// <summary>
        /// Allows entities to detect when there health changes.  Used by the player to update hud
        /// </summary>
        public virtual void setHealth(int amount) {
            if (amount > this.maxHealth) {
                amount = this.maxHealth;
            }
            this.health = amount;
        }

        /// <summary>
        /// Called when another Entity collides with this one.
        /// </summary>
        /// <param name="otherEntity"></param>
        public virtual void onEntityCollision(Entity otherEntity) { }

        /// <summary>
        /// Called when the player right clicks on this Entity.
        /// </summary>
        public virtual void onEntityInteract(EntityPlayer player) { }

        /// <summary>
        /// Returns true if the entity was killed by this damage.
        /// </summary>
        public virtual bool damage(int amount, string message) {
            this.setHealth(this.health - amount);
            if (this.health <= 0) {
                this.world.killEntity(this);
                return true;
            }
            return false;
        }

        public virtual string getMagnifyingText() {
            return "Entity.Unknown";
        }

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtInt("id", this.getEntityId()));
            tag.Add(new NbtInt("health", this.health));

            tag.Add(NbtHelper.writeVector3("position", this.transform.position));
            tag.Add(NbtHelper.writeVector3("rotation", this.transform.eulerAngles));
            tag.Add(NbtHelper.writeVector3("velocity", this.rBody.velocity));
            tag.Add(NbtHelper.writeVector3("angularVelocity", this.rBody.angularVelocity));

            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {
            this.health = tag.Get<NbtInt>("health").IntValue;

            this.transform.position = NbtHelper.readVector3(tag.Get<NbtCompound>("position"));
            this.transform.eulerAngles = NbtHelper.readVector3(tag.Get<NbtCompound>("rotation"));
            this.rBody.velocity = NbtHelper.readVector3(tag.Get<NbtCompound>("velocity"));
            this.rBody.angularVelocity = NbtHelper.readVector3(tag.Get<NbtCompound>("angularVelocity"));
        }

        public abstract int getEntityId();

        /// <summary>
        /// Sets the entities max health.  To be used in the constructor.
        /// </summary>
        public void setMaxHealth(int max) {
            this.maxHealth = max;
        }

        public void setShadow(float size, float darkness) {
            this.shadowSize = size;
            this.shadowDarkness = darkness;
        }
    }
}
