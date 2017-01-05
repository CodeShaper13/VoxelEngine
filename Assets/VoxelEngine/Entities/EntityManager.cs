using System;
using UnityEngine;

namespace VoxelEngine.Entities {

    public class EntityManager : MonoBehaviour {
        public static EntityManager singleton;

        public GameObject playerPrefab;
        public GameObject itemPrefab;
        public GameObject throwablePrefab;

        void Awake() {
            if (EntityManager.singleton == null) {
                EntityManager.singleton = this;
            }
            else {
                throw new Exception("There can only be one EntityManager script!");
            }
        }
    }
}