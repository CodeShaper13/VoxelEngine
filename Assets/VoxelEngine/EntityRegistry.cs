using System;
using UnityEngine;
using VoxelEngine.Entities;

namespace VoxelEngine {

    public class EntityRegistry {

        private static RegisteredEntity[] cachedRegistries;

        public static RegisteredEntity player;
        public static RegisteredEntity item;
        public static RegisteredEntity throwable;

        public EntityRegistry() {
            EntityRegistry.cachedRegistries = new RegisteredEntity[256];
        }

        public void registerEntities() {
            player = new RegisteredEntity("EntityPlayerPrefab");
            item = new RegisteredEntity("EntityItemPrefab");
            throwable = new RegisteredEntity("EntityThrowablePrefab");
        }

        public static GameObject getEntityPrefabFromId(int id) {
            RegisteredEntity re = EntityRegistry.cachedRegistries[id];
            if(re != null) {
                return re.prefab;
            } else {
                return null;
            }
        }

        public class RegisteredEntity {

            public GameObject prefab;
            private int id;

            public RegisteredEntity(string prefabName) {
                this.prefab = Resources.Load<GameObject>("Prefabs/Entities/" + prefabName);
                if(this.prefab == null) {
                    throw new Exception("Entity Prefab with name " + prefabName + " could not be found!");
                }
                this.id = this.prefab.GetComponent<Entity>().getEntityId();
                if(EntityRegistry.cachedRegistries[this.id] != null) {
                    throw new Exception("Two entities have the same id of " + this.id + "!");
                }
                EntityRegistry.cachedRegistries[this.id] = this;
            }
        }
    }
}