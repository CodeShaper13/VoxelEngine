using System;
using UnityEngine;

namespace VoxelEngine.Entities.Registry {

    public class RegisteredEntity {

        /// <summary>
        /// Prefab to instantiate when spawning the entity.
        /// </summary>
        private GameObject prefab;
        /// <summary>
        /// Id for the entity for saving to disk.  No duplicates.
        /// </summary>
        private int id;
        private Type type;

        public RegisteredEntity(int id, string prefabName) {
            this.prefab = Resources.Load<GameObject>("Entities/" + prefabName);
            if (this.prefab == null) {
                throw new Exception("Entity Prefab with name " + prefabName + " could not be found!");
            }
            this.id = id;
            if (EntityRegistry.getRegisteredEntityFromId(this.id) != null) {
                throw new Exception("Two entities have the same id of " + this.id + "!");
            }
            this.type = this.prefab.GetComponent<Entity>().GetType();
            EntityRegistry.addToRegistry(this);
        }

        public GameObject getPrefab() {
            return this.prefab;
        }

        public int getId() {
            return this.id;
        }

        public Type getType() {
            return this.type;
        }
    }
}
