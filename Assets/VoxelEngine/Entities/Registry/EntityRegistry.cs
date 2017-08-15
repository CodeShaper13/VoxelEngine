using System;
using UnityEngine;

namespace VoxelEngine.Entities.Registry {

    public class EntityRegistry {

        public static RegisteredEntity player;
        public static RegisteredEntity item;
        public static RegisteredEntity throwable;
        public static RegisteredEntity dynamicBlock;

        private static RegisteredEntity[] registry;

        public EntityRegistry() {
            EntityRegistry.registry = new RegisteredEntity[32];
        }

        /// <summary>
        /// Called from Main.Awake to register all the entites.
        /// </summary>
        public void registerEntities() {
            EntityRegistry.player = new RegisteredEntity(1, "EntityPlayerPrefab");
            EntityRegistry.item = new RegisteredEntity(2, "EntityItemPrefab");
            EntityRegistry.throwable = new RegisteredEntity(3, "EntityThrowablePrefab");
            EntityRegistry.dynamicBlock = new RegisteredEntity(4, "EntityDynamicBlockPrefab");
        }

        /// <summary>
        /// Returns an EntityPrefab ready to instantiate from an id.
        /// </summary>
        public static GameObject getEntityPrefabFromId(int id) {
            RegisteredEntity re = EntityRegistry.registry[id];
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
            for (int i = 0; i < EntityRegistry.registry.Length; i++) {
                re = EntityRegistry.registry[i];
                if(t == re.getType()) {
                    return re.getId();
                }
            }
            return -1;
        }

        public static RegisteredEntity getRegisteredEntityFromId(int id) {
            return EntityRegistry.registry[id];
        }

        public static void addToRegistry(RegisteredEntity registeredEntity) {
            EntityRegistry.registry[registeredEntity.getId()] = registeredEntity;
        }
    }
}