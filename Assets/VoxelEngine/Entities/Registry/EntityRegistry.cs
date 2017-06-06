using UnityEngine;

namespace VoxelEngine.Entities.Registry {

    public class EntityRegistry {

        public static RegisteredEntity[] cachedRegistries;

        public static RegisteredEntity player;
        public static RegisteredEntity item;
        public static RegisteredEntity throwable;

        public EntityRegistry() {
            EntityRegistry.cachedRegistries = new RegisteredEntity[256];
        }

        /// <summary>
        /// Called from Main.Awake to register all the entites.
        /// </summary>
        public void registerEntities() {
            EntityRegistry.player = new RegisteredEntity("EntityPlayerPrefab");
            EntityRegistry.item = new RegisteredEntity("EntityItemPrefab");
            EntityRegistry.throwable = new RegisteredEntity("EntityThrowablePrefab");
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
    }
}