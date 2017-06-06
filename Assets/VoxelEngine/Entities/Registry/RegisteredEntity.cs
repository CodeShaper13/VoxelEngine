using System;
using UnityEngine;

namespace VoxelEngine.Entities.Registry {

    public class RegisteredEntity {

        private GameObject prefab;
        private int id;

        public RegisteredEntity(string prefabName) {
            this.prefab = Resources.Load<GameObject>("Prefabs/Entities/" + prefabName);
            if (this.prefab == null) {
                throw new Exception("Entity Prefab with name " + prefabName + " could not be found!");
            }
            this.id = this.prefab.GetComponent<Entity>().getEntityId();
            if (EntityRegistry.cachedRegistries[this.id] != null) {
                throw new Exception("Two entities have the same id of " + this.id + "!");
            }
            EntityRegistry.cachedRegistries[this.id] = this;
        }

        public GameObject getPrefab() {
            return this.prefab;
        }
    }
}
