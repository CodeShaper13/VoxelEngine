using System;
using UnityEngine;

namespace VoxelEngine.Entities.Registry {

    public class EntityRegistry {

        public static RegisteredEntity[] cachedRegistries;

        public static RegisteredEntity player;
        public static RegisteredEntity item;
        public static RegisteredEntity throwable;

        public EntityRegistry() {
            EntityRegistry.cachedRegistries = new RegisteredEntity[64];
        }

        /// <summary>
        /// Called from Main.Awake to register all the entites.
        /// </summary>
        public void registerEntities() {
            EntityRegistry.player = new RegisteredEntity(1, "EntityPlayerPrefab");
            EntityRegistry.item = new RegisteredEntity(2, "EntityItemPrefab");
            EntityRegistry.throwable = new RegisteredEntity(3, "EntityThrowablePrefab");
        }

        /// <summary>
        /// Returns an EntityPrefab ready to instantiate from an id.
        /// </summary>
        public static GameObject getEntityPrefabFromId(int id) {
            RegisteredEntity re = EntityRegistry.cachedRegistries[id];
            if(re != null) {
                return re.getPrefab();
            } else {
                return null;
            }
        }

        /// <summary>
        /// Returns the id of the passed entity, or -1 on error.
        /// </summary>
        public static int getIdFromEntity(Entity entity) {
            Type t = entity.GetType();
            RegisteredEntity re;
            for (int i = 0; i < EntityRegistry.cachedRegistries.Length; i++) {
                re = EntityRegistry.cachedRegistries[i];
                if(t == re.getType()) {
                    return re.getId();
                }
            }
            return -1;
        }
    }
}