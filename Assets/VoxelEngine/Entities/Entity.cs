using fNbt;
using UnityEngine;
using VoxelEngine.Entities.Registry;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Entities {

    public abstract class Entity : MonoBehaviour {

        // References
        [HideInInspector]
        public World world;
        [HideInInspector]
        public Rigidbody rBody;
        protected Material entityMaterial;

        private float shadowSize;
        private float shadowDarkness = 1f;
        private Transform shadowTransform;
        private Material shadowMaterial;

        // Don't override!!!  Use Entity.onConstruct().
        private void Awake() {
            if(!this.CompareTag(Tags.ENTITY)) {
                print("Entity found without tag \"Entity\"!  Is that normal?");
            }
            this.rBody = this.GetComponent<Rigidbody>();
        }

        // Don't override!!!  Use Entity.onPostConstrcut().
        protected void Start() {
            this.onPostConstruct();

            // Get a reference to the Entity's material, unless they are the player who doesnt have one.
            if (!(this is EntityPlayer)) {
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

        // Don't override!!!  Use Entity.onEntityCollision().
        private void OnCollisionEnter(Collision collision) {
            Entity otherEntity = collision.gameObject.GetComponent<Entity>();
            this.onEntityCollision(otherEntity);
        }

        // Don't override!!!  Use Entity.onEntityUpdate().
        private void Update() {
            if(!Main.singleton.isPaused) {
                this.onEntityUpdate();

                int lightLevel = this.world.getLight(
                    Mathf.RoundToInt(this.transform.position.x),
                    Mathf.RoundToInt(this.transform.position.y),
                    Mathf.RoundToInt(this.transform.position.z));
                Color lightColor = RenderManager.instance.lightColors.getColorFromBrightness(lightLevel);

                // Update the lighting on the entity's model.
                if (this.entityMaterial != null) { // Player's don't need renderering and don't have their material set.
                    this.entityMaterial.SetColor(36, lightColor); // 36 is the serialized form of _LightColor.
                }

                // Update shadow.
                if(this.shadowTransform != null) {
                    RaycastHit hit;
                    // Ignore Entities, EntityItems and IslandMesh, layers 9, 10 and 11.
                    if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 20, ~(Layers.ENTITY | Layers.ENTITY_ITEM | Layers.ISLAND_MESH))) {
                        this.shadowTransform.position = hit.point + new Vector3(0, 0.0001f, 0);
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

        public virtual void onConstruct() { }

        protected virtual void onPostConstruct() { }

        protected virtual void onEntityUpdate() { }

        /// <summary>
        /// Called when another Entity collides with this one.  Entity may be null if this hit the world.
        /// </summary>
        public virtual void onEntityCollision(Entity otherEntity) { }

        /// <summary>
        /// Called when the player right clicks on this Entity.
        /// </summary>
        public virtual void onEntityInteract(EntityPlayer player) { }

        public virtual string getMagnifyingText() {
            return "Entity.Unknown";
        }

        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtInt("id", EntityRegistry.getIdFromEntity(this)));

            tag.Add(NbtHelper.writeVector3("position", this.transform.position));
            tag.Add(NbtHelper.writeVector3("rotation", this.transform.eulerAngles));
            tag.Add(NbtHelper.writeVector3("velocity", this.rBody.velocity));
            tag.Add(NbtHelper.writeVector3("angularVelocity", this.rBody.angularVelocity));

            return tag;
        }

        public virtual void readFromNbt(NbtCompound tag) {
            this.transform.position = NbtHelper.readVector3(tag.Get<NbtCompound>("position"));
            this.transform.eulerAngles = NbtHelper.readVector3(tag.Get<NbtCompound>("rotation"));
            this.rBody.velocity = NbtHelper.readVector3(tag.Get<NbtCompound>("velocity"));
            this.rBody.angularVelocity = NbtHelper.readVector3(tag.Get<NbtCompound>("angularVelocity"));
        }

        /// <summary>
        /// Call from onConstruct to set the shadow size and darkness of the entity.
        /// </summary>
        public void setShadow(float size, float darkness) {
            this.shadowSize = size;
            this.shadowDarkness = darkness;
        }

        public override string ToString() {
            return "Entity Info:\n\tGame Object Name: " + this.name + "\n\tPosition: " + this.transform.position;
        }
    }
}
