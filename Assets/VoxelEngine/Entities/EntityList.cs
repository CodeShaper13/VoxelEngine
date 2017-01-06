using System;
using UnityEngine;

namespace VoxelEngine.Entities {

    public class EntityList : MonoBehaviour {
        public static EntityList singleton;

        public GameObject playerPrefab;
        public GameObject itemPrefab;
        public GameObject throwablePrefab;

        void Awake() {
            if (EntityList.singleton == null) {
                EntityList.singleton = this;
            }
            else {
                throw new Exception("There can only be one EntityManager script!");
            }
        }

        public static GameObject getPrefabFromId(byte id) {
            switch(id) {
                case 1:
                    return EntityList.singleton.playerPrefab;
                case 2:
                    return EntityList.singleton.itemPrefab;
                case 3:
                    return EntityList.singleton.throwablePrefab;
            }
            return null;
        }
    }
}